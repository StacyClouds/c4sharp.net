using System.Linq;
using Structurizr.Api;
using Structurizr.Documentation;

namespace Structurizr.Examples
{
    
    /// <summary>
    /// This workspace contains a number of diagrams for a fictional reseller of widgets online.
    /// 
    /// You can see the workspace online at https://structurizr.com/public/14471 
    /// </summary>
    public class WidgetsLimited
    {
        
        private const long WorkspaceId = 14471;
        private const string ApiKey = "key";
        private const string ApiSecret = "secret";

        private const string ExternalTag = "External";
        private const string InternalTag = "Internal";

        static void Main()
        {
            Workspace workspace = new Workspace("Widgets Limited", "Sells widgets to customers online.");
            Model model = workspace.Model;
            ViewSet views = workspace.Views;
            Styles styles = views.Configuration.Styles;
    
            model.Enterprise = new Enterprise("Widgets Limited");
    
            Person customer = model.AddPerson(Location.External, "Customer", "A customer of Widgets Limited.");
            Person customerServiceUser = model.AddPerson(Location.Internal, "Customer Service Agent", "Deals with customer enquiries.");
            SoftwareSystem ecommerceSystem = model.AddSoftwareSystem(Location.Internal, "E-commerce System", "Allows customers to buy widgets online via the widgets.com website.");
            SoftwareSystem fulfilmentSystem = model.AddSoftwareSystem(Location.Internal, "Fulfilment System", "Responsible for processing and shipping of customer orders.");
            SoftwareSystem taxamo = model.AddSoftwareSystem(Location.External, "Taxamo", "Calculates local tax (for EU B2B customers) and acts as a front-end for Braintree Payments.");
            taxamo.Url = "https://www.taxamo.com";
            SoftwareSystem braintreePayments = model.AddSoftwareSystem(Location.External, "Braintree Payments", "Processes credit card payments on behalf of Widgets Limited.");
            braintreePayments.Url = "https://www.braintreepayments.com";
            SoftwareSystem jerseyPost = model.AddSoftwareSystem(Location.External, "Jersey Post", "Calculates worldwide shipping costs for packages.");
    
            model.People.Where(p => p.Location == Location.External).ToList().ForEach(p => p.AddTags(ExternalTag));
            model.People.Where(p => p.Location == Location.Internal).ToList().ForEach(p => p.AddTags(InternalTag));
    
            model.SoftwareSystems.Where(ss => ss.Location == Location.External).ToList().ForEach(ss => ss.AddTags(ExternalTag));
            model.SoftwareSystems.Where(ss => ss.Location == Location.Internal).ToList().ForEach(ss => ss.AddTags(InternalTag));
    
            customer.InteractsWith(customerServiceUser, "Asks questions to", "Telephone");
            customerServiceUser.Uses(ecommerceSystem, "Looks up order information using");
            customer.Uses(ecommerceSystem, "Places orders for widgets using");
            ecommerceSystem.Uses(fulfilmentSystem, "Sends order information to");
            fulfilmentSystem.Uses(jerseyPost, "Gets shipping charges from");
            ecommerceSystem.Uses(taxamo, "Delegates credit card processing to");
            taxamo.Uses(braintreePayments, "Uses for credit card processing");
    
            SystemLandscapeView systemLandscapeView = views.CreateSystemLandscapeView("SystemLandscape", "The system landscape for Widgets Limited.");
            systemLandscapeView.AddAllElements();
    
            SystemContextView ecommerceSystemContext = views.CreateSystemContextView(ecommerceSystem, "EcommerceSystemContext", "The system context diagram for the Widgets Limited e-commerce system.");
            ecommerceSystemContext.AddNearestNeighbours(ecommerceSystem);
            ecommerceSystemContext.Remove(customer.GetEfferentRelationshipWith(customerServiceUser));
    
            SystemContextView fulfilmentSystemContext = views.CreateSystemContextView(fulfilmentSystem, "FulfilmentSystemContext", "The system context diagram for the Widgets Limited fulfilment system.");
            fulfilmentSystemContext.AddNearestNeighbours(fulfilmentSystem);
    
            DynamicView dynamicView = views.CreateDynamicView("CustomerSupportCall", "A high-level overview of the customer support call process.");
            dynamicView.Add(customer, customerServiceUser);
            dynamicView.Add(customerServiceUser, ecommerceSystem);
    
            StructurizrDocumentationTemplate template = new StructurizrDocumentationTemplate(workspace);
            template.AddSection("System Landscape", Format.Markdown, "Here is some information about the Widgets Limited system landscape... ![](embed:SystemLandscape)");
            template.AddContextSection(ecommerceSystem, Format.Markdown, "This is the context section for the E-commerce System... ![](embed:EcommerceSystemContext)");
            template.AddContextSection(fulfilmentSystem, Format.Markdown, "This is the context section for the Fulfilment System... ![](embed:FulfilmentSystemContext)");

            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle(Tags.Person) { Shape = Shape.Person });
    
            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff" });
            styles.Add(new ElementStyle(ExternalTag) { Background = "#EC5381", Border = Border.Dashed });
            styles.Add(new ElementStyle(InternalTag) { Background = "#B60037" });
    
            StructurizrClient structurizrClient = new StructurizrClient(ApiKey, ApiSecret);
            structurizrClient.PutWorkspace(WorkspaceId, workspace);
        }
        
    }
    
}