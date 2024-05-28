using System.ComponentModel.DataAnnotations;
using Webion.Stargaze.Core.Entities;

namespace Webion.Stargaze.Api.Controllers.Validation;

public sealed class ClickUpObjectIdAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not string s)
            return false;
        
        return ClickUpObjectId.TryDeserialize(s, out _);
    }
}