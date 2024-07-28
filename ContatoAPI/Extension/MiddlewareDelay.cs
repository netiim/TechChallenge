namespace ContatoAPI.Extension
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public class RequestDelayMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _delayMilliseconds;

        public RequestDelayMiddleware(RequestDelegate next, int delayMilliseconds)
        {
            _next = next;
            _delayMilliseconds = delayMilliseconds;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Adiciona um delay artificial
            await Task.Delay(_delayMilliseconds);
            await _next(context);
        }
    }
}
