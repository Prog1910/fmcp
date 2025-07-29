namespace FMCP.Results;

public sealed record Error(string Code, string Description = "")
{
	public static Error None => new("");
}