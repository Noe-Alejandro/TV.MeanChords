using System.Collections.Generic;
using System.Linq;
using TV.MeanChords.Data.Db.Context.DiscosChowell;
using TV.MeanChords.Data.Db.UnitOfWork;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.Handlers.AddressHandler
{
    public class AddressService : IAddressService
    {
        private UoWDiscosChowell UoWDiscosChowell { get; set; }
        public static AddressService Create() => new AddressService();
        public AddressService()
        {
            UoWDiscosChowell = UoWDiscosChowell.Create();
        }
        public void Dispose()
        {
            UoWDiscosChowell.Dispose();
            UoWDiscosChowell = null;
        }

        public void DeleteAddress(int addressId)
        {
            var address = UoWDiscosChowell.AddressRepository.Get(x => x.AddressId.Equals(addressId)).FirstOrDefault();
            address.Status = false;
            UoWDiscosChowell.Save();
        }

        public ResponseBase<List<AddressResponseView>> GetAllAddress(int userId)
        {
            var responseDB = UoWDiscosChowell.AddressRepository.Get(x => x.UserId.Equals(userId) && (bool)x.Status).ToList();
            List<AddressResponseView> response = new List<AddressResponseView>();
            foreach (var address in responseDB)
            {
                response.Add(new AddressResponseView
                {
                    AddressId = address.AddressId,
                    Country = address.Country,
                    City = address.City,
                    Street = address.Street,
                    HouseNumber = address.HouseNumber,
                    ZIP = address.Zip,
                    UserId = address.UserId
                });
            }
            return ResponseBase<List<AddressResponseView>>.Create(response);
        }

        public ResponseBase<AddressResponse> PostAddress(AddressRequest request)
        {
            var address = new Address
            {
                Country = request.Country,
                City = request.City,
                Street = request.Street,
                HouseNumber = request.HouseNumber,
                Zip = request.ZIP,
                UserId = request.UserId,
                Status = true
            };
            UoWDiscosChowell.AddressRepository.Insert(address);
            UoWDiscosChowell.Save();
            return ResponseBase<AddressResponse>.Create(new AddressResponse { AddressId = address.AddressId});
        }

        public ResponseBase<AddressResponse> PutAddress(int addresId, AddressRequest request)
        {
            var address = UoWDiscosChowell.AddressRepository.Get(x => x.AddressId.Equals(addresId)).FirstOrDefault();
            address.Country = request.Country;
            address.City = request.City;
            address.HouseNumber = request.HouseNumber;
            address.Street = request.Street;
            address.Zip = request.ZIP;
            UoWDiscosChowell.Save();
            return ResponseBase<AddressResponse>.Create(new AddressResponse { AddressId = address.AddressId});
        }
    }
}
