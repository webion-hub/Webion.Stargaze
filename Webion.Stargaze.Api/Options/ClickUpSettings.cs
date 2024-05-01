using System.ComponentModel.DataAnnotations;

namespace Webion.Stargaze.Api.Options;

public sealed class ClickUpSettings
{
    public const string Section = "ClickUp";
    
    [Required]
    public long TeamId { get; init; }
    
    [Required]
    public string ApiKey { get; init; } = null!;
}