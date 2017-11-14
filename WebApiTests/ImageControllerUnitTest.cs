using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Results;
using DataExchange;
using DataExchange.EntityModel;
using DataExchange.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedResources.Views;
using SSD.Web.Api.Controllers;
using SSD.Web.Api.Models;

namespace WebApiTests
{
    [TestClass]
    public class ImageControllerUnitTest
    {
        /// <summary>
        ///     Cleanups this instance. 
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            _db.Dispose();
            _scope.Dispose();
        }

        [TestMethod]
        public void DeleteFictionalImage()
        {
            // Arrange
            var controller = new ImageController(_db);

            // Act
            IHttpActionResult actionResult = controller.Delete(100000);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void DeleteImage()
        {
            // Arrange
            var controller = new ImageController(_db);

            // Act
            IHttpActionResult actionResult = controller.Delete(11);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }

        [TestMethod]
        public void GetFictionalImage()
        {
            // Arrange
            Image model = _db.Images.Read(e => e.ID == 100000);
            var controller = new ImageController(_db);

            // Act
            IHttpActionResult actionResult = controller.Get(100000);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetImage()
        {
            // Arrange
            Image model = _db.Images.Read(e => e.ID == 11);
            var controller = new ImageController(_db);

            // Act
            IHttpActionResult actionResult = controller.Get(11);
            var contentResult = actionResult as OkNegotiatedContentResult<ImageListModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(11, contentResult.Content.ID);
            Assert.AreEqual(contentResult.Content.ESecretKey, model.ESecretKey);
        }

        [TestMethod]
        public void GetUserImagesFromAlbum()
        {
            // Arrange
            Image model = _db.Images.Read(e => e.ID == 11);
            var album = model.Album;
            var controller = new ImageController(_db);

            // Act
            //get user images by album
            IHttpActionResult actionResult = controller.GetUserImages(model.AlbumId);
            var contentResult = actionResult as OkNegotiatedContentResult<ImageViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(album.Images.Count, contentResult.Content.Images.Count());
        }

        [TestMethod]
        public void GetUserImagesFromFictionalAlbum()
        {
            // Arrange
            Image model = _db.Images.Read(e => e.ID == 100000);
            var controller = new ImageController(_db);

            // Act
            //get user images by album
            IHttpActionResult actionResult = controller.GetUserImages(100000);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        /// <summary>
        ///     Initializes this instance. 
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _scope = new TransactionScope();
            _db = new DataManager(new AppDbContext());
        }

        [TestMethod]
        public void PostImage()
        {
            // Arrange
            int imageCount = _db.Images.Count();
            var image = new ImageCreateModel()
            {
                Title = "Test",
                Description = "Test description",
                AlbumId = 5,
                Path = "path",
                ThumbNail = "thumb",
                ESecretKey = "ESecretKey",
                Signature = "Signature",
                EncryptedIV = "EncryptedIV",
                CreationDate = DateTime.Now
            };
            var controller = new ImageController(_db);

            // Act
            IHttpActionResult actionResult = controller.Post(image);
            var createdResult = actionResult as ResponseMessageResult;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(HttpStatusCode.Created, createdResult.Response.StatusCode);
        }

        private DataManager _db;
        private TransactionScope _scope;
    }
}