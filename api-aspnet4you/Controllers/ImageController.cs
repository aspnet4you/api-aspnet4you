using api.aspnet4you.mvc5.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api.aspnet4you.mvc5.Controllers
{
    public class ImageController : ApiController
    {
        // GET api/<controller>/5

        public string Get(int id)
        {
            return $"You have enterd {id}.";
        }



        /// <summary>
        ///  Returns  ImageModel model
        /// </summary>
        /// <param name="id">csharpbanner or app-architecture</param>
        /// <returns></returns>
        [HttpGet]
        public ImageModel GetImage(string id)
        {
            ImageModel imgModel = new ImageModel();
            imgModel.Id = 1;
            imgModel.Name = id;
            imgModel.ContentType = "img/jpg";
            imgModel.Data = imageToByteArray(id);
            return imgModel;
        }

        private byte[] imageToByteArray(string imageName)
        {
            string imgext = ConfigurationManager.AppSettings.Get("imgext");
            string filePath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}images\\{imageName}.{imgext}";
            System.Drawing.Image image = System.Drawing.Bitmap.FromFile(filePath);
            using (var ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}