using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using EquipIT.Front;
using EquipIT.Front.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Point to the EquipIT.Api backend (HTTPS so the browser allows the request)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7093") });
builder.Services.AddScoped<EquipmentApiService>();

builder.Services.AddLocalization();

var host = builder.Build();

// Read the saved culture from the browser and apply it before the app starts.
// Default to French, keep the user's choice only if it is a supported culture.
var js = host.Services.GetRequiredService<IJSRuntime>();

string? savedCulture = null;
try
{
    savedCulture = await js.InvokeAsync<string>("appCulture.get");
}
catch (JSException)
{
    // Local storage unavailable: fall back to the default culture
}

var cultureName = savedCulture is "fr-FR" or "en-US" ? savedCulture : "fr-FR";
var culture = new CultureInfo(cultureName);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();
