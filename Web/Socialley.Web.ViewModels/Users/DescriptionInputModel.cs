namespace Socialley.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    public class DescriptionInputModel
    {
        [MaxLength(250)]
        public string Description { get; set; }
    }
}
