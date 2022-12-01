using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.AuthorHandler
{
    public class AuthorService : IAuthorService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        public static AuthorService Create() => new AuthorService();
        public AuthorService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }

        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        public ResponseBase<List<AuthorResponse>> GetAllAuthor()
        {
            var authors = UoWDiscosChowell.AuthorRepository.GetAll().ToList();
            var response = new List<AuthorResponse>();
            foreach (var author in authors)
            {
                response.Add(new AuthorResponse
                {
                    AuthorId = author.AuthorId,
                    FullName = author.FullName,
                    ShortName = author.ShortName
                });
            }
            return ResponseBase<List<AuthorResponse>>.Create(response);
        }
    }
}
