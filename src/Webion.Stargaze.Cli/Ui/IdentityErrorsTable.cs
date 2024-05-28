using Microsoft.AspNetCore.Identity;
using Spectre.Console;

namespace Webion.Stargaze.Cli.Ui;

public static class IdentityErrorsTable
{
    public static Table From(IEnumerable<IdentityError> errors)
    {
        var table = new Table();
        table.Title("Error");
        table.Border(TableBorder.Minimal);
        table.AddColumns("Code", "Message");
        foreach (var e in errors)
            table.AddRow(e.Code, e.Description);

        return table;
    }
}