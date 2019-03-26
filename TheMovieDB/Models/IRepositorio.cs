using System;
using System.Collections.Generic;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;

namespace TheMovieDB.Models
{
    /// <summary>
    /// Respositório abstrato
    /// </summary>
    public interface IRepositorio
    {
        Task<IEnumerable<Movie>> ObterDestaqueSemana(CancellationToken cancellationToken, DateTime dataInicial, int page = 1, string language = "pt-BR");
        Task<IEnumerable<Movie>> ObterDestaqueMes(CancellationToken cancellationToken, int page = 1, string language = "pt-BR");
        Task<Movie> ObterPorId(int id, CancellationToken cancellationToken, string language = "pt-BR");
        Task<Movies> ObterPorTitulo(string query, CancellationToken cancellationToken, int page = 1, string language = "pt-BR");
    }
}