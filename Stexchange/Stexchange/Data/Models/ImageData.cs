using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Data.Models
{
    public class ImageData
    {
        public ImageData(Listing list, byte[] image)
        {
            Listing = list;
        }

        [Column("id", TypeName = "serial"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("listing_id", TypeName = "bigint(20) unsigned")]
        public int ListingId { get; set; }

        [Column("image", TypeName = "LONGBLOB"), Required]
        public byte[] Image { get; set; }

        public Listing Listing { get; set; }
    }
}
