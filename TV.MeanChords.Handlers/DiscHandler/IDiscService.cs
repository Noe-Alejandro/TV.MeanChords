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
        ResponseBase<PostDiscResponse> PostDisc(PostDiscRequest request);
    }
}
