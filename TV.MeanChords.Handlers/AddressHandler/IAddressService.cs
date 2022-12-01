using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.AddressHandler
{
    public interface IAddressService : IDisposable
    {
        ResponseBase<List<AddressResponseView>> GetAllAddress(int userId);
        ResponseBase<AddressResponse> PostAddress(AddressRequest request);
        void DeleteAddress(int addressId);
        ResponseBase<AddressResponse> PutAddress(int addresId, AddressRequest request);
    }
}
