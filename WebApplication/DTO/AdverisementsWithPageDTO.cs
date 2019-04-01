using System.Collections.Generic;

namespace WebApplication.DTO
{
    public class AdverisementsWithPageToEndDTO
    {
        public IEnumerable<AdvertisementsDTO> Advertisement { get; set; }
        public int PagesToEnd { get; set; }
    }
}
