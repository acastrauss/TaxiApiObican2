﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.UserTypes
{
    [DataContract]
    public class RideRating
    {
        [DataMember]
        public Guid RideId { get; set; }

        [DataMember]
        [System.ComponentModel.DataAnnotations.Range(1, 5)]
        public int Value { get; set; }
    }
}
