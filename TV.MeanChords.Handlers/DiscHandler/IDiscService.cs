using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.DiscHandler
{
    public interface IDiscService : IDisposable
    {
        ResponseBase<GetDiscResponse> GetDisc(GetDiscRequest request);
        ResponseBase<List<GetDiscResponse>> GetDiscByTitle(string Title);
        ResponseBase<List<GetDiscResponse>> GetDiscByCategory(int CategoryId);
        ResponseBase<List<GetDiscResponse>> GetAllDisc();
        ResponseBase<List<GetDiscResponse>> GetLastDisc(int? Quantity);
        ResponseBase<PostDiscResponse> PostDisc(PostDiscRequest request);
        ResponseBase<PostDiscResponse> PutDisc(int DiscId, PutDiscRequest request);
    }
}
