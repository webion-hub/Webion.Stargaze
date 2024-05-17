using Refit;
using Webion.ClickUp.Api.V2.Folders.Dtos;

namespace Webion.ClickUp.Api.V2.Folders;

public interface IClickUpFoldersApi
{
    [Get("/v2/space/{space_id}/folder")]
    Task<GetAllFoldersResponse> GetAllAsync([AliasAs("space_id")] long spaceId, [Query, AliasAs("archived")] bool? Archived);
}