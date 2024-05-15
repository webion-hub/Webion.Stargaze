using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Webion.Stargaze.Api.Controllers.v1.ClickUp.Sync.Folders;

[ApiController]
[Authorize]
[Route("v{version:apiVersion}/clickup/sync/folders")]
[Tags("ClickUp Sync")]
[ApiVersion("1.0")]

public sealed class SyncClickUpFoldersController : ControllerBase
{
    
}