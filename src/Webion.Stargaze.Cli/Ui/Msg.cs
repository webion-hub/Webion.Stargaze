namespace Webion.Stargaze.Cli.Ui;

public static class Msg
{
    public static string Ask(string question)
    {
        return $"{Icons.Ask} [b]{question}[/]";
    }
    
    public static string Warn(string warning)
    {
        return $"{Icons.Warn} [yellow](╬ Ò﹏Ó)[/] [b]{warning}[/]";
    }
    
    public static string Err(string error)
    {
        return $"{Icons.Err} [red](ﾉಥ益ಥ)ﾉ[/] [b]{error}[/]";
    }

    public static string Ok(string message)
    {
        return $"{Icons.Ok} {message}";
    }
}