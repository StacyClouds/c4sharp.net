using System;
using System.Diagnostics;
using System.IO;
using Xunit;

namespace StacyClouds.C4Sharp.Core.Tests.Documentation
{

	public class ReleaseApiDocsScriptTests
	{

		private static readonly string RepositoryRoot = FindRepositoryRoot();

		[Fact]
		public void ValidatePackageVersion_AcceptsBuildMetadata()
		{
			ProcessResult result = RunBash($"source '{GetScriptPath()}'; validate_package_version '1.2.3-rc.1+build.4'");

			Assert.Equal(0, result.ExitCode);
		}

		[Fact]
		public void ValidatePackageVersion_RejectsInvalidVersion()
		{
			ProcessResult result = RunBash($"source '{GetScriptPath()}'; validate_package_version 'release-1.2.3'");

			Assert.NotEqual(0, result.ExitCode);
			Assert.Contains("Invalid package version", result.StandardError);
		}

		[Fact]
		public void WriteReleaseNotes_WritesExpectedContent()
		{
			string tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(tempDirectory);

			try
			{
				string notesFile = Path.Combine(tempDirectory, "release-notes-1.2.3.md");
				ProcessResult result = RunBash($"source '{GetScriptPath()}'; write_release_notes '1.2.3' '{notesFile}' '2026-09-05'");

				Assert.Equal(0, result.ExitCode);
				Assert.True(File.Exists(notesFile));

				string content = File.ReadAllText(notesFile);
				Assert.Contains("title: Release 1.2.3", content);
				Assert.Contains("Released: 2026-09-05", content);
				Assert.Contains("https://www.nuget.org/packages/StacyClouds.C4Sharp.Core/1.2.3", content);
				Assert.Contains("https://www.nuget.org/packages/StacyClouds.C4Sharp.Editor/1.2.3", content);
				Assert.Contains("Browse the [API reference](index.html)", content);
			}
			finally
			{
				Directory.Delete(tempDirectory, true);
			}
		}

		private static string GetScriptPath()
		{
			return Path.Combine(RepositoryRoot, "scripts", "regenerate-release-api-docs.sh");
		}

		private static ProcessResult RunBash(string command)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo("bash")
			{
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				WorkingDirectory = RepositoryRoot
			};

			startInfo.ArgumentList.Add("-lc");
			startInfo.ArgumentList.Add($"set -euo pipefail; {command}");

			using Process process = Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start bash.");

			string standardOutput = process.StandardOutput.ReadToEnd();
			string standardError = process.StandardError.ReadToEnd();

			process.WaitForExit();

			return new ProcessResult(process.ExitCode, standardOutput, standardError);
		}

		private static string FindRepositoryRoot()
		{
			DirectoryInfo? directory = new DirectoryInfo(AppContext.BaseDirectory);

			while (directory is not null)
			{
				string candidate = Path.Combine(directory.FullName, "scripts", "regenerate-release-api-docs.sh");
				if (File.Exists(candidate))
				{
					return directory.FullName;
				}

				directory = directory.Parent;
			}

			throw new DirectoryNotFoundException("Repository root could not be located from the test output directory.");
		}

		private sealed record ProcessResult(int ExitCode, string StandardOutput, string StandardError);
	}
}
