namespace Blog2.BLL
{
    public class ExceptionMappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMappingMiddleware> _logger;

        public ExceptionMappingMiddleware(RequestDelegate next, ILogger<ExceptionMappingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Логируем любую необработанную ошибку в errors.log
                _logger.LogError(ex, "Произошла необработанная ошибка при выполнении запроса {Path}", context.Request.Path);

                // Прокидываем ошибку дальше или перенаправляем на страницу ошибки
                throw;
            }
        }
    }
}
