using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.AuthorHandler
{
    public interface IAuthorService : IDisposable
    {
        ResponseBase<List<AuthorResponse>> GetAllAuthor();
    }
}
