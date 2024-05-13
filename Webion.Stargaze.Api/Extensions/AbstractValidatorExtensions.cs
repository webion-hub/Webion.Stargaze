using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Webion.Stargaze.Api.Extensions;

public static class AbstractValidatorExtensions
{
    public static async Task ValidateModelAsync<T>(this AbstractValidator<T> validator,
        T model,
        ModelStateDictionary modelState,
        CancellationToken cancellationToken = default
    )
    {
        var result = await validator.ValidateAsync(model, cancellationToken);
        foreach (var e in result.Errors)
            modelState.AddModelError(e.ErrorCode, e.ErrorMessage);
    }
}