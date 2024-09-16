using DatabaseAccess.Entities;
using Microsoft.ServiceFabric.Data;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiData.DataImplementations;

namespace TaxiData.DataServices
{
    internal class ClientDataService : BaseDataService<Models.UserTypes.Client, ClientEntity>
    {
        public ClientDataService(Synchronizer<ClientEntity, Client> synchronizer, IReliableStateManager stateManager) : base(synchronizer, stateManager)
        {}
    }
}
