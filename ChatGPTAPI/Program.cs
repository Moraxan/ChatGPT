using dotenv.net;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddTransient<IChatGPTService, ChatGPTService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowFrontEnd",
                      builder =>
                      {
                          builder.WithOrigins("https://localhost:5173") // Replace with your frontend port
                                 .AllowAnyMethod()
                                 .AllowAnyHeader();
                      });
});

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseCors("AllowFrontEnd"); // Apply the CORS policy
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Referrer-Policy", "no-referrer-when-downgrade");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
