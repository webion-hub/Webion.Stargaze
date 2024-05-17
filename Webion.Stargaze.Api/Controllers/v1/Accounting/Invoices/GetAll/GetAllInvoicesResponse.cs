using Webion.Stargaze.Api.Controllers.Dtos;

namespace Webion.Stargaze.Api.Controllers.v1.Accounting.Invoices.GetAll;

public sealed class GetAllInvoicesResponse : PaginatedResponse<InvoiceDto, GetAllInvoicesResponse>;