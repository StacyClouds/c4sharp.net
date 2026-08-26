using System;
using System.IO;
using Xunit;

namespace StacyClouds.C4Sharp.Api.Tests.Util
{
    public class WorkspaceUtilsTests
    {
        [Fact]
        public void Load_and_save_workspace_round_trip_through_filesystem()
        {
            Workspace workspace = new Workspace("Name", "Description");
            workspace.Model.AddPerson("User");

            string directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);

            try
            {
                FileInfo file = new FileInfo(Path.Combine(directory, "workspace.json"));
                WorkspaceUtils.SaveWorkspaceToJson(workspace, file);
                Workspace loaded = WorkspaceUtils.LoadWorkspaceFromJson(file);

                Assert.True(file.Exists);
                Assert.Equal("Name", loaded.Name);
                Assert.Equal("Description", loaded.Description);
                Assert.Single(loaded.Model.People);
            }
            finally
            {
                if (Directory.Exists(directory))
                {
                    Directory.Delete(directory, true);
                }
            }
        }

        [Fact]
        public void LoadWorkspaceFromJson_throws_when_file_is_null()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => WorkspaceUtils.LoadWorkspaceFromJson(null));
            Assert.Equal("The path to a JSON file must be specified.", exception.Message);
        }

        [Fact]
        public void LoadWorkspaceFromJson_throws_when_file_does_not_exist()
        {
            FileInfo missingFile = new FileInfo(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "missing.json"));
            ArgumentException exception = Assert.Throws<ArgumentException>(() => WorkspaceUtils.LoadWorkspaceFromJson(missingFile));
            Assert.Equal("The specified JSON file does not exist.", exception.Message);
        }

        [Fact]
        public void SaveWorkspaceToJson_throws_when_workspace_is_null()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => WorkspaceUtils.SaveWorkspaceToJson(null, new FileInfo("workspace.json")));
            Assert.Equal("A workspace must be provided.", exception.Message);
        }

        [Fact]
        public void SaveWorkspaceToJson_throws_when_file_is_null()
        {
            Workspace workspace = new Workspace("Name", "Description");
            ArgumentException exception = Assert.Throws<ArgumentException>(() => WorkspaceUtils.SaveWorkspaceToJson(workspace, null));
            Assert.Equal("The path to a JSON file must be specified.", exception.Message);
        }

        [Fact]
        public void PrintWorkspaceAsJson_writes_output_to_console()
        {
            Workspace workspace = new Workspace("Name", "Description");
            StringWriter output = new StringWriter();
            TextWriter originalOutput = Console.Out;

            try
            {
                Console.SetOut(output);
                WorkspaceUtils.PrintWorkspaceAsJson(workspace);
            }
            finally
            {
                Console.SetOut(originalOutput);
            }

            Assert.Contains("\"name\": \"Name\"", output.ToString());
        }
    }
}
