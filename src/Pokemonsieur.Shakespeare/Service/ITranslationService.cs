using System.Threading.Tasks;
using Pokemonsieur.Shakespeare.Model;

namespace Pokemonsieur.Shakespeare.Service
{
    /// <summary>
    /// Translation Service - Calls "fun translation API" to translate 
    /// the given text required translation - configured via appsetting
    /// </summary>
    public interface ITranslationService
    {
        /// <summary>
        /// Get the desired translation 
        ///     Configure via appsetting
        /// </summary>
        /// <param name="text">Text to be translated</param>
        /// <returns>Translated text</returns>
         Task<Translation> GetTranslationAsync(string text);
    }
}