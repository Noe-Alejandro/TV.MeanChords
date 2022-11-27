using System;
using System.Collections.Generic;
using System.Linq;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Handlers.DiscHandler;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.ShoppingCarHandler
{
    public class ShoppingCarService : IShoppingCarService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        public static ShoppingCarService Create() => new ShoppingCarService();
        public ShoppingCarService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }
        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        public ResponseBase<PostShoppingCarResponse> PostDiscInWishList(int UserId, int DiscId)
        {
            if (UoWDiscosChowell.UserRepository.Get(x => x.UserId.Equals(UserId)).FirstOrDefault() == null)
                throw new Exception("El usuario con Id=" + UserId + " no existe");
            if(UoWDiscosChowell.DiscRepository.Get(x => x.DiscId.Equals(DiscId)).FirstOrDefault() == null)
                throw new Exception("El disco con Id=" + UserId + " no existe");
            if (UoWDiscosChowell.ShoppingCarRepository.Get(x => x.UserId.Equals(UserId) && x.DiscId.Equals(DiscId)).FirstOrDefault() != null)
                throw new Exception("El disco ya se encuentra en tu lista de deseos");
            UoWDiscosChowell.ShoppingCarRepository.Insert(new ShoppingCar
            {
                UserId = UserId,
                DiscId = DiscId
            });
            UoWDiscosChowell.Save();
            return ResponseBase<PostShoppingCarResponse>.Create(new PostShoppingCarResponse
            {
                Status = true
            });
        }

        public ResponseBase<DeleteShoppingCarResponse> RemoveDiscFromWishList(int UserId, int DiscId)
        {
            if (UoWDiscosChowell.UserRepository.Get(x => x.UserId.Equals(UserId)).FirstOrDefault() == null)
                throw new Exception("El usuario con Id=" + UserId + " no existe");
            if (UoWDiscosChowell.DiscRepository.Get(x => x.DiscId.Equals(DiscId)).FirstOrDefault() == null)
                throw new Exception("El disco con Id=" + UserId + " no existe");
            var shoppingCarItem = UoWDiscosChowell.ShoppingCarRepository.Get(x => x.UserId.Equals(UserId) && x.DiscId.Equals(DiscId)).FirstOrDefault();
            if (shoppingCarItem == null)
                throw new Exception("El disco no existe en tu lista de deseos");
            UoWDiscosChowell.ShoppingCarRepository.Delete(shoppingCarItem);
            UoWDiscosChowell.Save();
            return ResponseBase<DeleteShoppingCarResponse>.Create(new DeleteShoppingCarResponse
            {
                Status = true
            });
        }

        public ResponseBase<GetShoppingCarResponse> GetShoppingCar(int UserId)
        {
            var discLst = UoWDiscosChowell.ShoppingCarRepository.Get(x => x.UserId.Equals(UserId)).ToList();
            if (discLst == null)
                throw new Exception("No se encontraron discos con ese resultado");
            var responseLst = new GetShoppingCarResponse();
            responseLst.WishList = new List<GetDiscResponse>();
            foreach (var disc in discLst)
            {
                responseLst.WishList.Add(new GetDiscResponse
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
            return ResponseBase<GetShoppingCarResponse>.Create(responseLst);
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
    }
}
