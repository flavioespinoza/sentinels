namespace SolBotJudge;

/// <summary>The model behind the judge. Tests use a fake; no tools are ever offered.</summary>
public interface IModel
{
	string Name { get; }
	bool KeyPresent { get; }

	/// <summary>Returns the model's raw text reply. Throws when the model cannot be reached.</summary>
	Task<string> CompleteAsync(string system, string user, CancellationToken ct);
}

/// <summary>The model could not answer: no key, a refusal, or a bad reply envelope.</summary>
public sealed class ModelException(string message, int? status = null) : Exception(message)
{
	public int? Status { get; } = status;
}
