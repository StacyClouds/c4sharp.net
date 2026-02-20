using Structurizr.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Structurizr.Documentation;

namespace Structurizr.Examples
{

    /// <summary>
    /// An empty software architecture document using the Structurizr template.
    /// 
    /// See https://structurizr.com/share/14181/documentation for the live version.
    /// </summary>
    class StructurizrDocumentationExample
        {

        private const long WorkspaceId = 14181;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        static void Main()
        {
            Workspace workspace = new Workspace("Documentation - Structurizr", "An empty software architecture document using the Structurizr template.");
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

            StructurizrDocumentationTemplate template = new StructurizrDocumentationTemplate(workspace);

            // this is the Markdown version
            DirectoryInfo documentationRoot = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "structurizr" + Path.DirectorySeparatorChar + "markdown");
            template.AddContextSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "01-context.md")));
            template.AddFunctionalOverviewSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "02-functional-overview.md")));
            template.AddQualityAttributesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "03-quality-attributes.md")));
            template.AddConstraintsSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "04-constraints.md")));
            template.AddPrinciplesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "05-principles.md")));
            template.AddSoftwareArchitectureSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "06-software-architecture.md")));
            template.AddDataSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "07-data.md")));
            template.AddInfrastructureArchitectureSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "08-infrastructure-architecture.md")));
            template.AddDeploymentSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "09-deployment.md")));
            template.AddDevelopmentEnvironmentSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "10-development-environment.md")));
            template.AddOperationAndSupportSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "11-operation-and-support.md")));
            template.AddDecisionLogSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "12-decision-log.md")));

            // this is the AsciiDoc version
//            DirectoryInfo documentationRoot = new DirectoryInfo("Documentation" + Path.DirectorySeparatorChar + "structurizr" + Path.DirectorySeparatorChar + "asciidoc");
//            template.AddContextSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "01-context.adoc")));
//            template.AddFunctionalOverviewSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "02-functional-overview.adoc")));
//            template.AddQualityAttributesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "03-quality-attributes.adoc")));
//            template.AddConstraintsSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "04-constraints.adoc")));
//            template.AddPrinciplesSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "05-principles.adoc")));
//            template.AddSoftwareArchitectureSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "06-software-architecture.adoc")));
//            template.AddDataSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "07-data.adoc")));
//            template.AddInfrastructureArchitectureSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "08-infrastructure-architecture.adoc")));
//            template.AddDeploymentSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "09-deployment.adoc")));
//            template.AddDevelopmentEnvironmentSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "10-development-environment.adoc")));
//            template.AddOperationAndSupportSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "11-operation-and-support.adoc")));
//            template.AddDecisionLogSection(softwareSystem, new FileInfo(Path.Combine(documentationRoot.FullName, "12-decision-log.adoc")));

            StructurizrClient structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }

    }
}
