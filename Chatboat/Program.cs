using Chatboat.Infra;
using Chatboat.Models;
using Chatboat.ServiceRepository.EnquiryTypeRepository;
using Chatboat.ServiceRepository.LeadsRepository;
using Chatboat.ServiceRepository.MasterServicesRepository;
using Chatboat.ServiceRepository.PlatformoptionsRepository;
using Chatboat.ServiceRepository.ProjectQuotesRepository;
using Chatboat.ServiceRepository.ProjectQuotesRepository;
using Chatboat.ServiceRepository.ServicesRepository;
using Chatboat.ServiceRepository.SubServiceRepository;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using static Chatboat.Infra.RepositoryWrapper;
using Chatboat.ServiceRepository.SupportContactFlowRepository;
using Chatboat.ServiceRepository.TalkToExpertRepository;
using Chatboat.ServiceRepository.LeadMasterDataREpository;

var builder = WebApplication.CreateBuilder(args);

// Add Connectionstring for databse
builder.Services.AddDbContext<Datacontext>(options =>options.UseSqlServer(builder.Configuration.GetConnectionString("DataConnection")));

//for CROS_Po
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IServicesRepository,ServicesRepository>();
builder.Services.AddScoped<ILeadsRepository,LeadsRepository>();
builder.Services.AddScoped<IPlatformoptionsRepository,PlatformoptionsRepository>();
builder.Services.AddScoped<ISubServiceRepository,SubServiceRepository>();
builder.Services.AddScoped<IMasterServicesRepository,MasterServicesRepository>();
builder.Services.AddScoped<IProjectQuotesRepository, ProjectQuotesRepository>();
builder.Services.AddScoped<ISupportContactFlowRepository,SupportContactFlowRepository>();
builder.Services.AddScoped<ITalkToExpertRepository,TalkToExpertRepository>();
builder.Services.AddScoped<IEnquiryTypeRepository, EnquiryTypeRepository>();
builder.Services.AddScoped<ILeadMasterDataREpository,LeadMasterDataREpository>();





ConfigurationManager configuration = builder.Configuration; // new add allows both to access and to set up the config
IWebHostEnvironment environment = builder.Environment; // new add
var app = builder.Build();
//AppHttpContextAccessor.Configure(
//    app.Services.GetRequiredService<IHttpContextAccessor>(),
//    app.Services.GetRequiredService<IHostEnvironment>(),
//    app.Services.GetRequiredService<IWebHostEnvironment>(),
//    app.Services.GetRequiredService<IDataProtectionProvider>(),
//    app.Configuration,
//    app.Services.GetRequiredService<IHttpClientFactory>()
//);
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapGet("/health", () => Results.Ok("OK")); // new add simple liveness probe

app.MapControllers();

app.Run();
