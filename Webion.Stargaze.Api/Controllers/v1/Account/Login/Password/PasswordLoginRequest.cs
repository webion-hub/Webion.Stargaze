using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Controllers.v1.Account.Login.Password;

public sealed class PasswordLoginRequest
{
    [Required]
    public Guid ClientId { get; init; }
    
    [Required]
    [Base64String]
    public string ClientSecret { get; init; } = null!;
    
    [Required]
    public string UserName { get; init; } = null!;
    
    [Required]
    [PasswordPropertyText]
    public string Password { get; init; } = null!;
}