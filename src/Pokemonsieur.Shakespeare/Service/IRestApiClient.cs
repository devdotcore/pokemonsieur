using System;
using System.Net.Http;
using System.Threading.Tasks;
using Pokemonsieur.Shakespeare.Model;
using Refit;

namespace Pokemonsieur.Shakespeare.Service
{
    [Headers("ContentType: application/json")]
    public interface IRestApiClient<T, T1, in TKey> where T : class
    {
        [Get("/{**key}")]
        Task<T> Get(TKey key, T1 queryParam);
    }
}