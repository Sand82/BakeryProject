using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Areas.AdminConstants;

namespace Bakery.Areas.Admin.Controllers
{
    [Authorize(Roles = WebConstants.AdministratorRoleName)]
    [Area(AreaClass)]
    public abstract class AdminController : Controller
    {
    }
}
