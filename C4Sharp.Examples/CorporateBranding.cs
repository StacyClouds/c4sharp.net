using System.IO;
using StacyClouds.C4Sharp.Api;
using StacyClouds.C4Sharp.Documentation;
using StacyClouds.C4Sharp.Util;

namespace StacyClouds.C4Sharp.Examples
{
    
    /// <summary>
    /// This is a simple example that illustrates the corporate branding features of Structurizr, including:
    ///  - A logo, which is included in diagrams and documentation.
    ///
    /// You can see the live workspace at https://structurizr.com/share/35031
    /// </summary>
    public class CorporateBranding
    {
     
        private const long WorkspaceId = 35031;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        static void Main()
        {
            Workspace workspace = new Workspace("Corporate Branding", "This is a model of my software system.");
            Model model = workspace.Model;
    
            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");
    
            ViewSet views = workspace.Views;
            SystemContextView contextView = views.CreateSystemContextView(softwareSystem, "SystemContext", "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();
    
            Styles styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Person) { Shape = Shape.Person });
    
            C4SharpDocumentationTemplate template = new C4SharpDocumentationTemplate(workspace);
            template.AddContextSection(softwareSystem, Format.Markdown, "Here is some context about the software system...\n\n![](embed:SystemContext)");
    
            Branding branding = views.Configuration.Branding;
            branding.Logo = ImageUtils.GetImageAsDataUri(new FileInfo("structurizr-logo.png"));
    
            C4SharpClient structurizrClient = new C4SharpClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }

    }
    
}