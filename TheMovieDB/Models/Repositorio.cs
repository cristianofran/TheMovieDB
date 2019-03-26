using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;

namespace TheMovieDB.Models
{
    /// <summary>
    /// Classe de repositório concreto.
    /// </summary>
    public class Repositorio : IRepositorio
    {
        /// <summary>
        /// Chave de acesso a WebApi.
        /// </summary>
        public string ApiKey
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiKey"];
            }
        }

        /// <summary>
        /// Lista de filmes destaques da semana.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="dataInicial"></param>
        /// <param name="page"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>> ObterDestaqueSemana(CancellationToken cancellationToken, DateTime dataInicial, int page = 1, string language = "pt-BR")
        {
            using (var client = new ServiceClient(ApiKey))
            {
                var movies = await client.Movies.DiscoverAsync(language, true, null,
                    dataInicial, DateTime.Now, null, null, null, null, page, cancellationToken);
                return movies.Results;
            }
        }

        /// <summary>
        /// Lista de filmes destaques do mês.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="page"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>> ObterDestaqueMes(CancellationToken cancellationToken, int page = 1, string language = "pt-BR")
        {
            using (var client = new ServiceClient(ApiKey))
            {
                var movies = await client.Movies.GetNowPlayingAsync(language, page, cancellationToken);
                return movies.Results.Where(movie =>
                        !string.IsNullOrEmpty(movie.Poster) && movie.ReleaseDate.HasValue
                        && movie.ReleaseDate.Value.Year == DateTime.Now.Year
                        && movie.ReleaseDate.Value.Month == DateTime.Now.Month)
                    .OrderBy(x => x.Title);
            }
        }

        /// <summary>
        /// Busca por id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<Movie> ObterPorId(int id, CancellationToken cancellationToken, string language = "pt-BR")
        {
            using (var client = new ServiceClient(ApiKey))
            {
                return await client.Movies.GetAsync(id, language, true, cancellationToken);
            }
        }

        /// <summary>
        /// Busca por título.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="page"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<Movies> ObterPorTitulo(string query, CancellationToken cancellationToken, int page = 1, string language = "pt-BR")
        {
            using (var client = new ServiceClient(ApiKey))
            {
                return await client.Movies.SearchAsync(query, language, true, null, true, page, cancellationToken);
            }
        }
    }
}