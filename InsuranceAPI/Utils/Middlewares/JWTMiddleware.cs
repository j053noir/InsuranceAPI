using InsuranceAPI.Infrastructure.Services.Interfaces;

namespace InsuranceAPI.Utils.Middlewares
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;

        public JWTMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IAuthenticationService authenticationService)
        {
            var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(' ').Last();
            var userId = authenticationService.ValidateToken(token);

            if (userId != null)
            {
                context.Items["User"] = await userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}
