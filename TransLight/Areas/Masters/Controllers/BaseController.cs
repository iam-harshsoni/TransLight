using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TransLight.Areas.Masters.Controllers
{
    [Authorize]
    [Area("Masters")]
    public class BaseController : Controller
    {

    }
}
