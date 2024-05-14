using Blazored.LocalStorage;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using WebEditor.Components.DragAndDrop;
using WebEditor.Tools;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<WebEditor.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(ToolManager.Shared);
builder.Services.AddSingleton(DragAndDropManager.Shared);
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazorDownloadFile();

await builder.Build().RunAsync();