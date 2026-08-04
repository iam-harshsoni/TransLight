using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransLight.Areas.Store.Controllers
{
    [Authorize]
    [Area("Store")]
    public class BaseController : Controller
    {
    }
}
