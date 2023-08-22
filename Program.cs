var builder = WebApplication.CreateBuilder(args);


IronPdf.Logging.Logger.EnableDebugging = true;
IronPdf.Logging.Logger.LogFilePath = "ironpdf.log";
IronPdf.Logging.Logger.LoggingMode = IronPdf.Logging.Logger.LoggingModes.All;
IronPdf.Installation.AutomaticallyDownloadNativeBinaries = true;
IronPdf.Installation.Initialize();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


