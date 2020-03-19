using System.Threading.Tasks;
using Pokemonsieur.Shakespeare.Model;

namespace Pokemonsieur.Shakespeare.Service
{
    public interface IPokemonsieurService
    {   
        /// <summary>
        /// Get Pokemon details and translate as required
        /// </summary>
        /// <param name="name">Pokemon Name</param>
        /// <returns></returns>
         Task<Model.Pokemonsieur> GetDetailsAndTranslateAsync(string name);

        /// <summary>
        /// Get Error Details
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
         Error GetErrorDetails(int statusCode);
    }
}