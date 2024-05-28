namespace Webion.ClickUp.Api.V2.Common;

public sealed class SharingDto
{
    public required bool Public { get; init; }
    public required bool? PublicShareExpiresOn { get; init; }
    public required IEnumerable<string> PublicFields { get; init; }
    public required object? Token { get; init; }
    public required bool SeoOptimized { get; init; }
}