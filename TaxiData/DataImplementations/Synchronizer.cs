using Contracts.SQLDB;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiData.DataImplementations
{
    internal class Synchronizer<T1, T2> where T1 : class, BaseEntity 
    {
        private ISQLCRUD<T1> sqlStorage;
        private string ReliableDictName;
        private IDTOConverter<T1, T2> converter;
        private Microsoft.ServiceFabric.Data.IReliableStateManager StateManager;

        public Synchronizer(ISQLCRUD<T1> sqlStorage, string realiableDictName, IDTOConverter<T1, T2> converter, Microsoft.ServiceFabric.Data.IReliableStateManager stateManager)
        {
            this.sqlStorage = sqlStorage;
            this.ReliableDictName = realiableDictName;
            this.converter = converter;
            this.StateManager = stateManager;
        }

        public async Task SyncAzureTablesWithDict()
        {
            var dict = await StateManager.GetOrAddAsync<IReliableDictionary<string, T2>>(ReliableDictName);
            using var tx = StateManager.CreateTransaction();

            var collectionEnumerable = await dict.CreateEnumerableAsync(tx);
            var asyncEnum = collectionEnumerable.GetAsyncEnumerator();

            var entitiesToSync = new List<T1>();

            while (await asyncEnum.MoveNextAsync(default))
            {
                var entity = asyncEnum.Current.Value;
                if(entity != null)
                {
                    entitiesToSync.Add(converter.AppModelToSQL(entity));
                }
            }

            await tx.CommitAsync();

            if (entitiesToSync.Count > 0)
            {
                await sqlStorage.AddOrUpdateMultipleEntities(entitiesToSync);
            }
        }

        public async Task SyncDictWithAzureTable()
        {
            var azureTableEntities = sqlStorage.GetAllEntities();
            if(azureTableEntities == null)
            {
                return;
            }

            var dict = await StateManager.GetOrAddAsync<IReliableDictionary<string, T2>>(ReliableDictName);
            using var tx = StateManager.CreateTransaction();

            foreach (var entity in azureTableEntities) 
            {
                var appModel = converter.SQLToAppModel(entity);
                var dictKey = $"{entity.Id}";
                var created = await dict.AddOrUpdateAsync(tx, dictKey, appModel, (key, value) => value);
            }

            await tx.CommitAsync();
        }
    }
}
