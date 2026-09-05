using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
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
		public void Main_ShowsUsage_WhenPackageVersionIsMissing()
		{
			ProcessResult result = RunBash($"'{GetScriptPath()}'");

			Assert.NotEqual(0, result.ExitCode);
			Assert.Contains("Usage: scripts/regenerate-release-api-docs.sh <package-version>", result.StandardError);
		}

		[Fact]
		public void ValidatePackageVersion_RejectsInvalidVersion()
		{
			ProcessResult result = RunBash($"source '{GetScriptPath()}'; validate_package_version 'release-1.2.3'");

			Assert.NotEqual(0, result.ExitCode);
			Assert.Contains("Invalid package version", result.StandardError);
		}

		[Fact]
		public void Main_RejectsUnsupportedDocfxTargetFramework()
		{
			using TestScriptWorkspace workspace = CreateTestScriptWorkspace();

			ProcessResult result = RunBash(
				$"DOCFX_TARGET_FRAMEWORK='net12.0' '{workspace.ScriptPath}' '1.2.3'",
				workspace.RootPath,
				new Dictionary<string, string> { { "PATH", $"{workspace.BinPath}:{Environment.GetEnvironmentVariable("PATH")}" } });

			Assert.NotEqual(0, result.ExitCode);
			Assert.Contains("DOCFX_TARGET_FRAMEWORK 'net12.0' is not supported by:", result.StandardError);
		}

		[Fact]
		public void Main_WritesReleaseNotes_WhenInvocationIsValid()
		{
			using TestScriptWorkspace workspace = CreateTestScriptWorkspace();

			ProcessResult result = RunBash(
				$"'{workspace.ScriptPath}' '1.2.3'",
				workspace.RootPath,
				new Dictionary<string, string> { { "PATH", $"{workspace.BinPath}:{Environment.GetEnvironmentVariable("PATH")}" } });

			Assert.Equal(0, result.ExitCode);

			string notesFile = Path.Combine(workspace.RootPath, "docs", "api", "release-notes-1.2.3.md");
			Assert.True(File.Exists(notesFile));
			Assert.Contains("Regenerated docs/api/ for net10.0", result.StandardOutput);

			string dotnetLog = File.ReadAllText(Path.Combine(workspace.RootPath, "dotnet.log"));
			Assert.Contains("tool restore", dotnetLog);
			Assert.Contains("restore StacyClouds.C4Sharp.slnx -p:TargetFramework=net10.0", dotnetLog);
			Assert.Contains("docfx metadata docfx.json --property TargetFramework=net10.0 --noRestore", dotnetLog);
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

		private static ProcessResult RunBash(string command, string? workingDirectory = null, IReadOnlyDictionary<string, string>? environmentVariables = null)
		{
			ProcessStartInfo startInfo = new ProcessStartInfo("bash")
			{
				RedirectStandardError = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				WorkingDirectory = workingDirectory ?? RepositoryRoot
			};

			startInfo.ArgumentList.Add("-lc");
			startInfo.ArgumentList.Add($"set -euo pipefail; {command}");
			if (environmentVariables is not null)
			{
				foreach ((string key, string value) in environmentVariables)
				{
					startInfo.Environment[key] = value;
				}
			}

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

		private static TestScriptWorkspace CreateTestScriptWorkspace()
		{
			string rootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
			string scriptsPath = Path.Combine(rootPath, "scripts");
			string docsApiPath = Path.Combine(rootPath, "docs", "api");
			string binPath = Path.Combine(rootPath, "bin");

			Directory.CreateDirectory(scriptsPath);
			Directory.CreateDirectory(docsApiPath);
			Directory.CreateDirectory(binPath);

			File.Copy(GetScriptPath(), Path.Combine(scriptsPath, "regenerate-release-api-docs.sh"));
			File.WriteAllText(Path.Combine(rootPath, "StacyClouds.C4Sharp.slnx"), "<Solution />");
			File.WriteAllText(Path.Combine(rootPath, "docfx.json"), "{}");

			foreach (string projectPath in new[]
			{
				"StacyClouds.C4Sharp.Core/StacyClouds.C4Sharp.Core.csproj",
				"StacyClouds.C4Sharp.Client/StacyClouds.C4Sharp.Client.csproj",
				"StacyClouds.C4Sharp.Renderer/StacyClouds.C4Sharp.Renderer.csproj",
				"StacyClouds.C4Sharp.Editor/StacyClouds.C4Sharp.Editor.csproj"
			})
			{
				string fullPath = Path.Combine(rootPath, projectPath);
				Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
				File.WriteAllText(fullPath, "<Project><PropertyGroup><TargetFrameworks>net8.0;net9.0;net10.0;net11.0</TargetFrameworks></PropertyGroup></Project>");
			}

			string dotnetScriptPath = Path.Combine(binPath, "dotnet");
			File.WriteAllText(dotnetScriptPath, GetFakeDotnetScript(rootPath));
			ProcessResult chmodResult = RunBash($"chmod +x '{Path.Combine(scriptsPath, "regenerate-release-api-docs.sh")}' '{dotnetScriptPath}'");
			Assert.Equal(0, chmodResult.ExitCode);

			return new TestScriptWorkspace(rootPath, Path.Combine(scriptsPath, "regenerate-release-api-docs.sh"), binPath);
		}

		private static string GetFakeDotnetScript(string rootPath)
		{
			string escapedRootPath = rootPath.Replace("\\", "\\\\");
			StringBuilder script = new StringBuilder();
			script.AppendLine("#!/usr/bin/env bash");
			script.AppendLine("set -euo pipefail");
			script.AppendLine($"printf '%s\\n' \"$*\" >> '{escapedRootPath}/dotnet.log'");
			script.AppendLine("exit 0");
			return script.ToString();
		}

		private sealed class TestScriptWorkspace : IDisposable
		{
			public TestScriptWorkspace(string rootPath, string scriptPath, string binPath)
			{
				RootPath = rootPath;
				ScriptPath = scriptPath;
				BinPath = binPath;
			}

			public string BinPath { get; }

			public string RootPath { get; }

			public string ScriptPath { get; }

			public void Dispose()
			{
				if (Directory.Exists(RootPath))
				{
					Directory.Delete(RootPath, true);
				}
			}
		}

		private sealed record ProcessResult(int ExitCode, string StandardOutput, string StandardError);
	}
}
