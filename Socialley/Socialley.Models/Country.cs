using Socialley.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Socialley.Models
{
    public class Country
    {
        [Key]
        public string Id { get; set; } = new Guid().ToString();

        [Required]
        [MaxLength(AttributesConstraints.CountryNameMaxLength)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();

        public virtual ICollection<Region> Regions { get; set; } = new HashSet<Region>();
    }
}
