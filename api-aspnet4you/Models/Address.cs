using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.aspnet4you.mvc5.Models
{
    public class Address
    {
        public Address() { }
        public Address (int Id, string Name, string Phone, string City)
        {
            this.Id = Id;
            this.Name = Name;
            this.Phone = Phone;
            this.City = City;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
    }

}