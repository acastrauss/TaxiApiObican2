﻿using Azure.Data.Tables;
using AzureInterface;
using AzureInterface.DTO;
using AzureInterface.Entities;
using Contracts.SQLDB;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Models;
using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal abstract class BaseDataService<T1, T2> where T1 : class where T2 : class, BaseEntity
    {
        protected readonly Synchronizer<T2, T1> synchronizer;
        protected readonly IReliableStateManager stateManager;

        public BaseDataService(
            Synchronizer<T2, T1> synchronizer,
            IReliableStateManager stateManager
        )
        {
            this.synchronizer = synchronizer;
            this.stateManager = stateManager;
        }

        public async Task SyncAzureTablesWithDict()
        {
            await synchronizer.SyncAzureTablesWithDict();
        }

        public async Task SyncDictWithAzureTable()
        {
            await synchronizer.SyncDictWithAzureTable();
        }
        protected async Task<IReliableDictionary<string, T1>> GetReliableDictionary()
        {
            return await stateManager.GetOrAddAsync<IReliableDictionary<string, T1>>(typeof(T1).Name);
        }
    }
}
