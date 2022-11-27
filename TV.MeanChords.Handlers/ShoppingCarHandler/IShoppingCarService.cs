using System;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.ShoppingCarHandler
{
    public interface IShoppingCarService : IDisposable
    {
        ResponseBase<GetShoppingCarResponse> GetShoppingCar(int UserId);
        ResponseBase<PostShoppingCarResponse> PostDiscInWishList(int UserId, int DiscId);
        ResponseBase<DeleteShoppingCarResponse> RemoveDiscFromWishList(int UserId, int DiscId);
    }
}
