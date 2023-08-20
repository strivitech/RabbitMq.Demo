using Consumer.Factories;
using Consumer.Jobs;
using Consumer.Services;
using Consumer.Settings;
using Hangfire;
using Hangfire.PostgreSql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(cong => cong
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("HangfireConnection")));
        
builder.Services.AddHangfireServer();
builder.Services.AddSingleton<IRmqConnectionFactory, RmqConnectionFactory>();
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfiguration"));
builder.Services.AddTransient<IQueueConsumer, CreateUsersConsumer>();

var app = builder.Build();

app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<ConsumerHangfireJob>("ConsumerHangfireJob", x => x.DoAsync(), Cron.Minutely);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();