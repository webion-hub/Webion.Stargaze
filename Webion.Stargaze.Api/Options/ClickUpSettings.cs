using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Options;

public sealed class ClickUpSettings
{
    public const string Section = "ClickUp";
    
    [Required]
    public string TeamId { get; init; } = null!;
    
    [Required]
    public string ApiKey { get; init; } = null!;
    
    [Required]
    public string ClientId { get; init; } = null!;
    
    [Required]
    public string ClientSecret { get; init; } = null!;
}