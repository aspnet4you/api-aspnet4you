using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.aspnet4you.mvc5.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
    }
}