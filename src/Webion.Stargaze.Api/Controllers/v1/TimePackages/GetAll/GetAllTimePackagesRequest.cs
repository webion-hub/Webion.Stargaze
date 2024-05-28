namespace Webion.Stargaze.Api.Controllers.v1.TimePackages.GetAll;

public sealed class GetAllTimePackagesRequest
{
    /// <summary>
    /// If included, will return only the time packages that
    /// belong to that company.
    /// </summary>
    public Guid? CompanyId { get; init; }
}