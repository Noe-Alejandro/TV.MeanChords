using System.Web.Http.Cors;
using System.Web.Mvc;

namespace TV.MeanChords.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}