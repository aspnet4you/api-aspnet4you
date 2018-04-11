using Microsoft.AspNet.Identity;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace api.aspnet4you.mvc5.Models
{
    public class AzureIdentityUser : TableEntity, IUser
    {
        public AzureIdentityUser()
        {
            Id = Guid.NewGuid().ToString();
            SetPartitionAndRowKey();
        }

        public AzureIdentityUser(string userName)
        {
            UserName = userName;
            SetPartitionAndRowKey();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int FailedLogIns { get; set; }
        //public IList<string> Roles { get; set; }
        public IList<Claim> Claims { get; set; }

        private void SetPartitionAndRowKey()
        {
            PartitionKey = Id;
            RowKey = Id;
        }

        public ClaimsIdentity GenerateUserIdentityAsync(UserManager<AzureIdentityUser> manager, AzureIdentityUser user)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);

            Claim[] claims =user.Claims.ToArray();

            foreach(Claim clm in claims)
            {
                claimsIdentity.AddClaim(clm);
            }

            return claimsIdentity;
        }
    }

    public class AzureLogin : TableEntity
    {
        public AzureLogin()
        {
            PartitionKey = "ASPNET4YOU-Identity";
            RowKey = Guid.NewGuid().ToString();
        }

        public AzureLogin(string ownerId, UserLoginInfo info) : this()
        {
            UserId = ownerId;
            LoginProvider = info.LoginProvider;
            ProviderKey = info.ProviderKey;
        }

        public string UserId { get; set; }
        public string ProviderKey { get; set; }
        public string LoginProvider { get; set; }
    }
}