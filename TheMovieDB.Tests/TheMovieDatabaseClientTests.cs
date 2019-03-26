using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TheMovieDB.Tests
{
    [TestClass]
    public class TheMovieDatabaseClientTests
    {
        public string ApiKey
        {
            get { return "0f35e31d8c70cc8a5de68ad0e5810c94"; }
        }

        [TestMethod]
        public void TestarObterNumeroSemana()
        {
            var numeroSemanaAtual = ObterNumeroSemana(DateTime.Now);
            Assert.AreEqual(13, numeroSemanaAtual);

            var numeroSemanaPassada = ObterNumeroSemana(DateTime.Now.AddDays(-1));
            Assert.AreEqual(12, numeroSemanaPassada);

            var numeroSemanaFilme = ObterNumeroSemana(new DateTime(2019, 3, 1));
            Assert.AreNotEqual(numeroSemanaFilme, numeroSemanaAtual);
        }

        [TestMethod]
        public void TestarObterDestaqueSemana()
        {
            CancellationToken cancellationToken;
            var pageNumber = 1;
            int totalPages;

            do
            {
                var destaqueSemana = ObterDestaqueSemana(cancellationToken, pageNumber).Result;
                Console.WriteLine("Page {0} of {1} ({2} total results)", 
                    destaqueSemana.PageIndex, 
                    destaqueSemana.PageCount,
                    destaqueSemana.TotalCount);

                foreach (var info in destaqueSemana.Results)
                {
                    Console.WriteLine("{0} ({1}): {2}", info.Title, info.ReleaseDate, info.Overview);
                }
                totalPages = destaqueSemana.PageCount;

                Assert.IsNotNull(destaqueSemana);
            }
            while (pageNumber++ < totalPages);
        }

        [TestMethod]
        public void TestarObterDestaqueMes()
        {
            CancellationToken cancellationToken;
            var listaDestaqueMes = ObterDestaqueMes(cancellationToken).Result;

            Assert.IsNotNull(listaDestaqueMes);
        }

        [TestMethod]
        public void TestarObterPorId()
        {
            CancellationToken cancellationToken;
            const int id = 105;
            var filme = ObterPorId(id, cancellationToken).Result;

            Assert.AreEqual(filme.Id, id);
        }

        [TestMethod]
        public void TestarObterPorTitulo()
        {
            CancellationToken cancellationToken;
            var pageNumber = 1;
            int totalPages;

            do
            {
                const string titulo = "Harry";
                var filmes = ObterPorTitulo(titulo, cancellationToken).Result;
                Console.WriteLine("Page {0} of {1} ({2} total results)",
                    filmes.PageIndex,
                    filmes.PageCount,
                    filmes.TotalCount);

                foreach (var info in filmes.Results)
                {
                    Console.WriteLine("{0} ({1}): {2}", info.Title, info.ReleaseDate, info.Overview);
                }
                totalPages = filmes.PageCount;

                Assert.IsNotNull(filmes);
            }
            while (pageNumber++ < totalPages);
        }

        [TestMethod]
        public void TestarInicioSemana()
        {
            var esperado = "24/03/2019";
            
            var atual = ObterInicioSemana(new DateTime(2019, 3, 24));
            Assert.AreEqual(atual.ToString("dd/MM/yyyy"), esperado);

            atual = ObterInicioSemana(new DateTime(2019, 3, 28));
            Assert.AreEqual(atual.ToString("dd/MM/yyyy"), esperado);
            
            atual = ObterInicioSemana(new DateTime(2019, 3, 30));
            Assert.AreEqual(atual.ToString("dd/MM/yyyy"), esperado);
        }

        public DateTime ObterInicioSemana(DateTime dataAtual)
        {
            return dataAtual.AddDays(-(int)dataAtual.DayOfWeek);
        }
        public async Task<Movies> ObterDestaqueSemana(CancellationToken cancellationToken, int page = 1, string language = "pt-BR")
        {
            using (var client = new ServiceClient(ApiKey))
            {
                return await client.Movies.GetNowPlayingAsync(language, page, cancellationToken);
            }
        }

        public async Task<IEnumerable<Movie>> ObterDestaqueMes(CancellationToken cancellationToken, int pageNumber = 1, string language = "pt-BR")
        {
            var listaMovies = new List<Movie>();
            using (var client = new ServiceClient(ApiKey))
            {
                int totalPages;
                do
                {
                    var movies = await client.Movies.GetNowPlayingAsync(language, pageNumber, cancellationToken);
                    totalPages = movies.PageCount;

                    listaMovies.AddRange(movies.Results);
                    Thread.Sleep(500);

                } while (pageNumber++ < totalPages);
            }
            return listaMovies.Where(movie => movie.ReleaseDate.HasValue 
                && movie.ReleaseDate.Value.Year == DateTime.Now.Year 
                && movie.ReleaseDate.Value.Month == DateTime.Now.Month);
        }

        public async Task<Movie> ObterPorId(int id, CancellationToken cancellationToken, string language = "pt-BR")
        {
            using (var client = new ServiceClient(ApiKey))
            {
                return await client.Movies.GetAsync(id, language, true, cancellationToken);
            }
        }

        public async Task<Movies> ObterPorTitulo(string query, CancellationToken cancellationToken, int page = 1, string language = "pt-BR")
        {
            using (var client = new ServiceClient(ApiKey))
            {
                return await client.Movies.SearchAsync(query, language, true, null, true, page, cancellationToken);
            }
        }

        public int ObterNumeroSemana(DateTime date)
        {
            var gregorianCalendar = new GregorianCalendar();
            return gregorianCalendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }


        
    }
}
