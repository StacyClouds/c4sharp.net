using System.Collections.Generic;
using System.IO;
using StacyClouds.C4Sharp.Renderer;

namespace StacyClouds.C4Sharp.Examples
{
    /// <summary>Shows how to create standalone SVG documents from workspace views.</summary>
    public static class SvgRenderingExample
    {
        public static IReadOnlyDictionary<string, string> CreateSvgDocuments()
        {
            Workspace workspace = new Workspace("SVG rendering", "A workspace rendered locally as SVG.");
            Person user = workspace.Model.AddPerson("User", "Uses the system.");
            SoftwareSystem system = workspace.Model.AddSoftwareSystem("System", "Provides a service.");
            user.Uses(system, "Uses");

            SystemContextView view = workspace.Views.CreateSystemContextView(system, "system-context", "System context");
            view.AddAllPeople();
            view.AddAllSoftwareSystems();

            return new SvgWorkspaceRenderer().Render(workspace);
        }

        public static void WriteSvgDocuments(string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory);
            foreach (KeyValuePair<string, string> diagram in CreateSvgDocuments())
            {
                File.WriteAllText(Path.Combine(outputDirectory, diagram.Key + ".svg"), diagram.Value);
            }
        }
    }
}
