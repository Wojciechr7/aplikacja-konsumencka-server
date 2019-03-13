using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Commends
{
    public class AdvertisementCOM
    {
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Price { get; set; }
        public decimal Size { get; set; }

        public string Title { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Floor { get; set; }
        public string Category { get; set; }


        public List<ImageCOM> Images = new List<ImageCOM>();
    }
}
