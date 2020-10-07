using Socialley.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Socialley.Models
{
    public class Region
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        [Required]
        [MaxLength(AttributesConstraints.RegionNameMaxLength)]
        public string Name { get; set; }

        [Required, ForeignKey(nameof(Country))]
        public string CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}
