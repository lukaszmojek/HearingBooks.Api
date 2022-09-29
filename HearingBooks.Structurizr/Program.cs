using HearingBooks.Structurizr;
using Structurizr;
using Structurizr.Api;

Workspace workspace = new Workspace("Hearing Books", "Model of Hearing Books audiobook platform");
Model model = workspace.Model;

// Context - C1
Person customer = model.AddPerson(Location.External, "User", "A user of the Hearing Books platform");
SoftwareSystem cognitiveServices = model.AddSoftwareSystem(Location.External, "Azure Cognitive Services", "Performs synteses with the help of AI");
cognitiveServices.AddTags(CustomTags.ExternalSystem);
SoftwareSystem sendgrid = model.AddSoftwareSystem(Location.External, "SendGrid", "Provider for sending emails");
sendgrid.AddTags(CustomTags.ExternalSystem);
SoftwareSystem platform = model.AddSoftwareSystem(Location.Internal, "Platform", "Internal services making sure platform functions properly");

customer.Uses(platform, "Uses");
platform.Uses(cognitiveServices, "Sends requests/downloads synthesis files");
platform.Uses(sendgrid, "Sends emails using");
sendgrid.Delivers(customer, "Delivers emails to");

ViewSet viewSet = workspace.Views;
SystemContextView contextView = viewSet.CreateSystemContextView(platform, "HearingBooks context", "Context diagram of HearingBooks platform");

contextView.AddAllSoftwareSystems();
contextView.AddAllPeople();


// Container - C2
Container webApp = platform.AddContainer("WebApp", "Allows for creating and browsing syntheses as well as topping up account", "Angular 13");
Container api = platform.AddContainer("Api", "Entry point for all requests; gateway to all containers", ".NET 6");
Container mailing = platform.AddContainer("Mailing", "Creates requests for outgoing mails", ".NET 6");
Container synthesisProcessor = platform.AddContainer("SynthesisProcessor", "Processes syntheses requests depending on selected engine", ".NET 6");
Container liveNotifications = platform.AddContainer("LiveNotifications", "Sends notifications through SignalR the WebApp", ".NET 6");
Container database = platform.AddContainer("Database", "SQL database", "PostgreSQL");
database.AddTags(CustomTags.Database);
Container blobStorage = platform.AddContainer("Blob Storage", "Stores syntheses files for users");
Container rabbitMq = platform.AddContainer("RabbitMQ", "Ensures communication between services");

customer.Uses(webApp, "Uses");
webApp.Uses(api, "Fetches data/invokes actions requested by customer", "HTTP");

api.Uses(database, "Reads/writes", "EF Core");
api.Uses(blobStorage, "Downloads syntheses files");
api.Uses(rabbitMq, "Publishes messages to handle work in other services", "MassTransit");

rabbitMq.Uses(mailing, "Messages about mails to be sent", "AMQP");
rabbitMq.Uses(liveNotifications, "Messages about updates to send to the WebApp", "AMQP");
rabbitMq.Uses(synthesisProcessor, "Messages about synthesis requests", "AMQP");

synthesisProcessor.Uses(rabbitMq, "Publishes messages about syntheses updates", "MassTransit");
synthesisProcessor.Uses(database, "Saves data about processed syntheses", "EF Core");
synthesisProcessor.Uses(blobStorage, "Uploads syntheses files");
synthesisProcessor.Uses(cognitiveServices, "Sends syntheses requests");
    
liveNotifications.Uses(webApp, "Delivers updates about syntheses of a user");

mailing.Uses(sendgrid, "Sends emails using", "");

ContainerView containerView = viewSet.CreateContainerView(platform, "Containers", "containers desc");
containerView.Add(customer);
containerView.AddAllContainers();
containerView.Add(sendgrid);
containerView.Add(cognitiveServices);

// Component - C3


// Styling for every abstraction level
Styles styles = viewSet.Configuration.Styles;
styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
styles.Add(new ElementStyle(CustomTags.ExternalSystem) { Background = "#989898", Color = "#ffffff" });
styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
styles.Add(new ElementStyle(Tags.Container) { Background = "#448dd5", Color = "#ffffff" });
styles.Add(new ElementStyle(CustomTags.Database) { Shape = Shape.Cylinder });


StructurizrClient structurizrClient = new StructurizrClient(Secrets.ApiKey, Secrets.ApiSecret);
structurizrClient.PutWorkspace(76919, workspace);
