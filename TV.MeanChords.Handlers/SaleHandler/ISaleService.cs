using System;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.SaleHandler
{
    public interface ISaleService : IDisposable
    {
        ResponseBase<SaleResponse> PostSale(float Total, string DeliveryService, int AddressId, int UserId);
    }
}
