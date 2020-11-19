using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Data.Models
{
    public class Listing
    {
        [NotMapped]
        private double distance = -1;

        [Column("id", TypeName = "serial"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("title", TypeName = "varchar(80)")]
        public string Title { get; set; }

        [Column("description", TypeName = "text")]
        public string Description { get; set; }

        [Column("name_nl", TypeName = "varchar(30)")]
        public string NameNl { get; set; }

        [Column("name_lt", TypeName = "varchar(30)")]
        public string NameLatin { get; set; }

        [Column("quantity", TypeName = "int"), Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [NotMapped]
        public List<ImageData> Pictures { get; set; }

        [Column("user_id", TypeName = "bigint(20) unsigned")]
        public int UserId { get; set; }

        public User Owner { get; set; }

        [NotMapped]
        public string OwningUserName { get; set; }

        [NotMapped]
        public List<string> Categories { get; set; }

        [Column("created_at", TypeName = "timestamp"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public double Distance { get => distance; set => distance = value; }

        [Column("visible", TypeName = "tinyint(1)")]
        public bool Visible { get; set; }

        [Column("renewed", TypeName = "tinyint(1)")]
        public bool Renewed { get; set; }

        [Column("last_modified", TypeName = "timestamp"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastModified { get; set; }
    }
}
