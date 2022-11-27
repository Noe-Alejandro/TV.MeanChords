using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.TagHandler
{
    public class TagService : ITagService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        public static TagService Create() => new TagService();
        public TagService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }
        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        public ResponseBase<List<GetTagResponse>> GetCategories()
        {
            var tagLst = UoWDiscosChowell.TagRepository.GetAll().ToList();
            if (tagLst == null || tagLst.Count == 0)
                throw new Exception("No se encontraron etiquetas en la db");
            var responseLst = new List<GetTagResponse>();
            foreach (var tag in tagLst)
            {
                responseLst.Add(new GetTagResponse
                {
                    TagId = tag.TagId,
                    Name = tag.Name
                });
            }
            return ResponseBase<List<GetTagResponse>>.Create(responseLst);
        }
    }
}
