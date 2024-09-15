using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Entities
{
    [Table("Rating")]
    public class RatingEntity
    {
        [Key]
        public Guid Id { get; set; }
    
        public int Value { get; set; }

        public Guid RideId { get; set; }
        public virtual RideEntity Ride { get; set; }

    }
}
