using HearingBooks.Structurizr;
using Structurizr;
using Structurizr.Api;

Workspace workspace = new Workspace("Hearing Books", "Model of Hearing Books audiobook platform");
Model model = workspace.Model;

// Context - C1
Person user = model.AddPerson(Location.External, "User", "A user of the Hearing Books platform");
SoftwareSystem cognitiveServices = model.AddSoftwareSystem(Location.External, "Azure Cognitive Services", "Performs synteses with the help of AI");
SoftwareSystem sendgrid = model.AddSoftwareSystem(Location.External, "SendGrid", "Provider for sending emails");

SoftwareSystem webApp = model.AddSoftwareSystem(Location.Internal, "HearingBooks online", "Allows for creating and browsing syntheses as well as topping up account");
SoftwareSystem platform = model.AddSoftwareSystem(Location.Internal, "Platform", "Entry point for all requests; gateway to all containers");

user.Uses(webApp, "Uses");
webApp.Uses(platform, "Fetches data/performs actions requested by the user");
platform.Uses(cognitiveServices, "Sends requests/downloads synthesis files");
platform.Uses(sendgrid, "Sends emails using");
sendgrid.Delivers(user, "Delivers emails to");

ViewSet viewSet = workspace.Views;
SystemContextView contextView = viewSet.CreateSystemContextView(platform, "HearingBooks context", "Context diagram of HearingBooks platform");

contextView.AddAllSoftwareSystems();
contextView.AddAllPeople();

// Container - C2
SoftwareSystem api = model.AddSoftwareSystem(Location.Internal, "Platform", "Entry point for all requests; gateway to all containers");

// Component - C3

Styles styles = viewSet.Configuration.Styles;
styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });

StructurizrClient structurizrClient = new StructurizrClient(Secrets.ApiKey, Secrets.ApiSecret);
structurizrClient.PutWorkspace(76919, workspace);

// Person user = model.AddPerson(Location.External, "User", "A user of the Hearing Books platform");
//
// SoftwareSystem webApp = model.AddSoftwareSystem(Location.Internal, "Browser Client", "Web App written in Angular");
//
// user.Uses(webApp, "Uses");
//
// SoftwareSystem api = model.AddSoftwareSystem(Location.Internal, "Api", "Entry point for all requests; gateway to all containers");
//
// webApp.Uses(api, "Fetches data/performs actions requested by the user");