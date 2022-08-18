using GeneratorWeb.Extention;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddClients(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();   
});

app.Run();
