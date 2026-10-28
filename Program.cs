using EventsApi.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddSingleton<IEventService, EventService>();

var app = builder.Build();




app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
