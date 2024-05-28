using Refit;
using Webion.ClickUp.Api.V2.Spaces.Dtos;

namespace Webion.ClickUp.Api.V2.Spaces;

public interface IClickUpSpacesApi
{
    [Get("/v2/team/{teamId}/space")]
    Task<GetAllSpacesResponse> GetAllAsync(string teamId, [Query, AliasAs("archived")] bool? Archived = null);
}