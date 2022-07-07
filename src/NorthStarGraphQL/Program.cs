using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using NorthStarGraphQL.Backgrondservices;
using NorthStarGraphQL.Extention;
using NorthStarGraphQL.Interface;
using NorthStarGraphQL.Repository;
using NorthStarGraphQL.Services;
using NorthtarGraphQL.GraphQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// add notes schema
builder.Services.AddSingleton<ISchema, NorthStarSchema>(services => new NorthStarSchema(new SelfActivatingServiceProvider(services)));

builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

builder.Services.AddSingleton<IIdentityService, IdentityService>();
builder.Services.AddSingleton<IIdentityRepository, IdentityRepository>();

builder.Services.AddHostedService<KafkaMessagerService>();


// register graphQL
builder.Services.AddGraphQL(options =>
{
    options.EnableMetrics = true;
})
    .AddSystemTextJson();

builder.Services.AddClients(builder.Configuration);
// default setup
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "GraphQLNetExample", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GraphQLNetExample v1"));
    // add altair UI to development only
    app.UseGraphQLAltair();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// make sure all our schemas registered to route
app.UseGraphQL<ISchema>();

app.Run();