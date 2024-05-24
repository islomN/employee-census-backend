using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Api.DataAccess;
using Api.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Models;

namespace Api.AuthenticationHandler;

public class BasicAuthenticationHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserDataAccess _userDataAccess;
    private static readonly char[] Separator = new[] { ':' };

    /// <summary>
    ///
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="encoder"></param>
    /// <param name="clock"></param>
    /// <param name="userDataAccess"></param>
    [Obsolete("Obsolete")]
    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IUserDataAccess userDataAccess)
        : base(options, logger, encoder, clock)
    {
        _userDataAccess = userDataAccess;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var value))
            return AuthenticateResult.Fail("Missing Authorization Header");

        Result<UserModel> userResult;
        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(value!);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(Separator, 2);
            var username = credentials[0];
            var password = credentials[1];
            userResult = await GetUser(username, password, Context.RequestAborted);
        }
        catch
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        if (userResult.Payload == null)
            return AuthenticateResult.Fail(userResult.Message!);
        
        await _userDataAccess.UpdateActivity(userResult.Payload.Id, Context.RequestAborted);
        
        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, userResult.Payload.Id.ToString()),
            new Claim(ClaimTypes.Name, userResult.Payload.Username),
            new Claim(ClaimTypes.Role, userResult.Payload.Role.ToString()),
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        
        return AuthenticateResult.Success(ticket);
    }

    private async Task<Result<UserModel>> GetUser(
        string username,
        string password,
        CancellationToken cancellationToken)
    {
        var result = await _userDataAccess.GetByUsername(username, cancellationToken);

        if (!result.IsSuccess)
        {
            return result;
        }

        if (result.Payload!.PasswordHash != CryptoHelper.ComputeSha256Hash(password))
        {
            return new Result<UserModel>(
                null,
                "Password error",
                false);
        }

        return result;
    }
}
