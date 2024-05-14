using Refit;

namespace Webion.ClickUp.Api.V2.Spaces.Dtos;

public sealed class GetAllSpacesRequest
{
    [AliasAs("archived")]
    public bool Archived { get; init; }
}