using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webion.Stargaze.Pgsql;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Payments.Delete;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/payments/{paymentId}")]
[Tags("Payments")]
[ApiVersion("1.0")]
public sealed class DeletePaymentController : ControllerBase
{
    private readonly StargazeDbContext _db;

    public DeletePaymentController(StargazeDbContext db)
    {
        _db = db;
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid paymentId,
        CancellationToken cancellationToken
    )
    {
        var deletedRows = await _db.Payments
            .Where(x => x.Id == paymentId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deletedRows <= 0)
            return NotFound();

        return NoContent();
    }
}