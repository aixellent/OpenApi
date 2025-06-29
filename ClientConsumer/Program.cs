using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using ProjectManager.Simulator;
using Simulator;

var httpClient = new HttpClient();

var client = new ProjectManagerSimulatorClient("http://localhost:5147", httpClient);
var allEstimates = await client.GetAllEstimatesAsync(null);
foreach (var estimate in allEstimates)
{
    Console.WriteLine($"{estimate.EstimatedBy} - {estimate.EstimatedTimespan}");
}

var authProvider = new AnonymousAuthenticationProvider();
var adapter = new HttpClientRequestAdapter(authProvider);
adapter.BaseUrl = "http://localhost:5147";
var kiotaClient = new SimulatorClient(adapter);
var allKiotaEstimates = await kiotaClient.Api.Estimates.GetAsync();
var kiotaProjectIds = allKiotaEstimates.Select(x => x.ProjectId).Distinct();
foreach (var project in kiotaProjectIds)
{
    var deadline = await kiotaClient.Api.Deadlines.Calculate.PostAsync(new Simulator.Models.CalculateDeadlineRequest
    {
        Name = "New Deadline",
        ProjectId = project.HasValue ? project.Value : Guid.NewGuid()
    });
    Console.WriteLine($"Calculated Deadline {deadline.Name} at {deadline.DeadlineDate}");
}

Console.ReadKey();
