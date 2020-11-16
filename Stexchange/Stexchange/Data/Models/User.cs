using System;
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
        [EmailAddress, StringLength(254)]
        public string Email { get; set; }
        [StringLength(15), Key]
        public string Username { get; set; }
        [StringLength(6)]
        public string Postal_Code { get; set; }
        [MaxLength(64)]
        public byte[] Password { get; set; }
        public DateTime Created_At { get; set; }
        public bool IsVerified { get; set; }
        public UserVerification Verification { get; set; }
	}
}
