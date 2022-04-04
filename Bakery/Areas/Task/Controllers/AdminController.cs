using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Areas.AdminConstants;

namespace Bakery.Areas.Task.Controllers
{
    [Authorize(Roles = WebConstants.AdministratorRoleName)]
    [Area(AreaNameTask)]
    public abstract class AdminController : Controller
    {
    }
}
