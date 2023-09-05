using DevMS.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DevMS.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogTrace("index-home");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/erro/{id:length(3,3)}")]
        public IActionResult Error(int id)
        {
            var errorModel = new ErrorViewModel();

            if(id == 500)
            {
                errorModel.CodigoErro = id;
                errorModel.Titulo = "Ocorreu um erro!";
                errorModel.Mensagem = "Ops, ocorreu um erro. Tente novamente mais tarde ou entre em contato com nosso suporte.";
            }
            else if(id == 404)
            {
                errorModel.CodigoErro = id;
                errorModel.Titulo = "Página não encontrada!";
                errorModel.Mensagem = "A página que está procurando não exite. <br/> Em caso de dúvidas entre em contato com nosso suporte";
            }
            else if(id == 403)
            {
                errorModel.CodigoErro = id;
                errorModel.Titulo = "Acesso negado!";
                errorModel.Mensagem = "Você não tem permissão para fazer isso.";
            }
            else
            {
                return StatusCode(500);
            }

            return View("Error", errorModel);
        }
    }
}