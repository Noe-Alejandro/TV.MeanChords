using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.Handlers.ShoppingCarHandler;
using TV.MeanChords.Utils.GenericClass;
namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ShoppingCarController : ApiController
    {
        public IShoppingCarService GetService()
        {
            return ShoppingCarService.Create();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/ShoppingCar/GET")]
        public IHttpActionResult GetShoppingCar([FromUri]int UserId)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.GetShoppingCar(UserId);

                    var mvReponse = new ResponseBase<GetShoppingCarResponse>()
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<GetShoppingCarResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/ShoppingCar/POST")]
        public IHttpActionResult PostDiscInWishList(ShoppingCarRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.PostDiscInWishList(request.UserId, request.DiscId);

                    var mvReponse = new ResponseBase<PostShoppingCarResponse>()
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<PostShoppingCarResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("~/api/ShoppingCar/DELETE")]
        public IHttpActionResult RemoveDiscFromWishList(ShoppingCarRequest request)
        {
            try
            {
                using (var service = GetService())
                {
                    var response = service.RemoveDiscFromWishList(request.UserId, request.DiscId);

                    var mvReponse = new ResponseBase<DeleteShoppingCarResponse>()
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
                return Content(HttpStatusCode.InternalServerError, ResponseBase<DeleteShoppingCarResponse>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}