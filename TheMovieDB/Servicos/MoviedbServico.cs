using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.TMDb;
using System.Threading;
using System.Threading.Tasks;
using TheMovieDB.Models;

namespace TheMovieDB.Servicos
{
    /// <summary>
    /// Serviço que encapsula os métodos do repositório.
    /// </summary>
    public class MoviedbServico
    {
        /// <summary>
        /// Instância do repositório, que será recebido via injação de dependência.
        /// </summary>
        private readonly Repositorio _repositorio;

        public MoviedbServico(Repositorio repositorio)
        {
            _repositorio = repositorio;
        }
        
        /// <summary>
        /// Obtém do WebApi os destaques da semana.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Movie>> ObterDestaqueSemana(CancellationToken cancellationToken, int page = 1)
        {
            var dataAtual = DateTime.Now;
            var dataInicial = ObterInicioSemana(dataAtual);
            var resultado = await _repositorio.ObterDestaqueSemana(cancellationToken, dataInicial, page);

            return resultado.Where(x =>
                x.ReleaseDate >= ObterInicioSemana(dataAtual)
                && x.ReleaseDate <= dataAtual).ToList();
        }

        /// <summary>
        /// Obtém do WebApi uma lista de filmes lançados no mês corrente.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="page"></param>
        /// <returns>lista de filmes lançados no mês.</returns>
        public async Task<IEnumerable<Movie>> ObterDestaqueMes(CancellationToken cancellationToken, int page = 1)
        {
            return await _repositorio.ObterDestaqueMes(cancellationToken, page);
        }

        /// <summary>
        /// Obtém do WebApi o filme conforme o id.
        /// </summary>
        /// <param name="id">id do filme.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Retorna os dados do filme de id informado.</returns>
        public async Task<Movie> ObterPorId(int id, CancellationToken cancellationToken)
        {
            return await _repositorio.ObterPorId(id, cancellationToken);
        }

        /// <summary>
        /// Obtém uma lista de filmes a partir do título do filme.
        /// </summary>
        /// <param name="query">título do filme.</param>
        /// <param name="cancellationToken"></param>
        /// <param name="page"></param>
        /// <returns>Retorna os filmes que atendem os critérios da pesquisa.</returns>
        public async Task<Movies> ObterPorTitulo(string query, CancellationToken cancellationToken, int page = 1)
        {
            return await _repositorio.ObterPorTitulo(query, cancellationToken);
        }

        /// <summary>
        /// Calcula a data inicial da semana, conforme uma data informada.
        /// </summary>
        /// <param name="dataAtual">Data da semana que se deseja saber quando é o início da semana.</param>
        /// <returns>Retorna a data de início da semana.</returns>
        public DateTime ObterInicioSemana(DateTime dataAtual)
        {
            return dataAtual.AddDays(-(int)dataAtual.DayOfWeek);
        }
    }
}