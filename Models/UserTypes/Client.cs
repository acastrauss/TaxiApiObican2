using Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.UserTypes
{
    [DataContract]
    public class Client : UserProfile
    {

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public List<Models.Ride.Ride> Rides { get; set; }

        public Client() { }


        public Client(UserProfile user)
        {
            this.Address = user.Address;
            this.Password = user.Password;
            this.Email = user.Email;
            this.Username = user.Username;
            this.DateOfBirth = user.DateOfBirth;
            this.Fullname = user.Fullname;
            this.Type = UserType.DRIVER;
            this.ImagePath = user.ImagePath;
        }
    }
}
