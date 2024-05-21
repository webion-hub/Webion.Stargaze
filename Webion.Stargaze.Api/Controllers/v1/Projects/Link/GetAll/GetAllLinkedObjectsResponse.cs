using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Link.ClickUp;

public sealed class GetAllLinkedObjectsResponse
{
    public required IEnumerable<LinkedObjectDto> LinkedObjects { get; init; }
}