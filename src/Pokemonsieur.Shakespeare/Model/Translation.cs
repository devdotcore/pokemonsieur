using Refit;

namespace Pokemonsieur.Shakespeare.Model
{
    public class Translation : BaseModel
    {
        /// <summary>
        ///  Contents 
        /// </summary>
        /// <value></value>
        public Contents Contents { get; set; }

    }

    public class Contents
    {
        /// <summary>
        /// Translated Text
        /// </summary>
        /// <value></value>
        public string Translated { get; set; }

        /// <summary>
        /// Input Text
        /// </summary>
        /// <value></value>
        public string Text { get; set; }

        /// <summary>
        /// Translation Type
        /// </summary>
        /// <value></value>
        public string Translation { get; set; }
    }

    public class TranslationQueryParams
    {
        /// <summary>
        /// API Query
        /// </summary>
        /// <value></value>
        [AliasAs("text")]
        public string Query { get; set; }
    }
}