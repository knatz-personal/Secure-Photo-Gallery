using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;

namespace WebPortal.Extensions
{
    public static class HttpPostedFileBaseExtensions
    {
        public static bool IsExpectedFormat(this HttpPostedFileBase file, ImageFormats imageFormats)
        {
            if (!file.ContentType.Contains("image"))
            {
                return false;
            }

            foreach (ImageFormat format in imageFormats)
            {
                if (file.FileName.EndsWith(format.Extension, StringComparison.OrdinalIgnoreCase))
                {
                    MemoryStream memoryStream = file.InputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        file.InputStream.CopyTo(memoryStream);
                    }

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    byte[] signatureBlock = new byte[16];
                    memoryStream.Read(signatureBlock, 0, signatureBlock.Length);

                    string hexSignature =
                        (BitConverter.ToString(signatureBlock).Replace("-", " ")).ToUpper();

                    if (hexSignature.Contains(format.Signature.ToUpper()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}