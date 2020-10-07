using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Socialley.Models
{
    public class FavouritePost
    {
        public string PostId { get; set; }
        public virtual Post Post { get; set; }


        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public bool IsFavourite { get; set; }
    }
}
