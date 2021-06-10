using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Parky.web.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Parky.web.Models.ViewModel;
using Parky.web.Repository;

namespace Parky.web.Controllers
{
    public class HomeController : Controller
    {
        //todo: if user is not authorized (not admin) they will get page not found.  Need to implement pages for users when they are not authorized

        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _nationalParkRepo;
        private readonly ITrailRepository _trailRepo;
        private readonly IAccountRepository _accountRepo;

        private const string TOKEN_NAME = "JWToken";

        public HomeController(ILogger<HomeController> logger, 
            INationalParkRepository nationalParkRepo, 
            ITrailRepository trailRepo,
            IAccountRepository accountRepo)
        {
            _logger = logger;
            _nationalParkRepo = nationalParkRepo;
            _trailRepo = trailRepo;
            _accountRepo = accountRepo;
        }

        public async Task<IActionResult> Index()
        {
            var vm = new IndexViewModel()
            {
                NationalParks = await _nationalParkRepo.GetAllAsync(Routing.NationalParkRoute, HttpContext.Session.GetString(TOKEN_NAME)),
                Trails = await _trailRepo.GetAllAsync(Routing.TrailsRoute, HttpContext.Session.GetString(TOKEN_NAME))
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            var user = new User();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var loginUser = await _accountRepo.LoginAsync(Routing.AccountAPIPath + "authenticate/", user);
            if (loginUser.Token == null)
            {
                return View();
            }
            HttpContext.Session.SetString(TOKEN_NAME, loginUser.Token);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            var result = await _accountRepo.RegisterAsync(Routing.AccountAPIPath + "register/", user);
            if (!result) return View();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.SetString(TOKEN_NAME, string.Empty);
            return RedirectToAction("Index");
        }
    }
}
