﻿using Microsoft.ServiceFabric.Services.Remoting;
using Models.Auth;
using Models.UserTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Database
{
    [ServiceContract]
    public interface IAuthDataService : IService
    {
        [OperationContract]
        Task<bool> ExistsWithPwd(string email, string password);

        [OperationContract]
        Task<bool> ExistsSocialMediaAuth(string email);

        [OperationContract]
        Task<Models.Auth.UserProfile> GetUserProfile(Guid id);

        [OperationContract]
        Task<Models.Auth.UserProfile> UpdateUserProfile(UpdateUserProfileRequest request, Guid id);

        [OperationContract]
        Task<bool> CreateUser(UserProfile appModel);

    }
}
