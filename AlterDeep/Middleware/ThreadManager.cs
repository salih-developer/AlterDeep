using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using AlterDeep.Logging;
using Newtonsoft.Json;

namespace AlterDeep.Middleware
{
    public class ThreadLogManager
    {
        RequestDelegate next;
        private readonly ILogger<ThreadLogManager> _logger;

        public ThreadLogManager(RequestDelegate next, ILogger<ThreadLogManager> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IThreadLog threadLog)
        {

            var originalBodyStream = context.Response.Body;

            try
            {
                #region Request Logging

                var memoryBodyStream = new MemoryStream();

                context.Response.Body = memoryBodyStream;

                var log = new ApiRequestResponse
                {
                    Path = context.Request.Path,
                    Method = context.Request.Method,
                    QueryString = context.Request.QueryString.ToString()
                };

                if (context.Request.Method == "POST")
                {
                    context.Request.EnableBuffering();
                    var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    context.Request.Body.Position = 0;
                    log.Payload = body;
                }

                log.RequestedOn = DateTime.Now;

                #endregion

                await this.next.Invoke(context);

                #region response logging

                memoryBodyStream.Seek(0, SeekOrigin.Begin);
                string responseBody = await new StreamReader(memoryBodyStream).ReadToEndAsync();
                memoryBodyStream.Seek(0, SeekOrigin.Begin);

                await memoryBodyStream.CopyToAsync(originalBodyStream);

                log.Response = responseBody;
                log.ResponseCode = context.Response.StatusCode.ToString();
                log.IsSuccessStatusCode = (
                    context.Response.StatusCode == 200 ||
                    context.Response.StatusCode == 201);
                log.RespondedOn = DateTime.Now;
                await threadLog.Insert(log);

                #endregion
                context.Response.Body = originalBodyStream;
            }
            //catch (Exception ex)
            //{
            //    _logger.LogError($"Something went wrong: {ex}");
            //    await HandleExceptionAsync(context);
            //}
            finally
            {
                    context.Response.Body = originalBodyStream;
            }
        }
        //private Task HandleExceptionAsync(HttpContext context)
        //{
        //    context.Response.ContentType = "application/json";
        //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //    var originalBodyStream = context.Response.Body;
        //    return context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorDetails()
        //    {
        //        StatusCode = context.Response.StatusCode,
        //        Message = "Internal Server Error from the custom middleware."
        //    }));
        //}

    }
}
