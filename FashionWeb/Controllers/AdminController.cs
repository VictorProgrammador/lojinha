using FashionWeb.Domain.BusinessRules;
using FashionWeb.Domain.Entities;
using FashionWeb.Domain.InfraStructure.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FashionWeb.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR")]
    public class AdminController : Controller
    {
        private readonly IPersonBusinessRules _personBusinessRules;
        private readonly ICoreBusinessRules _coreBusinessRules;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AdminController(
          IPersonBusinessRules personBusinessRules,
          ICoreBusinessRules coreBusinessRules,
          UserManager<IdentityUser> userManager,
          IWebHostEnvironment hostingEnvironment)
        {
            _personBusinessRules = personBusinessRules;
            _userManager = userManager;
            _coreBusinessRules = coreBusinessRules;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ManagementUsers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetPersonsBusinessManagement([FromBody] SearchPersonBusiness filter)
        {
            filter.sortColumn = "Approved";
            filter.sortAscending = true;
            return Json(this._coreBusinessRules.GetPersonsBusinessManagement(filter));
        }

        [HttpPost]
        public async Task<IActionResult> SavePersonBusinessManagement([FromBody] PersonBusiness personBusiness)
        {

            UserInfo userInfo = this._coreBusinessRules.GetUserByPersonId(personBusiness.PersonId);
            var user = await _userManager.FindByIdAsync(userInfo.AspNetUserId.ToString());

            bool isInRole = await _userManager.IsInRoleAsync(user, "PREMIUM");

            //Se for assinante, devemos cadastrar a regra ao Usuário. Caso contrário a gente tira caso ele tenha essa regra.
            if (personBusiness.IsSubscriber)
            {
                if (!isInRole)
                {
                    await _userManager.AddToRoleAsync(user, "PREMIUM");
                }
            }
            else
            {
                if (isInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user, "PREMIUM");
                }
            }

            //var webRoot = _hostingEnvironment.WebRootPath;
            //var filePath = System.IO.Path.Combine(webRoot, "html");

            //string templatePath = $@"{filePath}/padrao.html";
            //var fileContent = System.IO.File.ReadAllText(templatePath);

            return Json(this._coreBusinessRules.SavePersonBusinessManagement(personBusiness, ""));
        }
    }
}
