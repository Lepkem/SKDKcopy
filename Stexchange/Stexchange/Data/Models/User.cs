using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        [EmailAddress, Column(TypeName = "VARCHAR(254)"), StringLength(254)]
        public string Email { get; set; }
        public UserVerification Verification { get; set; }
	}
}
