using System.Text.Json;

namespace SolBotJudge;

/// <summary>One engine log line: unix ms, level, and the text.</summary>
public sealed record LogLine(long At, string Level, string Line);

/// <summary>
/// What the sentinel sends. The alert, snapshot, and trend are passed through as data and are
/// never interpreted here.
/// </summary>
public sealed record JudgeRequest(
	JsonElement Alert,
	JsonElement? Snapshot,
	JsonElement? Trend,
	List<LogLine>? LogLines);

public sealed record EvidenceLine(string At, string Line);

/// <summary>The only thing the judge answers. It explains; it never acts.</summary>
public sealed record JudgeVerdict(
	string Verdict,
	string Cause,
	List<EvidenceLine> Evidence,
	string Action,
	double Confidence)
{
	/// <summary>What replaces any answer that fails the schema.</summary>
	public static JudgeVerdict Rejected(string why) =>
		new("unknown", $"judge output rejected: {why}", [], "person", 0);
}
