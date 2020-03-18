namespace Pokemonsieur.Shakespeare
{
    /// <summary>
    /// Application Setting
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Poke API Configuration
        /// </summary>
        /// <value></value>
        public PokeApi PokeApi { get; set; }

        /// <summary>
        /// Translation Configuration
        /// </summary>
        /// <value></value>
        public TranslationApi TranslationApi { get; set; }
        
    }

    public class TranslationApi
    {   
        /// <summary>
        /// Api Url
        /// </summary>
        /// <value></value>     
        public string Url { get; set; }

        /// <summary>
        /// Translation API
        /// </summary>
        /// <value></value>
        public string Type { get; set; }
    }

    public class PokeApi
    {
        /// <summary>
        /// Default Language
        /// </summary>
        /// <value></value>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// Poke API Url
        /// </summary>
        /// <value></value>
        public string Url { get; set; }

        /// <summary>
        /// API Key
        /// </summary>
        /// <value></value>
        public string Key { get; set; }
    }
}