using System.Web.Mvc;
using TheMovieDB.Models;
using TheMovieDB.Servicos;
using Unity;
using Unity.AspNet.Mvc;

namespace TheMovieDB
{
    /// <summary>
    /// Classe de configuração da injeção de dependência.
    /// </summary>
    public class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();
            container.RegisterType<IRepositorio, Repositorio>();
            container.RegisterType<MoviedbServico>();

            return container;
        }
    }
}