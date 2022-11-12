using TV.MeanChords.API.Helpers;
using TV.MeanChords.Handlers.CalculosCuotasHandler;
using TV.MeanChords.ModelViews;
using TV.MeanChords.Utils.GenericClass;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using TV.MeanChords.API.JWT;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ExampleController : ApiController
    {
        public IExample GetInteractor()
        {
            return ExampleInteractor.Create();
        }
        [Authorize]
        [HttpGet]
        [Route("~/api/ExampleValue")]
        public IHttpActionResult GetExampleValues()
        {
            try
            {
                using (var interactor = GetInteractor())
                {
                    var response = interactor.GetExampleValues();

                    var mvReponse = new ResponseBase<List<MVGetExampleValueResponse>>()
                    {
                        Data = MapperHelper.Map<List<MVGetExampleValueResponse>>(response.Data),
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

                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<MVGetExampleValueResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/RecipeExampleValue")]
        public IHttpActionResult GetRecipeValue()
        {
            try
            {
                using (var interactor = GetInteractor())
                {
                    var response = interactor.GetExampleRecipe();
                    var token = TokenGenerator.GenerateTokenJwt("noe@gmail.com");

                    var mvReponse = new ResponseBase<List<MVGetRecipeValueResponse>>()
                    {
                        Data = MapperHelper.Map<List<MVGetRecipeValueResponse>>(response.Data),
                        Errors = response.Errors,
                        Status = response.Status
                    };
                    var data = JsonConvert.SerializeObject(mvReponse);
                    if (mvReponse.Status)
                        return Content(HttpStatusCode.OK, mvReponse);

                    return Content(HttpStatusCode.BadRequest, mvReponse);
                }
            }
            catch (Exception e)
            {

                return Content(HttpStatusCode.InternalServerError, ResponseBase<List<MVGetRecipeValueResponse>>.Create(new List<string>()
                {
                    e.Message
                }));
            }
        }
    }
}