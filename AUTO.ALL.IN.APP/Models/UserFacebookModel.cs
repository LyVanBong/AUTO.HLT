using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AUTO.ALL.IN.APP.Models
{
    [Table("UserFacebook")]
    public class UserFacebookModel
    {
        [Key]
        public string Id { get; set; }
        [Column(TypeName = "text")]
        public string Cookie { get; set; }
        [Column(TypeName = "text")]
        public string Token { get; set; }
    }
}