using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.DiscHandler
{
    public class DiscService : IDiscService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        public static DiscService Create() => new DiscService();
        public DiscService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }

        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        private List<Category> MapCategories(Disc disc)
        {
            List<Category> categories = new List<Category>();
            foreach (var category in disc.DiscTag)
            {
                categories.Add(new Category { Name = category.Tag.Name });
            }
            return categories;
        }

        private bool ValidateParams(Disc request)
        {
            if (String.IsNullOrEmpty(request.Name))
                return false;
            if (String.IsNullOrEmpty(request.Description))
                return false;
            if (String.IsNullOrEmpty(request.DiscImgUrl))
                return false;
            if (request.Price < 0)
                throw new Exception("El precio no puede ser menor a 0");
            if (request.AuthorId == 0)
                throw new Exception("No se envío el identificador del autor o no se encuentra registrado en el sistema");
            return true;
        }

        public ResponseBase<GetDiscResponse> GetDisc(GetDiscRequest request)
        {
            var disc = UoWDiscosChowell.DiscRepository.Get(x => x.DiscId.Equals(request.DiscId)).FirstOrDefault();
            if (disc == null)
                throw new Exception("No se encontró el disco con el ID proporcionado");
            return ResponseBase<GetDiscResponse>.Create(new GetDiscResponse
            {
                DiscId = disc.DiscId,
                Name = disc.Name,
                Description = disc.Description,
                DiscImgUrl = disc.DiscImgUrl,
                Price = disc.Price,
                Amount = disc.Amount,
                Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName },
                Categories = MapCategories(disc)
            });
        }

        public ResponseBase<List<GetDiscResponse>> GetDiscByTitle(string Title)
        {
            var discLst = UoWDiscosChowell.DiscRepository.Get(x => x.Name.Contains(Title)).ToList();
            if (discLst == null)
                throw new Exception("No se encontraron discos con ese resultado");
            var responseLst = new List<GetDiscResponse>();
            foreach (var disc in discLst)
            {
                responseLst.Add(new GetDiscResponse
                {
                    DiscId = disc.DiscId,
                    Name = disc.Name,
                    Description = disc.Description,
                    DiscImgUrl = disc.DiscImgUrl,
                    Price = disc.Price,
                    Amount = disc.Amount,
                    Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName },
                    Categories = MapCategories(disc)
                });
            }
            return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
        }

        public ResponseBase<List<GetDiscResponse>> GetDiscByCategory(int CategoryId)
        {
            var discLst = UoWDiscosChowell.DiscTagRepository.Get(x => x.TagId.Equals(CategoryId)).ToList();
            if (discLst == null)
                throw new Exception("No se encontraron discos con ese resultado");
            var responseLst = new List<GetDiscResponse>();
            foreach (var disc in discLst)
            {
                responseLst.Add(new GetDiscResponse
                {
                    DiscId = disc.DiscId,
                    Name = disc.Disc.Name,
                    Description = disc.Disc.Description,
                    DiscImgUrl = disc.Disc.DiscImgUrl,
                    Price = disc.Disc.Price,
                    Amount = disc.Disc.Amount,
                    Author = new GetDiscResponseAuthor { FullName = disc.Disc.Author.FullName, ShortName = disc.Disc.Author.ShortName },
                    Categories = MapCategories(disc.Disc)
                });
            }
            return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
        }

        public ResponseBase<List<GetDiscResponse>> GetLastDisc(int? Quantity)
        {
            if(Quantity == null)
            {
                var disclst = UoWDiscosChowell.DiscRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToList();
                if (disclst == null || disclst.Count == 0)
                    throw new Exception("No hay discos en la db");
                var responseLst = new List<GetDiscResponse>();
                foreach (var disc in disclst)
                {
                    responseLst.Add(new GetDiscResponse
                    {
                        DiscId = disc.DiscId,
                        Name = disc.Name,
                        Description = disc.Description,
                        DiscImgUrl = disc.DiscImgUrl,
                        Price = disc.Price,
                        Amount = disc.Amount,
                        Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName },
                        Categories = MapCategories(disc)
                    });
                }
                return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
            }
            else
            {
                if (Quantity <= 0)
                    throw new Exception("Ingrese una cantidad válida");
                int take = (int)Quantity;
                var disclst = UoWDiscosChowell.DiscRepository.GetAll().OrderByDescending(x => x.CreatedDate).Take(take).ToList();
                if (disclst == null || disclst.Count == 0)
                    throw new Exception("No hay discos en la db");
                var responseLst = new List<GetDiscResponse>();
                foreach (var disc in disclst)
                {
                    responseLst.Add(new GetDiscResponse
                    {
                        DiscId = disc.DiscId,
                        Name = disc.Name,
                        Description = disc.Description,
                        DiscImgUrl = disc.DiscImgUrl,
                        Price = disc.Price,
                        Amount = disc.Amount,
                        Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName },
                        Categories = MapCategories(disc)
                    });
                }
                return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
            }
        }

        public ResponseBase<PostDiscResponse> PostDisc(PostDiscRequest request)
        {
            if (request == null)
                throw new Exception("Favor de mandar los datos del disco");
            Disc disc = new Disc
            {
                Name = request.Name,
                Description = request.Description,
                DiscImgUrl = request.DiscImgUrl,
                Price = request.Price,
                Amount = request.Amount,
                AuthorId = request.AuthorID,
                Status = true,
                CreatedDate = DateTime.Now,
                ModificationDate = DateTime.Now
            };
            ValidateParams(disc);
            UoWDiscosChowell.DiscRepository.Insert(disc);
            if (request.Categories != null) {
                foreach (var category in request.Categories)
                {
                    UoWDiscosChowell.DiscTagRepository.Insert(new DiscTag { DiscId = disc.DiscId, TagId = category.CategoryID });
                }
            }
            UoWDiscosChowell.Save();
            return ResponseBase<PostDiscResponse>.Create(new PostDiscResponse { DiscId = disc.DiscId });
        }

        public ResponseBase<List<GetDiscResponse>> GetAllDisc()
        {
            var disclst = UoWDiscosChowell.DiscRepository.GetAll().ToList();
            if (disclst == null || disclst.Count == 0)
                throw new Exception("No hay discos en la db");
            var responseLst = new List<GetDiscResponse>();
            foreach (var disc in disclst)
            {
                responseLst.Add(new GetDiscResponse
                {
                    DiscId = disc.DiscId,
                    Name = disc.Name,
                    Description = disc.Description,
                    DiscImgUrl = disc.DiscImgUrl,
                    Price = disc.Price,
                    Amount = disc.Amount,
                    Author = new GetDiscResponseAuthor { FullName = disc.Author.FullName, ShortName = disc.Author.ShortName },
                    Categories = MapCategories(disc)
                });
            }
            return ResponseBase<List<GetDiscResponse>>.Create(responseLst);
        }

        public ResponseBase<PostDiscResponse> PutDisc(int DiscId, PutDiscRequest request)
        {
            var disc = UoWDiscosChowell.DiscRepository.Get(x => x.DiscId.Equals(DiscId)).FirstOrDefault();
            if (disc != null)
                throw new Exception("No existe el id del disco proporcionado en la db");
            if (request.Amount == null)
            {
                if (ValidateString(request.Name))
                    disc.Name = request.Name;
                if (ValidateString(request.Description))
                    disc.Description = request.Description;
                if (ValidateString(request.DiscImgUrl))
                    disc.DiscImgUrl = request.DiscImgUrl;
                if (request.Price >= 0)
                    disc.Price = request.Price;
                if (request.Categories == null || request.Categories.Count == 0)
                    throw new Exception("Debe ingresarse al menos una categoría");
                foreach (var category in request.Categories)
                {
                    if (UoWDiscosChowell.TagRepository.Get(x => x.TagId.Equals(category.CategoryID)).FirstOrDefault() == null)
                        throw new Exception("La categoría con id " + category.CategoryID + " no existe en la db");
                    if (UoWDiscosChowell.DiscTagRepository.Get(x => x.DiscId.Equals(DiscId) && x.TagId.Equals(category.CategoryID)).FirstOrDefault() == null)
                        UoWDiscosChowell.DiscTagRepository.Insert(new DiscTag { DiscId = DiscId, TagId = category.CategoryID });
                }
                //var removeCategories = UoWDiscosChowell.DiscTagRepository.Get(x => x.DiscId.Equals(DiscId) && removeCategories.GroupBy)).ToList();
                if(request.AuthorID > 0)
                {
                    if (UoWDiscosChowell.AuthorRepository.Get(x => x.AuthorId.Equals(request.AuthorID)).FirstOrDefault() == null)
                        throw new Exception("El autor con id " + request.AuthorID + " no existe");
                    disc.AuthorId = request.AuthorID;
                }
                else
                {
                    if (request.Author != null)
                    {
                        if (!ValidateString(request.Author.FullName))
                            throw new Exception("Debe ingresar el nombre completo del autor");
                        if (!ValidateString(request.Author.ShortName))
                            throw new Exception("Debe ingresar el nombre corto del autor");
                        var newAuthor = new Author { FullName = request.Author.FullName, ShortName = request.Author.ShortName };
                        UoWDiscosChowell.AuthorRepository.Insert(newAuthor);
                        disc.AuthorId = newAuthor.AuthorId;
                    }
                    else
                    {
                        throw new Exception("Debe de introducirse un autor");
                    }
                }

            }
            else
            {
                if (request.Amount <= 0)
                    throw new Exception("Debe agregarse una cantidad válida");
                disc.Amount = (int)request.Amount;
            }
            UoWDiscosChowell.Save();
            return null;
        }

        private bool ValidateString(string value)
        {
            return value != null || value != "";
        }
    }
}
