using MassTransit;
using NewsLater.Reporting.Api.DataBase;
using NewsLater.Reporting.Api.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mongoConnectionString = builder.Configuration["DatabaseSettings:MongoConnectionString"];



using ILoggerFactory loggerFactory = LoggerFactory.Create(b => b.AddConsole());
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
logger.LogInformation("Program started...");

logger.LogInformation(mongoConnectionString);

builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddCors();

builder.Services.AddSingleton<ArticleService>();


builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<ArticleCreatedConsumer>();

    busConfigurator.AddConsumer<ArticleViewedEvent>();

    busConfigurator.UsingRabbitMq((context, configurtor) =>
    {
        configurtor.Host(new Uri(builder.Configuration["MessageBroker:Host"]!), h =>
        {
            h.Username(builder.Configuration["MessageBroker:Username"]!);
            h.Password(builder.Configuration["MessageBroker:Password"]!);
        });

        configurtor.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyOrigin()
                             .AllowAnyHeader()
                            .AllowAnyMethod());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
