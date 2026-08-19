using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace CrmImobiliaria.Web.Components.Account
{
    internal sealed class IdentityRedirectManager(NavigationManager navigationManager, IHttpContextAccessor httpContextAccessor)
    {
        public const string StatusCookieName = "Identity.StatusMessage";

        private static readonly CookieBuilder StatusCookieBuilder = new()
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true,
            IsEssential = true,
            MaxAge = TimeSpan.FromSeconds(5),
        };

        public void RedirectTo(string? uri)
        {
            uri ??= "";
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
                uri = navigationManager.ToBaseRelativePath(uri);

            // O projeto roda com BlazorDisableThrowNavigationException=true, então NavigationManager.NavigateTo
            // não lança NavigationException para virar redirect automaticamente durante SSR estático — por isso
            // as páginas de conta (que rodam fora do circuito interativo) redirecionam direto pelo HttpContext.
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext is not null && !httpContext.Response.HasStarted)
            {
                httpContext.Response.Redirect(navigationManager.ToAbsoluteUri(uri).ToString());
                return;
            }

            navigationManager.NavigateTo(uri, forceLoad: true);
        }

        public void RedirectTo(string uri, Dictionary<string, object?> queryParameters)
        {
            var uriSemQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
            var novaUri = navigationManager.GetUriWithQueryParameters(uriSemQuery, queryParameters);
            RedirectTo(novaUri);
        }

        public void RedirectToWithStatus(string uri, string mensagem, HttpContext context)
        {
            context.Response.Cookies.Append(StatusCookieName, mensagem, StatusCookieBuilder.Build(context));
            RedirectTo(uri);
        }

        private string CaminhoAtual => navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);

        public void RedirectToCurrentPage() => RedirectTo(CaminhoAtual);

        public void RedirectToCurrentPageWithStatus(string mensagem, HttpContext context)
            => RedirectToWithStatus(CaminhoAtual, mensagem, context);
    }
}
