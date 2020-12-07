using Stexchange.Data.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Stexchange.Models
{
    public class DetailAdvertisementModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name_NL { get; set; }
        public string Name_LT { get; set; }
        public uint Quantity { get; set; }
        public int User_id { get; set; }
        public DateTime Created_at { get; set; }
        public bool Visible { get; set; }
        public bool Renewed { get; set; }
        public DateTime Last_modified { get; set; }
        public Dictionary<string, string> Filterlist { get; set; }
        public List<string> Imagelist { get; set; }
    }
}
