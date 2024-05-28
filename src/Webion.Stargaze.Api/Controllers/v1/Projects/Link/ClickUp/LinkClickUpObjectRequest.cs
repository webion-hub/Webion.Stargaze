using Webion.Stargaze.Core.Entities;

namespace Webion.Stargaze.Api.Controllers.v1.Projects.Link.ClickUp;

public sealed class LinkClickUpObjectRequest
{
    public List<ClickUpObjectId> ClickUpObjectIds { get; init; } = [];
}