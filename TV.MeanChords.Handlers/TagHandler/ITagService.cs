using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.TagHandler
{
    public interface ITagService : IDisposable
    {
        ResponseBase<List<GetTagResponse>> GetCategories();
    }
}
