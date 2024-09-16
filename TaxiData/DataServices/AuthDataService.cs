using AzureInterface.Entities;
using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal class AuthDataService : BaseDataService<Models.Auth.UserProfile, UserEntity>, Contracts.Database.IAuthDataService
    {
        public AuthDataService(
            Synchronizer<UserEntity, UserProfile> synchronizer,
            IReliableStateManager stateManager
        )
            : base(synchronizer, stateManager)
        {}

        public async Task<UserProfile> UpdateUserProfile(UpdateUserProfileRequest request, Guid id)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var key = $"{id}";
            var existing = await dict.TryGetValueAsync(txWrapper.transaction, key);

            if (!existing.HasValue)
            {
                return null;
            }

            if (request.Password != null)
            {
                existing.Value.Password = request.Password;
            }

            if (request.Username != null)
            {
                existing.Value.Username = request.Username;
            }

            if (request.Address != null)
            {
                existing.Value.Address = request.Address;
            }

            if (request.ImagePath != null)
            {
                existing.Value.ImagePath = request.ImagePath;
            }

            if (request.Fullname != null)
            {
                existing.Value.Fullname = request.Fullname;
            }

            var updated = await dict.TryUpdateAsync(txWrapper.transaction, key, existing.Value, existing.Value);

            return updated ? existing.Value : null;
        }
    
        public async Task<UserProfile> GetUserProfile(Guid id)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var existing = await dict.TryGetValueAsync(txWrapper.transaction, $"{id}");
            return existing.Value;
        }

        public async Task<UserProfile> ExistsWithPwd(string email, string password)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var collectionEnum = await dict.CreateEnumerableAsync(txWrapper.transaction);
            var asyncEnum = collectionEnum.GetAsyncEnumerator();

            while (await asyncEnum.MoveNextAsync(default))
            {
                var user = asyncEnum.Current.Value;
                if(user != null)
                {
                    if (user.Email == email && user.Password == password)
                    {
                        return user;
                    } 
                }
            }

            return null;
        }

        public async Task<UserProfile> ExistsOnlyEmail(string email)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var collectionEnum = await dict.CreateEnumerableAsync(txWrapper.transaction);
            var asyncEnum = collectionEnum.GetAsyncEnumerator();

            while (await asyncEnum.MoveNextAsync(default))
            {
                var user = asyncEnum.Current.Value;
                if (user != null)
                {
                    if (user.Email == email)
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        public async Task<bool> Create(UserProfile appModel)
        {
            var dict = await GetReliableDictionary();
            using var txWrapper = new StateManagerTransactionWrapper(stateManager.CreateTransaction());
            var dictKey = $"{appModel.Id}";
            var created = await dict.AddOrUpdateAsync(txWrapper.transaction, dictKey, appModel, (key, value) => value);
            return created != null;
        }

        public async Task<bool> CreateUser(UserProfile appModel)
        {
            return await Create(appModel);
        }
    }
}
