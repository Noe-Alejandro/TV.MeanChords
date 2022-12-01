using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.Handlers.AddressHandler;
using TV.MeanChords.Handlers.SaleHandler;
using TV.MeanChords.Handlers.ShoppingCarHandler;
using TV.MeanChords.Utils.GenericClass;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AddressController : ApiController
    {
        public IAddressService GetService()
        {
            return AddressService.Create();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Address/GET")]
        public IHttpActionResult GetAllAddresByUserId([FromUri] int UserId)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetAllAddress(UserId);

                    var mvReponse = new ResponseBase<List<AddressResponseView>>()
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<AddressResponseView>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Address/POST")]
        public IHttpActionResult PostAddress([FromBody] AddressRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PostAddress(request);

                    var mvReponse = new ResponseBase<AddressResponse>()
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<AddressResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("~/api/Address/DELETE")]
        public IHttpActionResult DeleteAddress([FromUri] int AddressId)
        {
            try
            {
                using (var service = GetService())
                {
                    service.DeleteAddress(AddressId);

                    return Ok();
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, ResponseBase<AddressResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}