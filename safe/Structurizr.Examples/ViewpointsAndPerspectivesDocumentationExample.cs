using System.IO;
using Structurizr.Api;
using Structurizr.Documentation;

namespace Structurizr.Examples
{
    
    /// <summary>
    /// An empty software architecture document using the "Viewpoints and Perspectives" template.
    /// 
    /// See https://structurizr.com/share/36371/documentation for the live version.
    /// </summary>
    public class ViewpointsAndPerspectivesDocumentationExample
    {
        
        private const long WorkspaceId = 36371;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        static void Main()
        {
            Workspace workspace = new Workspace("Documentation - Viewpoints and Perspectives", "An empty software architecture document using the Viewpoints and Perspectives template.");
            Model model = workspace.Model;
            ViewSet views = workspace.Views;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            SystemContextView contextView = views.CreateSystemContextView(softwareSystem, "SystemContext", "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Person) { Shape = Shape.Person });

            ViewpointsAndPerspectivesDocumentation template = new ViewpointsAndPerspectivesDocumentation(workspace);

            // this is the Markdown version
            DirectoryInfo documentationRoot = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "viewpointsandperspectives" + Path.DirectorySeparatorChar + "markdown");
            template.AddIntroductionSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "01-introduction.md")));
            template.AddGlossarySection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "02-glossary.md")));
            template.AddSystemStakeholdersAndRequirementsSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "03-system-stakeholders-and-requirements.md")));
            template.AddArchitecturalForcesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "04-architectural-forces.md")));
            template.AddArchitecturalViewsSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "05-architectural-views")));
            template.AddSystemQualitiesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "06-system-qualities.md")));
            template.AddAppendicesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "07-appendices.md")));

            // this is the AsciiDoc version
//            DirectoryInfo documentationRoot = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "viewpointsandperspectives" + Path.DirectorySeparatorChar + "asciidoc");
//            template.AddIntroductionSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "01-introduction.adoc")));
//            template.AddGlossarySection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "02-glossary.adoc")));
//            template.AddSystemStakeholdersAndRequirementsSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "03-system-stakeholders-and-requirements.adoc")));
//            template.AddArchitecturalForcesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "04-architectural-forces.adoc")));
//            template.AddArchitecturalViewsSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "05-architectural-views")));
//            template.AddSystemQualitiesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "06-system-qualities.adoc")));
//            template.AddAppendicesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "07-appendices.adoc")));

            StructurizrClient structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
        
    }
    
}