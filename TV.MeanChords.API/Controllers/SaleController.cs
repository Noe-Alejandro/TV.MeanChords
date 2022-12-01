using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.Handlers.SaleHandler;
using TV.MeanChords.Handlers.ShoppingCarHandler;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SaleController : ApiController
    {
        public ISaleService GetService()
        {
            return SaleService.Create();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Sale/POST")]
        public IHttpActionResult PostSale([FromBody] SaleRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PostSale(request.Total, request.DeliveryService, request.AddressId, request.UserId);

                    var mvReponse = new ResponseBase<SaleResponse>()
                    {
                        Data = response.Data,
                        Errors = response.Errors,
                        Status = response.Status
                    };

                    if (mvReponse.Status)
                        return Content(HttpStatusCode.OK, mvReponse);

                    return Content(HttpStatusCode.BadRequest, mvReponse);
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ResponseBase<SaleResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        public class SaleRequest
        {
            public float Total { get; set; }
            public string DeliveryService { get; set; }
            public int AddressId { get; set; }
            public int UserId { get; set; }
        }
    }
}