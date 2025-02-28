using Wörterbuch;
using Wörterbuch.AppData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

App.Instance._WebHostEnvironment = app.Services.GetRequiredService<IWebHostEnvironment>();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapDefaultControllerRoute().WithStaticAssets();

app.Run();
