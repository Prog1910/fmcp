using System.Diagnostics.CodeAnalysis;

namespace FMCP.Results;

public sealed class Result<TValue>
{
	[MemberNotNullWhen(true, nameof(Value))]
	[MemberNotNullWhen(false, nameof(Error))]
	public bool IsSuccess { get; init; }

	public Error? Error { get; init; }
	public TValue? Value { get; init; }

	private Result(Error error, TValue? value = default)
	{
		IsSuccess = error == Error.None;
		Error = error;
		Value = value;
	}

	public static Result<TValue> Success(TValue value) => new(Error.None, value);

	public static Result<TValue> Failure(Error error) => new(error);

	public static implicit operator Result<TValue>(TValue value) => Success(value);

	public static implicit operator Result<TValue>(Error error) => Failure(error);
}