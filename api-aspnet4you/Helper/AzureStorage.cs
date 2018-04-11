using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using api.aspnet4you.mvc5.Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using System.Security.Claims;
using System.Configuration;

namespace api.aspnet4you.mvc5.Helper
{
    public class AzureStorage : IUserStore<AzureIdentityUser>, IUserClaimStore<AzureIdentityUser>, IUserPasswordStore<AzureIdentityUser>, IUserLockoutStore<AzureIdentityUser, string>, IUserTwoFactorStore<AzureIdentityUser, string>, IUserLoginStore<AzureIdentityUser>, IUserEmailStore<AzureIdentityUser>
    {
        private static string azureStorageUri = ConfigurationManager.AppSettings[GlobalConstants.AzureStorageUri];
        private static string azureStorageConnectionString = ConfigurationManager.AppSettings[GlobalConstants.AzureStorageConnectionString];
        private static string azureTableIdentityUser = ConfigurationManager.AppSettings[GlobalConstants.AzureTableIdentityUser];
        private static string azureTableLogin = ConfigurationManager.AppSettings[GlobalConstants.AzureTableLogin];

        private readonly CloudTable ctAzureIdentityUser;
        private readonly CloudTable ctAzureLogin;

        public AzureStorage()
        {
            StorageUri storageUri = new StorageUri(new Uri(azureStorageUri));
            CloudStorageAccount csa = CloudStorageAccount.Parse(azureStorageConnectionString);
            ctAzureIdentityUser = csa.CreateCloudTableClient().GetTableReference(azureTableIdentityUser);
            ctAzureIdentityUser.CreateIfNotExists();
            ctAzureLogin = csa.CreateCloudTableClient().GetTableReference(azureTableLogin);
            ctAzureLogin.CreateIfNotExists();
        }

        #region common interfaces
        public Task CreateAsync(AzureIdentityUser user)
        {
            return UpdateAsync(user);
        }

        public Task DeleteAsync(AzureIdentityUser user)
        {
            Task<TableResult> tsk = null;
            TableOperation retrieveOperation = TableOperation.Retrieve<AzureIdentityUser>(user.Email, user.RowKey);
            TableResult retrievedResult = ctAzureIdentityUser.Execute(retrieveOperation);
            AzureIdentityUser deleteEntity = (AzureIdentityUser)retrievedResult.Result;

            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                tsk = ctAzureIdentityUser.ExecuteAsync(deleteOperation);
            }

            return tsk;
        }

        public void Dispose()
        {
            
        }

        public Task<AzureIdentityUser> FindByIdAsync(string userId)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<AzureIdentityUser>(userId, userId);
            TableResult retrievedResult = ctAzureIdentityUser.Execute(retrieveOperation);
            AzureIdentityUser retrievedEntity = (AzureIdentityUser)retrievedResult.Result;

            return Task.FromResult<AzureIdentityUser>(retrievedEntity);
        }

        public Task<AzureIdentityUser> FindByNameAsync(string userName)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<AzureIdentityUser>(userName, userName);
            TableResult retrievedResult = ctAzureIdentityUser.Execute(retrieveOperation);
            AzureIdentityUser retrievedEntity = (AzureIdentityUser)retrievedResult.Result;

            return Task.FromResult<AzureIdentityUser>(retrievedEntity);
        }

        public Task UpdateAsync(AzureIdentityUser user)
        {
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(user);
            Task<TableResult> tableResult = ctAzureIdentityUser.ExecuteAsync(insertOrReplaceOperation);

            return tableResult;
        }

        #endregion common interfaces

        #region IUserClaimStore
        public Task<IList<Claim>> GetClaimsAsync(AzureIdentityUser user)
        {
            return Task.FromResult<IList<Claim>>(user.Claims);
        }

        public Task AddClaimAsync(AzureIdentityUser user, Claim claim)
        {
            if (!user.Claims.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            {
                user.Claims.Add(claim);
            }

            return Task.FromResult(0);
        }

        public Task RemoveClaimAsync(AzureIdentityUser user, Claim claim)
        {
            user.Claims.Remove(claim);
            return Task.FromResult(0);
        }
        #endregion IUserClaimStore

        #region IUserPasswordStore
        public Task<string> GetPasswordHashAsync(AzureIdentityUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AzureIdentityUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(AzureIdentityUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        #endregion IUserPasswordStore

        #region IUserLockoutStore
        public Task<int> GetAccessFailedCountAsync(AzureIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(AzureIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AzureIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(AzureIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(AzureIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(AzureIdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(AzureIdentityUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        #endregion IUserLockoutStore

        #region IUserTwoFactorStore
        public Task<bool> GetTwoFactorEnabledAsync(AzureIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(AzureIdentityUser user, bool enabled)
        {
            throw new NotImplementedException();
        }
        #endregion IUserTwoFactorStore


        #region IUserLoginStore
        public Task AddLoginAsync(AzureIdentityUser user, UserLoginInfo login)
        {
            TableOperation op = TableOperation.Insert(new AzureLogin(user.Id, login));
            var result = ctAzureLogin.Execute(op);
            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(AzureIdentityUser user, UserLoginInfo login)
        {
            var al = Find(login);
            if (al != null)
            {
                TableOperation op = TableOperation.Delete(al);
                var result = ctAzureLogin.Execute(op);
            }
            return Task.FromResult(0);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AzureIdentityUser user)
        {
            TableQuery<AzureLogin> query = new TableQuery<AzureLogin>()
            .Where(TableQuery.GenerateFilterCondition("UserId", QueryComparisons.Equal, user.Id))
            .Select(new string[] { "LoginProvider", "ProviderKey" });
            var results = ctAzureLogin.ExecuteQuery(query);
            IList<UserLoginInfo> logins = new List<UserLoginInfo>();
            foreach (var al in results)
            {
                logins.Add(new UserLoginInfo(al.LoginProvider, al.ProviderKey));
            }
            return Task.FromResult(logins);
        }

        public Task<AzureIdentityUser> FindAsync(UserLoginInfo login)
        {
            var al = Find(login);
            if (al != null)
            {
                return FindByIdAsync(al.UserId);
            }
            return Task.FromResult<AzureIdentityUser>(null);
        }

        private AzureLogin Find(UserLoginInfo login)
        {
            TableQuery<AzureLogin> query = new TableQuery<AzureLogin>()
                .Where(TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("LoginProvider", QueryComparisons.Equal, login.LoginProvider),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("ProviderKey", QueryComparisons.Equal, login.ProviderKey)))
                .Select(new string[] { "UserId" });
            return ctAzureLogin.ExecuteQuery(query).FirstOrDefault();
        }


        #endregion IUserLoginStore

        public Task SetEmailAsync(AzureIdentityUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(AzureIdentityUser user)
        {
            return Task.FromResult<string>(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(AzureIdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(AzureIdentityUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<AzureIdentityUser> FindByEmailAsync(string email)
        {
            return FindByNameAsync(email);
        }
    }
}