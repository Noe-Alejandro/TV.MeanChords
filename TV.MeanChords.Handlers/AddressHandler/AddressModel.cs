using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV.MeanChords.Handlers.AddressHandler
{
    public class AddressRequest
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int ZIP { get; set; }
        public int UserId { get; set; }
    }

    public class AddressResponse
    {
        public int AddressId { get; set; }
    }

    public class AddressResponseView
    {
        public int AddressId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int ZIP { get; set; }
        public int UserId { get; set; }
    }
}
