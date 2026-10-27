using SolBotJudge;

// Not top-level statements: a generated Program type would clash with the sentinel's Program in
// the test project that references both.
internal static class Entry
{
	// Listens where ASPNETCORE_URLS says; the default is loopback, because only the sentinel calls it.
	private static void Main(string[] args) =>
		JudgeApp.Create(args, b =>
			b.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://127.0.0.1:8096")).Run();
}
