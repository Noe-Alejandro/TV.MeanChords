using System.Collections.Generic;
using TV.MeanChords.Handlers.DiscHandler;

namespace TV.MeanChords.Handlers.ShoppingCarHandler
{
    public class GetShoppingCarResponse
    {
        public List<GetDiscResponse> WishList { get; set; }
    }

    public class PostShoppingCarResponse
    {
        public bool Status { get; set; }
    }

    public class DeleteShoppingCarResponse
    {
        public bool Status { get; set; }
    }

    public class ShoppingCarRequest
    {
        public int UserId { get; set; }
        public int DiscId { get; set; }
    }
}
