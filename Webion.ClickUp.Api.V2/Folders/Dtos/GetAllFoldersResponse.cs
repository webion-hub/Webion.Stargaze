using Webion.ClickUp.Api.V2.Common;

namespace Webion.ClickUp.Api.V2.Folders.Dtos;

public sealed class GetAllFoldersResponse
{
    public IEnumerable<Folders5Dto> Folders { get; init; } = [];
}