using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Provider;
using SecurityUtil;
using SecurityUtil.Keys;
using SharedResources;
using SharedResources.Views;
using WebPortal.Extensions;
using WebPortal.Helpers;
using WebPortal.Models;

namespace WebPortal.Controllers
{
    public class ImageController : Controller
    {
        #region Field

        private static ImageFormats _imageFormats;
        private readonly HttpClient _client;
        private readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();

        #endregion Field

        #region Constructor

        public ImageController()
        {
            _client = HttpClientHelpers.GetApiClient(Properties.Settings.Default.SSDAPI);
            // file types white list
            _imageFormats = AcceptedImageFormat.GetConfig();
        }

        #endregion Constructor

        public AppUserManager GetUserManager => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

        // GET: Image/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Image/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return View();
            }
            catch
            {
                return View();
            }
        }

        public FileResult Download(string id)
        {
            AppUser user = GetUserManager.FindByNameAsync(User.Identity.Name).Result;
            HttpResponseMessage imageResponse = _client.GetAsync($"api/Image/{id}").Result;

            if (imageResponse.IsSuccessStatusCode)
            {
                string responseString = imageResponse.Content.ReadAsStringAsync().Result;
                VwImage img = _serializer.Deserialize<VwImage>(responseString);

                byte[] encryptedFile = EncryptionUtil.ReadFile(Server.MapPath(img.Path));

                if (encryptedFile != null)
                {
                    // Verify digital signature
                    var isNotModified = EncryptionUtil.VerifyFile(encryptedFile, user.PublicKey, img.Signature);

                    if (isNotModified)
                    {
                        // decrypt
                        var symparam = new SymmetricParameters();
                        symparam.SecretKey = EncryptionUtil.Decrypt(Convert.FromBase64String(img.ESecretKey), user.PrivateKey);
                        symparam.IV = EncryptionUtil.Decrypt(Convert.FromBase64String(img.EncryptedIV), user.PrivateKey);

                        byte[] decryptedFile = EncryptionUtil.DecryptFile(symparam, encryptedFile);

                        return File(decryptedFile, "application/force-download", $"{img.Title}{Path.GetExtension(img.Path)}");
                    }
                }

                throw new HttpException(404, "Image file not found");
            }
            return null;
        }

        public PartialViewResult Upload()
        {
            var model = new ImageModel { Albums = GetAlbumList() };
            return PartialView("_Upload", model);
        }

        [HttpPost]
        public ActionResult Upload(ImageModel model)
        {
            model.Albums = GetAlbumList();
            if (model.AlbumId == 0)
            {
                ModelState.AddModelError("", "Please select an album");

                return UploadFailed(model);
            }

            if (model.File == null)
            {
                ModelState.AddModelError("", new ArgumentNullException(nameof(model.File)));
                return UploadFailed(model);
            }

            if (model.File.ContentLength == 0)
            {
                ModelState.AddModelError("", new ArgumentNullException(nameof(model.File)));
                return UploadFailed(model);
            }

            int maxRequestLength = Convert.ToInt32(ConfigurationManager.AppSettings["maxRequestLength"]);

            if (model.File.ContentLength > maxRequestLength)
            {
                ModelState.AddModelError("", $"File size limit exceeded. Upload files less than {maxRequestLength}kilo bytes.");
                return UploadFailed(model);
            }

            if (!model.File.IsExpectedFormat(_imageFormats))
            {
                ModelState.AddModelError("", "File format not supported");
                return UploadFailed(model);
            }

            AppUser user = GetUserManager.FindByNameAsync(User.Identity.Name).Result;
            var symmParam = EncryptionUtil.GenerateSymmetricParameters(user.PasswordHash, Guid.NewGuid().ToString());

            string newFileName = Guid.NewGuid().ToString();
            string extention = Path.GetExtension(model.File.FileName);
            string imageFileName = $"{newFileName}{extention}";

            var imagePath = Path.Combine("/Uploads/", imageFileName);
            var thumbPath = Path.Combine("/Thumbs/", imageFileName);

            CreateThumbNail(model, thumbPath);

            model.File.InputStream.Position = 0;
            byte[] fileData = new byte[model.File.InputStream.Length];
            model.File.InputStream.Read(fileData, 0, fileData.Length);

            // symmetric encryption
            byte[] encryptedFile = EncryptionUtil.EncryptFile(symmParam, fileData);

            // asymmetric encryption
            var encryptedSecret = EncryptionUtil.Encrypt(symmParam.SecretKey, user.PublicKey);
            var encryptedIV = EncryptionUtil.Encrypt(symmParam.IV, user.PublicKey);

            // Digital signing
            string fileSignature = EncryptionUtil.SignFile(encryptedFile, user.PrivateKey);

            string fullPath = Server.MapPath(imagePath);
            EncryptionUtil.WriteBytesToFile(fullPath, encryptedFile);

            // save model and path to database
            using (_client)
            {
                var image = new VwImage();
                var date = DateTime.Now;

                image.Title = model.Title;
                image.Description = model.Description;
                image.Path = imagePath;
                image.ThumbNail = thumbPath;
                image.ModifiedOn = date;
                image.CreationDate = date;
                image.UserId = User.Identity.GetUserId();
                image.AlbumId = model.AlbumId;
                image.ESecretKey = encryptedSecret;
                image.EncryptedIV = encryptedIV;
                image.Signature = fileSignature;

                HttpResponseMessage responseMessage = _client.PostAsJsonAsync("api/Image", image).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    return Redirect($"/Album/Open/{model.AlbumId}");
                }
                ModelState.AddModelError(string.Empty, "Failed to add a new Image");
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Upload", model) : View("Upload", model);
        }

        private void CreateThumbNail(ImageModel model, string thumbPath)
        {
            // Create unencrypted thumbnail
            model.File.InputStream.Position = 0;
            Image photo = Image.FromStream(model.File.InputStream);

            int resizeToWidth = 600, resizeToHeight = 500;

            Image thumb = photo.GetThumbnailImage(resizeToWidth, resizeToHeight, () => false, IntPtr.Zero);

            thumb.Save(Server.MapPath(thumbPath));
        }

        private SelectList GetAlbumList()
        {
            HttpResponseMessage responseMessage = _client.GetAsync("/api/My/Albums?userId=" + User.Identity.GetUserId()).Result;

            if (responseMessage == null)
            {
                throw new NullReferenceException("ImageController>GetAlbumList: responseMessage is null");
            }

            var responseData = responseMessage.Content.ReadAsStringAsync().Result;

            var albums = _serializer.Deserialize<List<AlbumModel>>(responseData);

            var selist = new SelectList(albums, "ID", "Title", 0);

            return selist;
        }

        private ActionResult UploadFailed(ImageModel model)
        {
            model.File = null;
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_Upload", model) : View("Upload", model);
        }
    }
}