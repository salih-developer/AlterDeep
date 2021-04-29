using System;
using System.Net;
using AlterDeep.DBOperations;
using AlterDeep.DBOperations.Model;
using AlterDeep.Log;
using AlterDeep.Middleware;
using AlterDeep.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AlterDeep
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddDbContext<DeepContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton<IThreadLog, ElasticThreadLogger>();
            services.AddScoped<IDeepLinkService, DeepLinkService>();
            services.AddLogging();
            services.AddControllers();
            services.AddApiVersioning(v =>
            {
                v.ReportApiVersions = true;
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
            });
            services.AddVersionedApiExplorer(
                 options =>
                 {
                     options.GroupNameFormat = "'v'VVV";
                     options.SubstituteApiVersionInUrl = true;
                 });
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerFactory loggerFactory,ILogger<Startup> logger)
        {
            loggerFactory.AddFile("Logs/mylog-{Date}.txt");
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                logger.LogError(exception,exception.Message); 

                var result = JsonConvert.SerializeObject(new { message = "Sistem yöneticiniz ile görüþün." });
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(result);
            }));

            
            app.UseMiddleware<ThreadLogManager>();
            app.UseRouting();

            app.UseAuthorization();
            app.UseSwagger();

            app.UseSwaggerUI(
                  options =>
                  {
                      foreach (var description in provider.ApiVersionDescriptions)
                      {
                          options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                              description.GroupName.ToUpperInvariant());
                      }
                  });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Db Create
            try
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    DeepContext context = serviceScope.ServiceProvider.GetRequiredService<DeepContext>();
                    try
                    {

                        context.Database.Migrate();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }

                    TransactionPage transactionPage = new TransactionPage { Name = "Transaction", FriendlyName = "eft" };
                    Content content = new Content { ContentText = "test", Id = 123 };
                    TransactionPageContents transactionPageContents = new TransactionPageContents { Content = content, TransactionPage = transactionPage };
                    Flow flow = new Flow { Id = 352, Name = "TestEftFlow" };
                    context.TransactionPages.Add(transactionPage);
                    context.Contents.Add(content);
                    context.TransactionPageContents.Add(transactionPageContents);
                    context.Flows.Add(flow);
                    context.SaveChanges();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            #endregion
        }
    }
}
