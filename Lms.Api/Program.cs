
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Lms.Data.Data;
using Lms.Api.Extensions;
using Lms.Core.Repositories;
using Lms.Data.Repositories;
using Lms.Api.Services;

namespace Lms.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<LmsApiContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LmsApiContext") ?? throw new InvalidOperationException("Connection string 'LmsApiContext' not found.")));

            // Add services to the container.

            builder.Services.AddControllers(configure =>
            {
                configure.ReturnHttpNotAcceptable = true;
            })
                .AddNewtonsoftJson(setupAction =>
            {
                setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            })
                .AddXmlDataContractSerializerFormatters();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUoW, UoW>();

            builder.Services.AddAutoMapper(typeof(MapperProfile));

            //Add Transient Service for DataShaping fields Error Handling via Interface
            builder.Services.AddTransient<IPropertyCheckerService, PropertyCheckerService>();

            var app = builder.Build();

            await app.SeedDataAsync();

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
        }
    }
}