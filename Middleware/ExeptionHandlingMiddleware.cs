using System.Net;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyShopPet.Models.DTOs.ErrorDTO;

namespace MyShopPet.Middleware
{
    public class ExeptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
       
        private readonly ILogger<ExeptionHandlingMiddleware> _logger;
        public ExeptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExeptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            const int PrimaryKeyExeptionNumber = 2627;
            const int DuplicateIndexExeptionNumber = 2601;
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlException &&
                (sqlException.Number == PrimaryKeyExeptionNumber || sqlException.Number == DuplicateIndexExeptionNumber))
            {
                await HandleExeptionAsync(context, ex.Message, HttpStatusCode.InternalServerError, "The entity is already exist!");
            }
            catch (Exception ex)
            {
                await HandleExeptionAsync(context, ex.Message, HttpStatusCode.InternalServerError, "The internal server exeption!");
            }
        }
        private async Task HandleExeptionAsync(HttpContext context,
            string exMsg,
            HttpStatusCode statusCode,
            string message)
        {
            _logger.LogError(exMsg);
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)statusCode;
            ErrorDTO error = new() { Message = exMsg, StatusCode = (int)statusCode };
            string result = JsonSerializer.Serialize(error);
            await response.WriteAsJsonAsync(result);

        }
    }
}
