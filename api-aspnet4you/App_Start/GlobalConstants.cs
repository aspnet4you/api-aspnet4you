using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.aspnet4you.mvc5
{
    public class GlobalConstants
    {
        public static readonly string LinkedInClientId = "LinkedInClientId";
        public static readonly string LinkedInClientSecrete = "LinkedInClientSecrete";
        public static readonly string LinkedInCallbackPath = "LinkedInCallbackPath";
        public static readonly string CookieLoginPath = "CookieLoginPath";
        public static readonly string JwtTopSecrete512 = "JwtTopSecrete512";

        public static readonly string AllowedAudiences = "AllowedAudiences";
        public static readonly string AllowedIssuers = "AllowedIssuers";

        public static readonly string IdDocDb_Database = "IdDocDb_Database";
        public static readonly string IdDocDb_Uri = "IdDocDb_Uri";
        public static readonly string IdDocDb_CollectionName = "IdDocDb_CollectionName";
        public static readonly string IdDocDb_AuthKey = "IdDocDb_AuthKey";

        public static readonly string AzureStorageUri = "AzureStorageUri";
        public static readonly string AzureStorageConnectionString = "AzureStorageConnectionString";
        public static readonly string AzureTableIdentityUser = "AzureTableIdentityUser";
        public static readonly string AzureTableLogin = "AzureTableLogin";
    }
}