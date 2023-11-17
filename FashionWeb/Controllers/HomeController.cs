using FashionWeb.Domain.BusinessRules;
using FashionWeb.Domain.Entities;
using FashionWeb.Domain.InfraStructure;
using FashionWeb.Domain.InfraStructure.Request;
using FashionWeb.Domain.Utils;
using FashionWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FashionWeb.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IPersonBusinessRules _personBusinessRules;
        private readonly ICoreBusinessRules _coreBusinessRules;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogWritter _logWritter;

        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IPersonBusinessRules personBusinessRules,
            ICoreBusinessRules coreBusinessRules,
            IWebHostEnvironment hostingEnvironment,
            ILogWritter logWritter,
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _personBusinessRules = personBusinessRules;
            _coreBusinessRules = coreBusinessRules;
            _hostingEnvironment = hostingEnvironment;
            _logWritter = logWritter;
        }

        [NoCache]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Register()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View();
        }

        [HttpGet]
        public IActionResult Login(bool isConfirmed)
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View(isConfirmed);
        }

        public IActionResult DontAccessPage()
        {
            return View();
        }

        public IActionResult RecoverPassword()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendPassword([FromBody] UserViewModel model)
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            //Se a model tiver preenchida, vamos verificar se ele possui um usuário!
            if (model != null && !string.IsNullOrEmpty(model.username))
            {
                var user = await _userManager.FindByNameAsync(model.username);
                if (user != null)
                {
                    try
                    {
                        var userInfo = this._personBusinessRules.GetUser(new Guid(user.Id));

                        var webRoot = _hostingEnvironment.WebRootPath;
                        var filePath = Path.Combine(webRoot, "html");

                        string templatePath = $@"{filePath}/padrao.html";
                        var fileContent = System.IO.File.ReadAllText(templatePath);
                        var send = SendEmail.Send(fileContent, $@"A sua senha é: {userInfo.Password}", "Você esqueceu sua senha!", model.username);

                    }
                    catch(Exception ex)
                    {
                        _logWritter.Writer($"excessão gerada: {ex.Message}");
                        return Json(false);
                    }

                    
                    return Json(true);
                }
                else
                    return Json(false);
            }


            return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserViewModel model)
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            ValidationModel validator = null;
            List<string> erros = new List<string>();

            DefaultReturnData defaultReturnData = new DefaultReturnData();

            if (string.IsNullOrEmpty(model.username))
                erros.Add("Necessário preencher nome de usuário.");

            if (string.IsNullOrEmpty(model.password))
                erros.Add("Necessário preencher a senha.");

            validator = new ValidationModel(erros);
            if (validator.errorList.Count() > 0)
                return Json(validator);

            var user = await _userManager.FindByNameAsync(model.username);

            try
            {
                if (user != null)
                {
                    //Vamos verificar se o e-mail está confirmado.
                    var isConfirmed = this._coreBusinessRules.VerifyEmailConfirmed(user.Id);
                    if (isConfirmed == true)
                    {
                        var SignInResult = await _signInManager.PasswordSignInAsync(user, model.password, false, false);

                        if (SignInResult.Succeeded)
                        {
                            defaultReturnData.success = true;

                            // Decodificar a URL fornecida
                            if (!string.IsNullOrEmpty(model.returnUrl))
                            {
                                model.returnUrl = System.Web.HttpUtility.UrlDecode(model.returnUrl);

                                Uri uri = new Uri(model.returnUrl);
                                string returnUrl = uri.Query; // Retorna "?ReturnUrl=/User/Profile"

                                // Remova o caractere "?" do início da string, se necessário
                                if (returnUrl.StartsWith("?"))
                                {
                                    returnUrl = returnUrl.Substring(1);

                                    // Se você só precisa do valor do parâmetro "ReturnUrl"
                                    string returnUrlValue = System.Web.HttpUtility.ParseQueryString(returnUrl)["ReturnUrl"];
                                    model.returnUrl = returnUrlValue;
                                }
                            }

                            defaultReturnData.data = model.returnUrl;
                            return Json(defaultReturnData);
                        }
                        else
                        {
                            erros.Add("Não foi possível fazer login. Senha incorreta.");
                        }
                    }
                    else
                        erros.Add("Necessário confirmar seu endereço de e-mail!");
                }
                else
                    erros.Add("Esse Usuário não está cadastrado.");
            }
            catch (Exception)
            {
                erros.Add("Erro ao processar requisição!");
            }

            validator = new ValidationModel(erros);
            return Json(validator);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserViewModel model)
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index");

            List<string> erros = new List<string>();

            if (!ModelState.IsValid)
            {

                IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                if (allErrors != null && allErrors.Count() > 0)
                {
                    foreach (var error in allErrors)
                        erros.Add(error.ErrorMessage);
                }
            }
            else
            {
                //Agora segue validando se é um e-mail valido.
                if (!model.username.IsValidEmail())
                {
                    erros.Add("O endereço de e-mail digitado é inválido!");
                }
            }

            if (erros != null && erros.Count() > 0)
            {
                var validatorModel = new ValidationModel(erros);
                return Json(validatorModel);
            }

            //Daqui pra baixo a validação fica na mão do Identity

            var userinfo = new IdentityUser
            {

                UserName = model.username,
                Email = string.Empty,
                EmailConfirmed = true
            };

            var userresult = await _userManager.CreateAsync(userinfo, model.password);

            if (model.typeUser == (int)Domain.Entities.UserInfo.Roles.NEGOCIANTE)
            {
                await _userManager.AddToRoleAsync(userinfo, Domain.Entities.UserInfo.Roles.NEGOCIANTE.ToString());
            }
            else
                await _userManager.AddToRoleAsync(userinfo, Domain.Entities.UserInfo.Roles.USUARIO.ToString());

            if (userresult.Succeeded)
            {
                //Deve cadastrar um User vinculado ao AspNetUser
                this._personBusinessRules.InsertUser(new Domain.Entities.UserInfo()
                {
                    AspNetUserId = new Guid(userinfo.Id),
                    typeUser = 0,
                    UniqueId = Guid.NewGuid(),
                    Password = model.password,
                    Profile = new Domain.Entities.Person
                    {
                        Name = model.name
                    }
                });

                var request = HttpContext.Request;
                var domain = $"{request.Scheme}://{request.Host}";

                var webRoot = _hostingEnvironment.WebRootPath;
                var filePath = Path.Combine(webRoot, "html");
                string templatePath = $@"{filePath}/padrao.html";
                var fileContent = System.IO.File.ReadAllText(templatePath);

                var send = SendEmail.Send(fileContent, $@"<p>Seu usuário: {userinfo.UserName}</p><p>Senha inicial: {model.password}</p>", "Bem-vindo ao Shop Digital", model.username);

                return Json(true);
            }

            if (userresult.Errors != null && userresult.Errors.Count() > 0)
            {
                foreach (var error in userresult.Errors)
                    erros.Add(error.Description);

            }

            var validatorIdentity = new ValidationModel(erros);
            return Json(validatorIdentity);
        }

        [HttpGet]
        public IActionResult ConfirmAccount(Guid user)
        {
            //Atualizando o confirmEmail do usuário
            this._coreBusinessRules.ConfirmarEmail(user);
            return RedirectToAction("Login", new { isConfirmed = true });
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            return Json(this._coreBusinessRules.GetAllCategories());
        }

        [HttpPost]
        public IActionResult GetPersonsBusiness([FromBody] SearchPersonBusiness filter)
        {
            var resultado = this._coreBusinessRules.GetPersonsBusiness(filter);
            if(resultado.Results != null && resultado.Results.Count() > 0)
            {
                var webRootPath = _hostingEnvironment.WebRootPath;

                //Se a imagem de algum dos negócios nao existir, carrego imagem default.
                foreach (var item in resultado.Results)
                {
                    string archivePath = webRootPath + "/" + item.Person.PhotoUrl;

                    if (string.IsNullOrEmpty(item.Person.PhotoUrl) ||
                        !System.IO.File.Exists(archivePath))
                    {
                        item.Person.PhotoUrl = "/gravatar.png";
                    }
                }
            }

            return Json(resultado);
        }

        [HttpGet]
        public IActionResult Product(int Id)
        {
            ViewBag.Id = Id;

            return View();
        }

        [HttpPost]
        public IActionResult GetProfile([FromBody] int personId)
        {
            var result = this._coreBusinessRules.GetPersonBusinessToVisit(personId);
            var webRootPath = _hostingEnvironment.WebRootPath;
            string archivePath = webRootPath + "/" + result.Person.PhotoUrl;

            if (string.IsNullOrEmpty(result.Person.PhotoUrl) ||
                !System.IO.File.Exists(archivePath))
            {
                result.Person.PhotoUrl = "/gravatar.png";
            }

            string bannerPath = webRootPath + "/" + result.Person.BannerUrl;

            if (string.IsNullOrEmpty(result.Person.BannerUrl) ||
                !System.IO.File.Exists(bannerPath))
            {
                result.Person.HasBanner = false;
            }
            else
            {
                result.Person.HasBanner = true;
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetRateProfileByPerson([FromBody] UserClassificationBusiness userClassificationBusiness)
        {

            if (User.Identity.IsAuthenticated)
            {
                var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
                var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));

                userClassificationBusiness.PersonId = user.PersonId;
            }

            return Json(this._coreBusinessRules.GetRateProfileByPerson(userClassificationBusiness));
        }

        [HttpPost]
        public async Task<IActionResult> SaveBusinessClassification([FromBody] UserClassificationBusiness userClassificationBusiness)
        {
            //Usuário não está logado, retorna false para salvar classificação
            if (!User.Identity.IsAuthenticated)
                return Json(false);

            //Aqui eu preciso puxar o usuário que está executando a ação.
            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));

            if(user != null && user.PersonId > 0)
            {
                userClassificationBusiness.PersonId = user.PersonId;

                return Json(this._coreBusinessRules.SaveUserClassificationBusiness(userClassificationBusiness));
            }


            return Json(false);
        }

        [HttpPost]
        public IActionResult GetPersonBusinessProducts([FromBody] SearchPersonBusinessProducts searchPersonBusinessProducts)
        {
            searchPersonBusinessProducts.AllProducts = true;
            return Json(this._coreBusinessRules.GetPersonBusinessWithProducts(searchPersonBusinessProducts));
        }

        [HttpGet]
        public IActionResult CheckinProfile(int personId)
        {
            ViewBag.PersonId = personId;

            return View();
        }

        [HttpPost]
        public IActionResult SavePersonBusinessCheckin([FromBody] CheckinPersonBusinessRaffle checkinPersonBusiness)
        {
            return Json(this._coreBusinessRules.SaveCheckinPersonBusinesRaffles(checkinPersonBusiness));
        }

        [HttpPost]
        public IActionResult GetProfileRaffles([FromBody] SearchPersonBusinessRaffles searchPersonBusinessRaffles)
        {

            searchPersonBusinessRaffles.AllRaffles = true;

            var result = this._coreBusinessRules.GetPersonBusinessToVisit(searchPersonBusinessRaffles.PersonId);
            searchPersonBusinessRaffles.PersonBusinessId = result.Id;

            result.Raffles = this._coreBusinessRules.GetPersonBusinessRafflesPaged(searchPersonBusinessRaffles);

            var webRootPath = _hostingEnvironment.WebRootPath;
            string archivePath = webRootPath + "/" + result.Person.PhotoUrl;

            if (string.IsNullOrEmpty(result.Person.PhotoUrl) ||
                !System.IO.File.Exists(archivePath))
            {
                result.Person.PhotoUrl = "/gravatar.png";
            }

            string bannerPath = webRootPath + "/" + result.Person.BannerUrl;

            if (string.IsNullOrEmpty(result.Person.BannerUrl) ||
                !System.IO.File.Exists(bannerPath))
            {
                result.Person.HasBanner = false;
            }
            else
            {
                result.Person.HasBanner = true;
            }

            return Json(result);
        }

        [HttpPost]
        public IActionResult GetPersonBusinessArchives([FromBody] SearchPersonBusinessArchive searchPersonBusinessArchive)
        {
            return Json(this._coreBusinessRules.GetPersonBusinessArchives(searchPersonBusinessArchive));
        }

        [HttpPost]
        public IActionResult GetProducts([FromBody] SearchPersonBusinessProducts filter)
        {
            var resultado = this._coreBusinessRules.GetProductsByCategoryId(filter);
            return Json(resultado);
        }

        [HttpPost]
        public IActionResult GetProduct([FromBody] int Id)
        {
            var result = this._coreBusinessRules.GetProduct(Id);
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProductCart(Int32 Id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login");

            //Aqui eu preciso puxar o usuário que está executando a ação.
            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));

            if (user != null && user.PersonId > 0)
            {
                //Se o carrinho não existir, cria o carrinho.
                Cart cart = this._coreBusinessRules.GetCart(user.PersonId);

                if (cart == null || cart.Id == 0)
                {
                    this._coreBusinessRules.InsertCart(new Cart()
                    {
                        PersonId = user.PersonId
                    });

                    cart = this._coreBusinessRules.GetCart(user.PersonId);
                }

                //Insere o produto no carrinho.
                this._coreBusinessRules.InsertCartProduct(new CartProduct()
                {
                    ProductId = Id,
                    CartId = cart.Id
                });
            }

            return RedirectToAction("Cart", "User");
        }

    }
}
