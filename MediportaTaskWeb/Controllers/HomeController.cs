using MediportaTaskWeb.Models;
using MediportaTaskWeb.Services.TagService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MediportaTaskWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITagService _tagService;

        public HomeController(ILogger<HomeController> logger, ITagService tagService)
        {
            _logger = logger;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel();
            model.Tags = await _tagService.GetMostPopularTagsAsync(1000);
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}