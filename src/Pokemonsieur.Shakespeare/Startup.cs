using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PokeApiNet;
using Pokemonsieur.Shakespeare.Model;
using Pokemonsieur.Shakespeare.Service;
using System.Text.Json;


namespace Pokemonsieur.Shakespeare
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets access to information about the web hosting environment that the FTB Offer in running within.
        /// </summary>
        public IHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            }); 

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Pokemonsieur API",
                    Description = "Return Shakespeariean pokemon description"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<AppSettings>(Configuration)
                .AddScoped<ITranslationService, TranslationService>()
                .AddScoped<IPokemonService, PokemonService>()
                .AddScoped<IPokemonsieurService, PokemonsieurService>();

            services.AddHttpClient(nameof(PokeApi), c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>($"{nameof(PokeApi)}:{nameof(PokeApi.Url)}"));
            }).AddTypedClient(c => Refit.RestService.For<IRestApiClient<PokemonSpecies, PokemonQueryParams, string>>(c));


            services.AddHttpClient(nameof(TranslationApi), c =>
            {
                c.BaseAddress = new Uri(Configuration.GetValue<string>($"{nameof(TranslationApi)}:{nameof(TranslationApi.Url)}"));
            }).AddTypedClient(c => Refit.RestService.For<IRestApiClient<Translation, TranslationQueryParams, string>>(c));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokemonsieur API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}