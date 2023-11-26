using FashionWeb.Domain.BusinessRules;
using FashionWeb.Domain.Entities;
using FashionWeb.Domain.InfraStructure;
using FashionWeb.Domain.InfraStructure.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;
using System.Threading;
using RestSharp;
using System.Net.Http;
using System.Text;
using FashionWeb.Domain.Entities.Order;

namespace FashionWeb.Controllers
{
    [Authorize(Roles = "USUARIO, ADMINISTRADOR")]
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPersonBusinessRules _personBusinessRules;
        private readonly ICoreBusinessRules _coreBusinessRules;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogWritter _logWritter;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IWebHostEnvironment webHostEnvironment,
            IPersonBusinessRules personBusinessRules,
            ICoreBusinessRules coreBusinessRules,
            ILogWritter logWritter,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _personBusinessRules = personBusinessRules;
            _userManager = userManager;
            _logWritter = logWritter;
            _coreBusinessRules = coreBusinessRules;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Profile()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Products()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult SuccessOrder()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult CanceledOrder()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult Raffle()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Gallery()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpGet]
        public IActionResult Order(int Id)
        {
            ViewBag.Id = Id;

            return View();
        }

        public IActionResult Orders()
        {
            return View();
        }

        #region Profile

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUser(new Guid(aspUser.Id));
            var webRootPath = _webHostEnvironment.WebRootPath;
            string archivePath = webRootPath + "/" + user.Profile.PhotoUrl;

            if (string.IsNullOrEmpty(user.Profile.PhotoUrl) ||
                !System.IO.File.Exists(archivePath))
            {
                user.Profile.HasPhoto = false;
            }
            else
            {
                user.Profile.HasPhoto = true;
            }

            string bannerPath = webRootPath + "/" + user.Profile.BannerUrl;

            if (string.IsNullOrEmpty(user.Profile.BannerUrl) ||
                !System.IO.File.Exists(bannerPath))
            {
                user.Profile.HasBanner = false;
            }
            else
            {
                user.Profile.HasBanner = true;
            }

            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePersonBannerPhoto(Microsoft.AspNetCore.Http.IFormFile file)
        {
            string userValue = Request.Form["user"];
            if (string.IsNullOrEmpty(userValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            UserInfo user = null;
            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                user = JsonConvert.DeserializeObject<UserInfo>(userValue);
                string folderUserPath = Path.Combine(webRootPath, "uploads", User.Identity.Name);

                if (!Directory.Exists(folderUserPath))
                {
                    //Se o diretório de arquivos do usuário não existir, vamos criar uma pra ele!
                    Directory.CreateDirectory(folderUserPath);
                }

                if (file != null && file.Length > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string archivePath = Path.Combine(folderUserPath, "banner" + extension);

                    string pathLastArchive = webRootPath + "\\" + user.Profile.BannerUrl;

                    if (!string.IsNullOrEmpty(user.Profile.BannerUrl))
                    {
                        //Se já existe uma foto de perfil, vamos apagar para não deixar o servidor pesado.
                        if (System.IO.File.Exists(pathLastArchive))
                        {
                            System.IO.File.Delete(pathLastArchive);
                        }
                    }
                    // salvando a imagem em um arquivo
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        stream.Position = 0;

                        using (var image = Image.Load(stream))
                        {
                            // define a largura máxima desejada para a imagem
                            int larguraMaxima = 800;


                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(larguraMaxima, 0),
                                Mode = ResizeMode.Max
                            }));

                            // Salvar imagem redimensionada em disco
                            image.Save(archivePath);
                        }
                    }

                    user.Profile.BannerUrl = Path.Combine(@"uploads", User.Identity.Name, "banner" + extension);
                }

            }
            catch (Exception ex)
            {
                _logWritter.Writer($"excessão gerada: {ex.Message}");
                validationModel.success = false;
                validationModel.errorList.Add("Tivemos problema com a tua imagem. Entre em contato com o suporte ou suba um arquivo diferente!");
                return Json(validationModel);
            }

            //Aqui atualiza apenas o banner.
            if (this._personBusinessRules.UpdatePersonBannerPhoto(user.PersonId, user.Profile.BannerUrl))
            {
                validationModel.success = true;
            }
            else
            {
                validationModel.success = false;
                validationModel.errorList.Add("Infelizmente não conseguimos atualizar seu banner. Entre em contato com o suporte!");
            }

            return Json(validationModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePersonPhoto(string file)
        {
            string userValue = Request.Form["user"];
            if (string.IsNullOrEmpty(userValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            UserInfo user = null;
            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                user = JsonConvert.DeserializeObject<UserInfo>(userValue);
                string folderUserPath = Path.Combine(webRootPath, "uploads", User.Identity.Name);

                if (!string.IsNullOrEmpty(file))
                {
                    string base64String = file.Substring(file.IndexOf(',') + 1);

                    if (!Directory.Exists(folderUserPath))
                    {
                        //Se o diretório de arquivos do usuário não existir, vamos criar uma pra ele!
                        Directory.CreateDirectory(folderUserPath);
                    }

                    // supondo que a variável base64String contenha o conteúdo em formato Base64
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    // verificando a extensão a partir dos primeiros bytes
                    string extension = ".png";
                    if (imageBytes[0] == 255 && imageBytes[1] == 216 && imageBytes[2] == 255)
                    {
                        extension = ".jpg";
                    }

                    string newArchive = "perfil" + extension;
                    string imgUserToProfile = Path.Combine(folderUserPath, newArchive);

                    string pathLastArchive = webRootPath + user.Profile.PhotoUrl;

                    if (!string.IsNullOrEmpty(user.Profile.PhotoUrl))
                    {
                        //Se já existe uma foto de perfil, vamos apagar para não deixar o servidor pesado.
                        if (System.IO.File.Exists(pathLastArchive))
                        {
                            System.IO.File.Delete(pathLastArchive);
                        }
                    }

                    // salvando a imagem em um arquivo
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var image = Image.Load(ms))
                        {
                            // define as dimensões máximas da imagem
                            int alturaMaxima = 150;
                            int larguraMaxima = 300;


                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(larguraMaxima, alturaMaxima),
                                Mode = ResizeMode.Max
                            }));

                            // Salvar imagem redimensionada em disco
                            image.Save(imgUserToProfile);
                        }
                    }

                    user.Profile.PhotoUrl = Path.Combine(@"uploads", User.Identity.Name, newArchive);
                }

            }
            catch (Exception ex)
            {
                _logWritter.Writer($"excessão gerada: {ex.Message}");
                validationModel.success = false;
                validationModel.errorList.Add("Tivemos problema com a tua imagem. Entre em contato com o suporte ou suba um arquivo diferente!");
                return Json(validationModel);
            }

            //Aqui atualiza apenas a foto de perfil.
            if (this._personBusinessRules.UpdatePersonPhoto(user.PersonId, user.Profile.PhotoUrl))
            {
                validationModel.success = true;
            }
            else
            {
                validationModel.success = false;
                validationModel.errorList.Add("Infelizmente não conseguimos atualizar seu perfil. Entre em contato com o suporte!");
            }

            return Json(validationModel);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMyProfile([FromBody] UserInfo user)
        {
            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            if (this._personBusinessRules.UpdateUser(user))
            {
                validationModel.success = true;
            }
            else
            {
                validationModel.success = false;
                validationModel.errorList.Add("Infelizmente não conseguimos atualizar seu perfil. Entre em contato com o suporte!");
            }

            return Json(validationModel);
        }

        #endregion

        #region MyBusiness

        public IActionResult SaveMyBusiness([FromBody] PersonBusiness personBusiness)
        {
            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            if (!ModelState.IsValid)
            {
                IEnumerable<Microsoft.AspNetCore.Mvc.ModelBinding.ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                if (allErrors != null && allErrors.Count() > 0)
                {
                    foreach (var error in allErrors)
                        validationModel.errorList.Add(error.ErrorMessage);
                }

                validationModel.success = false;
                return Json(validationModel);
            }

            if (this._coreBusinessRules.SaveMyBusiness(personBusiness))
            {
                validationModel.success = true;
            }
            else
            {
                validationModel.success = false;
                validationModel.errorList.Add("Ocorreu um erro ao salvar seu negócio. Entre em contato com o suporte!");
            }

            return Json(validationModel);
        }

        [HttpPost]
        public async Task<IActionResult> GetMyProducts([FromBody] SearchPersonBusinessProducts searchPersonBusinessProducts)
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));
            searchPersonBusinessProducts.PersonId = user.PersonId;


            return Json(this._coreBusinessRules.GetPersonBusinessWithProducts(searchPersonBusinessProducts));
        }

        [HttpPost]
        public async Task<IActionResult> GetMyComplements()
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));

            return Json(this._coreBusinessRules.GetPersonBusinessComplements(user.PersonId));
        }

        [HttpPost]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> SaveMyProductFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            string productValue = Request.Form["product"];
            if (string.IsNullOrEmpty(productValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            Domain.Entities.Product product = null;
            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                product = JsonConvert.DeserializeObject<Domain.Entities.Product>(productValue);
                string folderUserPath = Path.Combine(webRootPath, "uploads", User.Identity.Name);

                if (!Directory.Exists(folderUserPath))
                {
                    //Se o diretório de arquivos do usuário não existir, vamos criar uma pra ele!
                    Directory.CreateDirectory(folderUserPath);
                }

                if (file.Length > 0)
                {
                    string fileExtension = Path.GetExtension(file.FileName);
                    string newArchive = Guid.NewGuid().ToString() + fileExtension;
                    string imgProduct = Path.Combine(folderUserPath, newArchive);

                    string pathLastArchive = webRootPath + "\\" + product.Image;

                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        //Se já existe uma foto de perfil, vamos apagar para não deixar o servidor pesado.
                        if (System.IO.File.Exists(pathLastArchive))
                        {
                            System.IO.File.Delete(pathLastArchive);
                        }
                    }

                    using (var stream = new FileStream(imgProduct, FileMode.Create))
                    {
                        // Copia o conteúdo do arquivo para o fluxo
                        file.CopyTo(stream);
                    }

                    product.Image = Path.Combine(@"uploads", User.Identity.Name, newArchive);
                }

            }
            catch (Exception ex)
            {
                _logWritter.Writer($"excessão gerada: {ex.Message}");
                validationModel.success = false;
                validationModel.errorList.Add("Tivemos problema com a tua imagem. Entre em contato com o suporte ou suba um arquivo diferente!");
                return Json(validationModel);
            }

            return Json(this._coreBusinessRules.SaveProduct(product));
        }

        [HttpPost]
        public IActionResult SaveMyProduct(string file)
        {
            string productValue = Request.Form["product"];
            if (string.IsNullOrEmpty(productValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            Domain.Entities.Product product = null;
            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                product = JsonConvert.DeserializeObject<Domain.Entities.Product>(productValue);
                string folderUserPath = Path.Combine(webRootPath, "uploads", User.Identity.Name);

                if (!Directory.Exists(folderUserPath))
                {
                    //Se o diretório de arquivos do usuário não existir, vamos criar uma pra ele!
                    Directory.CreateDirectory(folderUserPath);
                }

                if (!string.IsNullOrEmpty(file))
                {
                    string base64String = file.Substring(file.IndexOf(',') + 1);
                    // supondo que a variável base64String contenha o conteúdo em formato Base64
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    // verificando a extensão a partir dos primeiros bytes
                    string extension = ".png";
                    if (imageBytes[0] == 255 && imageBytes[1] == 216 && imageBytes[2] == 255)
                    {
                        extension = ".jpg";
                    }

                    string newArchive = Guid.NewGuid().ToString() + extension;
                    string imgProduct = Path.Combine(folderUserPath, newArchive);

                    string pathLastArchive = webRootPath + "\\" + product.Image;

                    if (!string.IsNullOrEmpty(product.Image))
                    {
                        //Se já existe uma foto de perfil, vamos apagar para não deixar o servidor pesado.
                        if (System.IO.File.Exists(pathLastArchive))
                        {
                            System.IO.File.Delete(pathLastArchive);
                        }
                    }

                    // salvando a imagem em um arquivo
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var image = Image.Load(ms))
                        {
                            // define as dimensões máximas da imagem
                            int alturaMaxima = 200;
                            int larguraMaxima = 200;


                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(larguraMaxima, alturaMaxima),
                                Mode = ResizeMode.Max
                            }));

                            // Salvar imagem redimensionada em disco
                            image.Save(imgProduct);
                        }
                    }

                    product.Image = Path.Combine(@"uploads", User.Identity.Name, newArchive);
                }

            }
            catch (Exception ex)
            {
                _logWritter.Writer($"excessão gerada: {ex.Message}");
                validationModel.success = false;
                validationModel.errorList.Add("Tivemos problema com a tua imagem. Entre em contato com o suporte ou suba um arquivo diferente!");
                return Json(validationModel);
            }

            return Json(this._coreBusinessRules.SaveProduct(product));
        }

        [HttpPost]
        public IActionResult ExcluirProduto([FromBody] Domain.Entities.Product product)
        {
            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(product.Image))
            {
                string pathLastArchive = webRootPath + "\\" + product.Image;

                //Se já existe uma foto de perfil, vamos apagar para não deixar o servidor pesado.
                if (System.IO.File.Exists(pathLastArchive))
                {
                    System.IO.File.Delete(pathLastArchive);
                }
            }

            return Json(this._coreBusinessRules.ExcluirProduto(product));
        }

        [HttpPost]
        public async Task<IActionResult> GetMyRaffles([FromBody] SearchPersonBusinessRaffles searchPersonBusinessRaffles)
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));
            searchPersonBusinessRaffles.PersonId = user.PersonId;


            return Json(this._coreBusinessRules.GetPersonBusinessWithRaffles(searchPersonBusinessRaffles));
        }

        [HttpPost]
        public IActionResult GetRaffleParticipants([FromBody] Raffle raffle)
        {
            return Json(this._coreBusinessRules.GetCheckinsPersonBusinessRaffle(raffle.UniqueId));
        }

        [HttpPost]
        public IActionResult SaveMyRaffle(string file)
        {
            string rafleValue = Request.Form["raffle"];
            if (string.IsNullOrEmpty(rafleValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            Raffle raffle = null;
            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                raffle = JsonConvert.DeserializeObject<Raffle>(rafleValue);
                string folderUserPath = Path.Combine(webRootPath, "uploads", User.Identity.Name);

                if (!Directory.Exists(folderUserPath))
                {
                    //Se o diretório de arquivos do usuário não existir, vamos criar uma pra ele!
                    Directory.CreateDirectory(folderUserPath);
                }

                if (!string.IsNullOrEmpty(file))
                {
                    string base64String = file.Substring(file.IndexOf(',') + 1);
                    // supondo que a variável base64String contenha o conteúdo em formato Base64
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    // verificando a extensão a partir dos primeiros bytes
                    string extension = ".png";
                    if (imageBytes[0] == 255 && imageBytes[1] == 216 && imageBytes[2] == 255)
                    {
                        extension = ".jpg";
                    }

                    string newArchive = Guid.NewGuid().ToString() + extension;
                    string imgProduct = Path.Combine(folderUserPath, newArchive);

                    string pathLastArchive = webRootPath + "\\" + raffle.Image;

                    if (!string.IsNullOrEmpty(raffle.Image))
                    {
                        //Se já existe uma foto de perfil, vamos apagar para não deixar o servidor pesado.
                        if (System.IO.File.Exists(pathLastArchive))
                        {
                            System.IO.File.Delete(pathLastArchive);
                        }
                    }

                    // salvando a imagem em um arquivo
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var image = Image.Load(ms))
                        {
                            // define as dimensões máximas da imagem
                            int alturaMaxima = 200;
                            int larguraMaxima = 200;


                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(larguraMaxima, alturaMaxima),
                                Mode = ResizeMode.Max
                            }));

                            // Salvar imagem redimensionada em disco
                            image.Save(imgProduct);
                        }
                    }

                    raffle.Image = Path.Combine(@"uploads", User.Identity.Name, newArchive);
                }

            }
            catch (Exception ex)
            {
                _logWritter.Writer($"excessão gerada: {ex.Message}");
                validationModel.success = false;
                validationModel.errorList.Add("Tivemos problema com a tua imagem. Entre em contato com o suporte ou suba um arquivo diferente!");
                return Json(validationModel);
            }

            if (raffle.EndDate == null)
            {
                validationModel.success = false;
                validationModel.errorList.Add("Precisa por uma data de encerramento pro sorteio.");
                return Json(validationModel);
            }

            return Json(this._coreBusinessRules.SaveRaffle(raffle));
        }

        [HttpPost]
        public IActionResult ExcluirSorteio([FromBody] Raffle raffle)
        {
            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            if (!string.IsNullOrEmpty(raffle.Image))
            {
                string pathLastArchive = webRootPath + "\\" + raffle.Image;

                //Se já existe uma foto de perfil, vamos apagar para não deixar o servidor pesado.
                if (System.IO.File.Exists(pathLastArchive))
                {
                    System.IO.File.Delete(pathLastArchive);
                }
            }

            return Json(this._coreBusinessRules.ExcluirSorteio(raffle));
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> GetMyArchives([FromBody] SearchPersonBusinessArchive searchPersonBusinessArchive)
        {
            //Se o usuário estiver logado, não acessa essa página.
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));
            searchPersonBusinessArchive.PersonId = user.PersonId;

            return Json(this._coreBusinessRules.GetPersonBusinessWithArchives(searchPersonBusinessArchive));
        }

        [HttpPost]
        public IActionResult ExcluirPersonBusinessArchive([FromBody] PersonBusinessArchive personBusinessArchive)
        {
            // Deletar arquivo da azure aqui abaixo
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=armazenamento0;AccountKey=TyXnvuHIe5Bq+Syd0pJX2MwWcYpoq9s1GQLYjpW5l4KbTs7sICAtJ2Hqt0gNhWZLSox2N7j2RDy2+AStz2eXkw==;EndpointSuffix=core.windows.net";
            string containerName = personBusinessArchive.UserPath;
            string blobName = personBusinessArchive.Name;

            // Criar cliente BlobServiceClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Obter BlobClient para o arquivo desejado
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Excluir o arquivo, se existir
            bool deleted = blobClient.DeleteIfExists();

            return Json(this._coreBusinessRules.ExcluirPersonBusinessArchive(personBusinessArchive));
        }

        [HttpPost]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> SavePersonBusinesVideo(Microsoft.AspNetCore.Http.IFormFile videoFile)
        {
            string personBusinessArchiveValue = Request.Form["personBusinessArchive"];
            if (string.IsNullOrEmpty(personBusinessArchiveValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            if (videoFile == null || videoFile.Length == 0)
            {
                validationModel.success = false;
                validationModel.errorList.Add("Arquivo inválido ou não anexado!");
                return Json(validationModel);
            }

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;
            PersonBusinessArchive personBusinessArchive = null;

            try
            {

                personBusinessArchive = JsonConvert.DeserializeObject<PersonBusinessArchive>(personBusinessArchiveValue);


                string fileExtension = Path.GetExtension(videoFile.FileName);
                string newArchive = Guid.NewGuid().ToString() + fileExtension.ToLower();
                personBusinessArchive.Name = newArchive;
                personBusinessArchive.Extension = fileExtension.ToLower();
                personBusinessArchive.UserPath = User.Identity.Name.Replace("@", "").Replace(".", "");

                //Subir pra azure
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=armazenamento0;AccountKey=TyXnvuHIe5Bq+Syd0pJX2MwWcYpoq9s1GQLYjpW5l4KbTs7sICAtJ2Hqt0gNhWZLSox2N7j2RDy2+AStz2eXkw==;EndpointSuffix=core.windows.net";
                string containerName = personBusinessArchive.UserPath;
                string blobName = personBusinessArchive.Name;

                personBusinessArchive.Url = @$"https://armazenamento0.blob.core.windows.net/{containerName}/{blobName}";

                // Criar cliente BlobServiceClient
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Criar um contêiner se não existir
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                // Enviar o conteúdo do arquivo para o Azure Blob Storage
                using (var stream = videoFile.OpenReadStream())
                {
                    System.Threading.CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken cancellationToken = cancellationTokenSource.Token;


                    await containerClient.UploadBlobAsync(blobName, stream, cancellationToken: cancellationToken);
                }

            }
            catch (Exception ex)
            {
                validationModel.success = false;
                validationModel.errorList.Add("Ocorreu um erro. Contate a Administração!");
                return Json(validationModel);
            }

            return Json(this._coreBusinessRules.SavePersonBusinessArchive(personBusinessArchive));
        }

        [HttpPost]
        public async Task<IActionResult> SaveMyPersonBusinessArchive(string file)
        {

            string personBusinessArchiveValue = Request.Form["personBusinessArchive"];
            if (string.IsNullOrEmpty(personBusinessArchiveValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            if (string.IsNullOrEmpty(file))
            {
                validationModel.success = false;
                validationModel.errorList.Add("Arquivo inválido ou não anexado!");
                return Json(validationModel);
            }

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            PersonBusinessArchive personBusinessArchive = null;

            try
            {
                personBusinessArchive = JsonConvert.DeserializeObject<PersonBusinessArchive>(personBusinessArchiveValue);
                string folderUserPath = Path.Combine(webRootPath, "uploads", User.Identity.Name);

                if (!Directory.Exists(folderUserPath))
                {
                    //Se o diretório de arquivos do usuário não existir, vamos criar uma pra ele!
                    Directory.CreateDirectory(folderUserPath);
                }

                if (!string.IsNullOrEmpty(file))
                {
                    string base64String = file.Substring(file.IndexOf(',') + 1);
                    // supondo que a variável base64String contenha o conteúdo em formato Base64
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    // verificando a extensão a partir dos primeiros bytes
                    string extension = ".png";
                    if (imageBytes[0] == 255 && imageBytes[1] == 216 && imageBytes[2] == 255)
                    {
                        extension = ".jpg";
                    }

                    string newArchive = Guid.NewGuid().ToString() + extension.ToLower();
                    personBusinessArchive.Name = newArchive;
                    personBusinessArchive.Extension = extension.ToLower();
                    personBusinessArchive.UserPath = User.Identity.Name.Replace("@", "").Replace(".", "");

                    // salvando a imagem em um arquivo
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var image = Image.Load(ms))
                        {
                            // define as dimensões máximas da imagem
                            int alturaMaxima = 200;
                            int larguraMaxima = 200;


                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(larguraMaxima, alturaMaxima),
                                Mode = ResizeMode.Max
                            }));

                            // Salvar imagem redimensionada em disco
                            using (System.IO.MemoryStream imagemFormatada = new System.IO.MemoryStream())
                            {
                                image.SaveAsJpeg(imagemFormatada);
                                imagemFormatada.Position = 0;

                                //Subir pra azure
                                string connectionString = "DefaultEndpointsProtocol=https;AccountName=armazenamento0;AccountKey=TyXnvuHIe5Bq+Syd0pJX2MwWcYpoq9s1GQLYjpW5l4KbTs7sICAtJ2Hqt0gNhWZLSox2N7j2RDy2+AStz2eXkw==;EndpointSuffix=core.windows.net";
                                string containerName = personBusinessArchive.UserPath;
                                string blobName = personBusinessArchive.Name;

                                personBusinessArchive.Url = @$"https://armazenamento0.blob.core.windows.net/{containerName}/{blobName}";

                                // Criar cliente BlobServiceClient
                                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                                // Criar um contêiner se não existir
                                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);


                                // Criar BlobClient para o novo arquivo
                                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                                containerClient.UploadBlob(blobName, imagemFormatada);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logWritter.Writer($"excessão gerada: {ex.Message}");
                validationModel.success = false;
                validationModel.errorList.Add("Tivemos problema com teu arquivo. Entre em contato com o suporte ou suba um arquivo diferente!");
                return Json(validationModel);
            }

            return Json(this._coreBusinessRules.SavePersonBusinessArchive(personBusinessArchive));
        }
    
        [HttpPost]
        public IActionResult ExcludeCartProduct([FromBody] int id)
        {
            return Json(this._coreBusinessRules.ExcludeProductCard(id));
        }
     
        public async Task<string> GetMelhorEnvioToken()
        {
            string responseContent = "";
            string token = "";

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = "https://sandbox.melhorenvio.com.br/oauth/token";
                // Credenciais do aplicativo (substitua pelos valores reais)
                string clientId = "3842";
                string clientSecret = "t58s9USjkzOnAOxWbv5r6shuSg8VuiKFgaX4QWAW";
                string grantType = "client_credentials";

                // Construa os parâmetros da solicitação
                var requestData = new
                {
                    grant_type = grantType,
                    client_id = clientId,
                    client_secret = clientSecret
                };

                // Converte os parâmetros em formato JSON
                var content = new System.Net.Http.StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), System.Text.Encoding.UTF8, "application/json");

                // Faça a solicitação POST para obter o token
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Leitura da resposta em formato JSON
                    responseContent = await response.Content.ReadAsStringAsync();
                    dynamic tokenResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    token = tokenResponse["access_token"];
                }
            }
            return token;
        }

        [HttpPost]
        public ActionResult GetOrder([FromBody] int Id)
        {            
            return Json(this._coreBusinessRules.GetOrder(Id));
        }

        [HttpPost]
        public async Task<ActionResult> GetOrders()
        {
            //Aqui eu preciso puxar o usuário que está executando a ação.
            var aspUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var user = this._personBusinessRules.GetUserTiny(new Guid(aspUser.Id));
            int personId = 0;

            if (user != null && user.PersonId > 0)
            {
                personId = user.PersonId;
            }
            else
            {
                return RedirectToAction("Login");
            }


            return Json(this._coreBusinessRules.GetOrders(personId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveProductArchive(string file)
        {

            string productArchiveValue = Request.Form["productArchive"];
            if (string.IsNullOrEmpty(productArchiveValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            if (string.IsNullOrEmpty(file))
            {
                validationModel.success = false;
                validationModel.errorList.Add("Arquivo inválido ou não anexado!");
                return Json(validationModel);
            }

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;

            ProductArchive productArchive = null;

            try
            {
                productArchive = JsonConvert.DeserializeObject<ProductArchive>(productArchiveValue);
                string folderUserPath = Path.Combine(webRootPath, "uploads", User.Identity.Name);

                if (!Directory.Exists(folderUserPath))
                {
                    //Se o diretório de arquivos do usuário não existir, vamos criar uma pra ele!
                    Directory.CreateDirectory(folderUserPath);
                }

                if (!string.IsNullOrEmpty(file))
                {
                    string base64String = file.Substring(file.IndexOf(',') + 1);
                    // supondo que a variável base64String contenha o conteúdo em formato Base64
                    byte[] imageBytes = Convert.FromBase64String(base64String);

                    // verificando a extensão a partir dos primeiros bytes
                    string extension = ".png";
                    if (imageBytes[0] == 255 && imageBytes[1] == 216 && imageBytes[2] == 255)
                    {
                        extension = ".jpg";
                    }

                    string newArchive = Guid.NewGuid().ToString() + extension.ToLower();
                    productArchive.Name = newArchive;
                    productArchive.Extension = extension.ToLower();
                    productArchive.UserPath = User.Identity.Name.Replace("@", "").Replace(".", "");

                    // salvando a imagem em um arquivo
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        using (var image = Image.Load(ms))
                        {
                            // define as dimensões máximas da imagem
                            int alturaMaxima = 200;
                            int larguraMaxima = 200;


                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(larguraMaxima, alturaMaxima),
                                Mode = ResizeMode.Max
                            }));

                            // Salvar imagem redimensionada em disco
                            using (System.IO.MemoryStream imagemFormatada = new System.IO.MemoryStream())
                            {
                                image.SaveAsJpeg(imagemFormatada);
                                imagemFormatada.Position = 0;

                                //Subir pra azure
                                string connectionString = "DefaultEndpointsProtocol=https;AccountName=armazenamento0;AccountKey=TyXnvuHIe5Bq+Syd0pJX2MwWcYpoq9s1GQLYjpW5l4KbTs7sICAtJ2Hqt0gNhWZLSox2N7j2RDy2+AStz2eXkw==;EndpointSuffix=core.windows.net";
                                string containerName = productArchive.UserPath;
                                string blobName = productArchive.Name;

                                productArchive.Url = @$"https://armazenamento0.blob.core.windows.net/{containerName}/{blobName}";

                                // Criar cliente BlobServiceClient
                                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                                // Criar um contêiner se não existir
                                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);


                                // Criar BlobClient para o novo arquivo
                                BlobClient blobClient = containerClient.GetBlobClient(blobName);

                                containerClient.UploadBlob(blobName, imagemFormatada);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logWritter.Writer($"excessão gerada: {ex.Message}");
                validationModel.success = false;
                validationModel.errorList.Add("Tivemos problema com teu arquivo. Entre em contato com o suporte ou suba um arquivo diferente!");
                return Json(validationModel);
            }

            return Json(this._coreBusinessRules.SaveProductArchive(productArchive));
        }


        [HttpPost]
        [RequestSizeLimit(long.MaxValue)]
        public async Task<IActionResult> SaveProductVideo(Microsoft.AspNetCore.Http.IFormFile file)
        {
            string productArchiveValue = Request.Form["productArchive"];
            if (string.IsNullOrEmpty(productArchiveValue) || !User.Identity.IsAuthenticated)
            {
                return Json(false);
            }

            Models.ValidationModel validationModel = new Models.ValidationModel(new System.Collections.Generic.List<string>());

            if (file == null || file.Length == 0)
            {
                validationModel.success = false;
                validationModel.errorList.Add("Arquivo inválido ou não anexado!");
                return Json(validationModel);
            }

            // Obtenha o caminho da pasta wwwroot
            var webRootPath = _webHostEnvironment.WebRootPath;
            ProductArchive productArchive = null;

            try
            {

                productArchive = JsonConvert.DeserializeObject<ProductArchive>(productArchiveValue);


                string fileExtension = Path.GetExtension(file.FileName);
                string newArchive = Guid.NewGuid().ToString() + fileExtension.ToLower();
                productArchive.Name = newArchive;
                productArchive.Extension = fileExtension.ToLower();
                productArchive.UserPath = User.Identity.Name.Replace("@", "").Replace(".", "");

                //Subir pra azure
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=armazenamento0;AccountKey=TyXnvuHIe5Bq+Syd0pJX2MwWcYpoq9s1GQLYjpW5l4KbTs7sICAtJ2Hqt0gNhWZLSox2N7j2RDy2+AStz2eXkw==;EndpointSuffix=core.windows.net";
                string containerName = productArchive.UserPath;
                string blobName = productArchive.Name;

                productArchive.Url = @$"https://armazenamento0.blob.core.windows.net/{containerName}/{blobName}";

                // Criar cliente BlobServiceClient
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

                // Criar um contêiner se não existir
                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

                // Enviar o conteúdo do arquivo para o Azure Blob Storage
                using (var stream = file.OpenReadStream())
                {
                    System.Threading.CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                    CancellationToken cancellationToken = cancellationTokenSource.Token;


                    await containerClient.UploadBlobAsync(blobName, stream, cancellationToken: cancellationToken);
                }

            }
            catch (Exception ex)
            {
                validationModel.success = false;
                validationModel.errorList.Add("Ocorreu um erro. Contate a Administração!");
                return Json(validationModel);
            }

            return Json(this._coreBusinessRules.SaveProductArchive(productArchive));
        }

        [HttpPost]
        public IActionResult ExcluirProductArchive([FromBody] ProductArchive productArchive)
        {
            // Deletar arquivo da azure aqui abaixo
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=armazenamento0;AccountKey=TyXnvuHIe5Bq+Syd0pJX2MwWcYpoq9s1GQLYjpW5l4KbTs7sICAtJ2Hqt0gNhWZLSox2N7j2RDy2+AStz2eXkw==;EndpointSuffix=core.windows.net";
            string containerName = productArchive.UserPath;
            string blobName = productArchive.Name;

            // Criar cliente BlobServiceClient
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Obter BlobClient para o arquivo desejado
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            // Excluir o arquivo, se existir
            bool deleted = blobClient.DeleteIfExists();

            return Json(this._coreBusinessRules.ExcluirProductArchive(productArchive));
        }

    }
}
