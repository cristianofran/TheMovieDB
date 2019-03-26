
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using TheMovieDB.Servicos;

namespace TheMovieDB.Controllers
{
    public class HomeController : Controller
    {
        private readonly MoviedbServico _moviedbServico;

        public HomeController(MoviedbServico moviedbServico)
        {
            _moviedbServico = moviedbServico;
        }

        /// <summary>
        /// Ao iniciar o sistema, exibe a página de filmes em Destaque da Semana.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("DestaqueSemana");
        }

        public ActionResult DestaqueSemana()
        {
            return View();
        }

        public ActionResult DestaqueMes()
        {
            return View();
        }
        
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// Exibe os detalhes do filme informado.
        /// </summary>
        /// <param name="id">id do filme.</param>
        /// <returns></returns>
        public async Task<ActionResult> Detalhes(int id)
        {
            CancellationToken cancellationToken;
            var filme = await _moviedbServico.ObterPorId(id, cancellationToken);

            return View(filme);
        }

        /// <summary>
        /// Recebe os dados vindos da caixa de pesquisa.
        /// </summary>
        /// <param name="titulo">título do filme.</param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Pesquisar([FromBody] string titulo)
        {
            ViewBag.Query = titulo;

            CancellationToken cancellationToken;
            var listaMovies = await _moviedbServico.ObterPorTitulo(titulo, cancellationToken);

            return View(listaMovies);
        }

        /// <summary>
        /// Invocado por ajax, ao entrar na página de "Destaques da Semana", 
        /// retorna uma lista de filmes de destaque da semana.
        /// </summary>
        /// <returns>lista de filmes da semana.</returns>
        public async Task<JsonResult> ObterDestaqueSemana()
        {
            CancellationToken cancellationToken;
            var listaMovies = await _moviedbServico.ObterDestaqueSemana(cancellationToken);
            return Json(new { result = listaMovies }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Invocado por ajax, ao entrar na página de "Destaques do Mês", 
        /// retorna uma lista de filmes de destaque do mês.
        /// </summary>
        /// <param name="pageNumber">número da página</param>
        /// <returns>lista de filmes do mês.</returns>
        public async Task<JsonResult> ObterDestaqueMes(int pageNumber)
        {
            CancellationToken cancellationToken;
            var listaMovies = await _moviedbServico.ObterDestaqueMes(cancellationToken, pageNumber);
            return Json(new { result = listaMovies }, JsonRequestBehavior.AllowGet);
        }
    }
}
