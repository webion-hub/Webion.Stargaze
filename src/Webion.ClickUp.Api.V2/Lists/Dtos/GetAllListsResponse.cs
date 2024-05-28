using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.Lists.Dtos;

public sealed class GetAllListsResponse
{
    public required IEnumerable<List4Dto> Lists { get; init; }
}