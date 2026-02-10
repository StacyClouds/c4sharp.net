using StacyClouds.C4Sharp.Api;

namespace StacyClouds.C4Sharp.Examples
{

    /// <summary>
    /// This is an example of how to use an external theme.
    /// 
    /// The live workspace is available to view at https://structurizr.com/share/38898
    /// </summary>
    class Theme
    {

        private const long WorkspaceId = 38898;
        private const string ApiKey = "";
        private const string ApiSecret = "";

        static void Main()
        {
            Workspace workspace = new Workspace("Theme", "This is a model of my software system.");
            Model model = workspace.Model;

            Person user = model.AddPerson("User", "A user of my software system.");
            SoftwareSystem softwareSystem = model.AddSoftwareSystem("Software System", "My software system.");
            user.Uses(softwareSystem, "Uses");

            ViewSet viewSet = workspace.Views;
            SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "SystemContext", "An example of a System Context diagram.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // add a theme
            viewSet.Configuration.Theme = "https://raw.githubusercontent.com/StacyClouds/c4sharp.net/main/C4Sharp.Examples/Theme/theme.json";

            C4SharpClient structurizrClient = new C4SharpClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }

    }
}