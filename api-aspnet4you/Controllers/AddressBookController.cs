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
    [RoutePrefix("api/addressbook")]
    public class AddressBookController : ApiController
    {
        private static Address[] Addresses = {
            new Address(){Id=1, Name="Prodip", Phone="214-555-1212", City="Lewisville" },
            new Address(){Id=2, Name="Sreeni", Phone="214-556-1212", City="Plan-O" },
            new Address(){Id=3, Name="Jian", Phone="214-557-1212", City="Irving" },
            new Address(){Id=4, Name="Ravi", Phone="214-558-1212", City="Frisco" },
            new Address(){Id=5, Name="John", Phone="214-559-1212", City="Dallas" },
            new Address(){Id=6, Name="Robert", Phone="214-560-1212", City="Dallas" },
            new Address(){Id=7, Name="Chuck", Phone="214-561-1212", City="Carrollton" },
            new Address(){Id=8, Name="Trae", Phone="214-562-1212", City="Plan-O" },
            new Address(){Id=9, Name="Sergey", Phone="214-563-1212", City="Denton" },
            new Address(){Id=10, Name="Jane", Phone="214-564-1212", City="Las Colinas" }
        };

        // GET api/<controller>/1
        [Route("Get/Id")]
        public Address Get(int Id)
        {
            Address tempAddress = null;
            Address[] addressList= Addresses.Where(a => a.Id == Id).ToArray();

            if(addressList !=null && addressList.Count() > 0)
            {
                tempAddress = addressList[0];
            }

            return tempAddress;
        }

        /// <summary>
        /// Return address for the matching Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>Address if Id match or null</returns>
        [HttpGet]
        [Route("GetAddressById/Id")]
        public Address GetAddressById(int Id)
        {
            return Get(Id);
        }

        /// <summary>
        /// Returns all Addresses
        /// </summary>
        /// <returns>Address[]</returns>
        [HttpGet]
        [Route("GetAllAddresses")]
        public Address[] GetAllAddresses()
        {
            return Addresses;
        }

        /// <summary>
        /// Returns all Addresses  protected by oAuth Authentication
        /// </summary>
        /// <returns>Address[]</returns>
        [Authorize]
        [HttpGet]
        [Route("GetProtectedAddresses")]
        public Address[] GetProtectedAddresses()
        {
            return Addresses;
        }
    }
}