using System.Security.Claims;
using Application.Extension.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace WebUI.Components.Layout.Identity;

internal sealed class IdentityRevalidatingAuthStateProvider(ILoggerFactory loggerFactory, IServiceScopeFactory scopeFactory, IOptions<IdentityOptions> options):RevalidatingServerAuthenticationStateProvider(loggerFactory)
{
    protected override TimeSpan RevalidationInterval => TimeSpan.FromSeconds(20);

    protected async override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
        CancellationToken cancellationToken)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await ValidateAuthenticationStateAsync(userManager, authenticationState.User);
    }

    private async Task<bool> ValidateAuthenticationStateAsync(UserManager<ApplicationUser> authenticationState, ClaimsPrincipal authenticationStateUser)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager,
        ClaimsPrincipal principal)
    {
        var user = await userManager.GetUserAsync(principal);
        if (user is null) return false;
        else if (!userManager.SupportsUserSecurityStamp) return true;
        else
        {
            var principalStamp = principal.FindFirstValue(options.Value.ClaimsIdentity.ToString());
            var userStamp = await userManager.GetSecurityStampAsync(user);
            return principalStamp == userStamp;
        }
    }
}