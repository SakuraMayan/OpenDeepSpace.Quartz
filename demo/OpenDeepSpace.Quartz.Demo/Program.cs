using OpenDeepSpace.NetCore.Autofacastle.Extensions;
using OpenDeepSpace.Quartz.Demo.Jobs;
using Quartz;
using OpenDeepSpace.Quartz.Extensions;
using OpenDeepSpace.Quartz.MySql.Extensions;
using OpenDeepSpace.QuartzDashboard.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseAutofacastle();

//Quartz
builder.Services.AddIntegrationQuartz();
builder.Services.AddQuartzMySql();
//添加Quartz控制面板
builder.Services.AddQuartzDashboard();


//添加一个Quartz循环任务
builder.Services.AddQuartz(q =>
{
    q.ScheduleJob<FirstQuartzJob>(q =>
    {
        q.WithCronSchedule("0/7 * * * * ?");
    });

});

var app = builder.Build();

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
