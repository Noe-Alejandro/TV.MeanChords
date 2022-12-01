using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.SaleHandler
{
    public class SaleService : ISaleService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        private List<DeliveryServiceModel> deliveryServices;
        public static SaleService Create() => new SaleService();
        public SaleService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
            deliveryServices = new List<DeliveryServiceModel>()
            {
                new DeliveryServiceModel(){ DeliveryId = 1, Name = "DHL"},
                new DeliveryServiceModel(){ DeliveryId = 2, Name = "UPS"},
                new DeliveryServiceModel(){ DeliveryId = 3, Name = "FEDEX"},
                new DeliveryServiceModel(){ DeliveryId = 4, Name = "EMS"}
            };
        }
        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        public ResponseBase<SaleResponse> PostSale(float Total, string DeliveryService, int AddressId, int UserId)
        {
            var sale = new Sale
            {
                Date = DateTime.Now,
                Total = Total,
                UserId = UserId,
                AddressId = AddressId,
                Status = 1,
                DeliveryService = deliveryServices.Find(x => x.Name.Equals(DeliveryService)).DeliveryId
            };
            UoWDiscosChowell.SaleRepository.Insert(sale);
            UoWDiscosChowell.Save();
            var discInShoppingCar = UoWDiscosChowell.ShoppingCarRepository.Get(x => x.UserId.Equals(UserId)).ToList();
            var saleDetails = new List<SaleDisc>();
            var discLstToReduce = new List<Disc>();
            foreach (var disc in discInShoppingCar)
            {
                saleDetails.Add(new SaleDisc
                {
                    SaleId = sale.SaleId,
                    DiscId = disc.DiscId,
                    Amount = 1,
                    Total = 1 * disc.Disc.Price
                });
                discLstToReduce.Add(disc.Disc);
            }
            UoWDiscosChowell.SaleDiscRepository.InsertByRange(saleDetails);
            UoWDiscosChowell.Save();

            var discToNotify = new List<Disc>();
            foreach (var disc in discLstToReduce)
            {
                disc.Amount -= 1;
                if (disc.Amount <= 3)
                {
                    discToNotify.Add(disc);
                }
            }
            //Send to notity => discToNotify
            UoWDiscosChowell.ShoppingCarRepository.DeleteByRange(discInShoppingCar);
            UoWDiscosChowell.Save();
            return ResponseBase<SaleResponse>.Create(new SaleResponse
            {
                SaleId = sale.SaleId
            });
        }

        private class DeliveryServiceModel
        {
            public int DeliveryId { get; set; }
            public string Name { get; set; } 
        }
    }
}
