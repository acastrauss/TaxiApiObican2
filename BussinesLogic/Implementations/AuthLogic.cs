using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLogic.Implementations
{
    internal class AuthLogic : Contracts.Logic.IAuthLogic
    {
        private Contracts.Database.IData dbService;

        public AuthLogic(Contracts.Database.IData dbService) 
        {
            this.dbService = dbService;
        }

        public async Task<UserProfile> GetUserProfile(Guid id)
        {
            return await dbService.GetUserProfile(id);
        }

        public async Task<Tuple<bool, UserType>> Login(LoginData loginData)
        {
            bool exists = false;
            foreach (UserType type in Enum.GetValues(typeof(UserType)))
            {
                if (loginData.authType == AuthType.TRADITIONAL)
                {
                    exists |= await dbService.ExistsWithPwd(loginData.Email, loginData.Password);
                }
                // Google Auth
                else
                {
                    exists |= await dbService.ExistsOnlyEmail(loginData.Email);
                }

                if (exists)
                {
                    return Tuple.Create(exists, type);
                }
            }

            return Tuple.Create<bool, UserType>(exists, default);
        }

        public async Task<bool> Register(UserProfile userProfile)
        {
            var userExists = false;
            foreach (UserType type in Enum.GetValues(typeof(UserType)))
            {
                userExists |= await dbService.ExistsOnlyEmail(userProfile.Email);
            }

            if (userExists)
            {
                return false;
            }

            if (userProfile.Type == UserType.DRIVER)
            {
                var newDriver = new Models.UserTypes.Driver(userProfile, Models.UserTypes.DriverStatus.NOT_VERIFIED);
                return await dbService.CreateDriver(newDriver);
            }

            return await dbService.CreateUser(userProfile);
        }

        public async Task<UserProfile> UpdateUserProfile(UpdateUserProfileRequest updateUserProfileRequest, Guid id)
        {
            return await dbService.UpdateUserProfile(updateUserProfileRequest, id);
        }
    }
}
