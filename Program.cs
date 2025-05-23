using ChatApp.Data;
using ChatApp.Hubs;
using ChatApp.Services;
using Microsoft.Azure.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllersWithViews();

// connecting to azure signalR service via connection string
builder.Services.AddSignalR().AddAzureSignalR(options =>
{
    options.ConnectionString = builder.Configuration["Azure:SignalR:ConnectionString"];
});

// connecting to azure sql database
builder.Services.AddDbContext<ChatContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// adding sentiment analysis service
builder.Services.AddSingleton(new SentimentAnalysisService(
    builder.Configuration["Azure:CognitiveServices:Endpoint"]!,
    builder.Configuration["Azure:CognitiveServices:ApiKey"]!
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDeveloperExceptionPage();
app.UseRouting();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapHub<ChatHub>("/chatHub");

app.Run();
