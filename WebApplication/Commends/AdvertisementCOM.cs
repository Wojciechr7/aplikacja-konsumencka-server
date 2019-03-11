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
        public string Adress { get; set; }
        public decimal Size { get; set; }
        public string Categories { get; set; }
        public List<ImageCOM> Images = new List<ImageCOM>();
    }
}
