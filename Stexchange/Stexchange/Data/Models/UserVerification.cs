using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stexchange.Data.Models
{
    public class UserVerification
	{
        [Column("user_id", TypeName = "bigint(20) unsigned")]
        public int Id { get; set; }
        [Column("verification_code", TypeName = "varbinary(16)")]
        public Guid Guid { get; set; }
        [NotMapped]
        public User User { get; set; }
	}
}
