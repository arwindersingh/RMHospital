using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HospitalAllocation.Model;
using HospitalAllocation.Providers.Allocation;
using HospitalAllocation.Providers.Allocation.Database;
using HospitalAllocation.Providers.Image.Interface;
using HospitalAllocation.Providers.Image.Database;
using HospitalAllocation.Providers.CSVFile.Interface;
using HospitalAllocation.Providers.CSVFile.Database;
using HospitalAllocation.Providers.Staff.Interface;
using HospitalAllocation.Providers.Staff.Database;
using HospitalAllocation.Providers.Skill.Interface;
using HospitalAllocation.Providers.Skill.Database;
using HospitalAllocation.Providers.Designation.Interface;
using HospitalAllocation.Providers.Designation.Database;
using HospitalAllocation.Providers.Note.Interface;
using HospitalAllocation.Providers.Note.Database;
using HospitalAllocation.Providers.Handover.Inteface;
using HospitalAllocation.Providers.Handover.Database;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;

namespace HospitalAllocation
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;

        public Startup(IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            _loggerFactory = loggerFactory;
            _loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            _loggerFactory.AddDebug();
            _loggerFactory.AddFile(Configuration.GetSection("FileLogging"));
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc().AddJsonOptions(options =>
            {
                // When we receive JSON message bodies, ones that are
                // malformed in some way just come through as null.
                // Adding this handler means we get a nice print out
                // describing what causes it to deserialise to null
                options.SerializerSettings.Error = (sender, e) =>
                {
                    Console.Error.WriteLine(e.ErrorContext.OriginalObject); 
                    Console.Error.WriteLine(e.ErrorContext.Error.StackTrace); 
                    Console.Error.WriteLine("-----ERROR-----");
                    Console.Error.WriteLine("Path: " + e.ErrorContext.Path);
                    Console.Error.WriteLine("Error causing member: " + e.ErrorContext.Member);
                    Console.Error.WriteLine("Message: " + e.ErrorContext.Error.Message);
                    Console.Error.WriteLine();
                };
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new Info { Title = "Hospital Allocation API", Version = "v2" });
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Build the database provider configuration
            var optionsBuilder = new DbContextOptionsBuilder<AllocationContext>();
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));

            DbContextOptions<AllocationContext> dbOptions = optionsBuilder.Options;
            using (var context = new AllocationContext(dbOptions))
            {
                context.Database.Migrate();
                context.EnsureSeedData();
            }

            // Add the provider for the allocation store
            var allocationProvider = new AllocationProvider(new AllocationDatabaseStore(dbOptions));
            services.AddSingleton(allocationProvider);

            // Add the image storage provider for the image API
            var imageProvider = new DbImageProvider(dbOptions);
            services.AddSingleton<IImageProvider>(imageProvider);

            var csvfileProvider = new DbCSVFileProvider(dbOptions);
            services.AddSingleton<ICSVFileProvider>(csvfileProvider);

            // Add the staff storage provider for the staff API
            var staffProvider = new DbStaffProvider(dbOptions);
            services.AddSingleton<IStaffProvider>(staffProvider);

            // Add the skill storage provider for the skill API
            var skillProvider = new DbSkillProvider(dbOptions);
            services.AddSingleton<ISkillProvider>(skillProvider);

            // Add the designation storage provider for the designation API
            var designationProvider = new DbDesignationProvider(dbOptions);
            services.AddSingleton<IDesignationProvider>(designationProvider);

            // Add the note storage provider for the note API
            var noteProvider = new DbNoteProvider(dbOptions);
            services.AddSingleton<INoteProvider>(noteProvider);

            // Add the handover provider for the handover API
            var handoverProvider = new DbHandoverProvider(dbOptions);
            services.AddSingleton<IHandoverProvider>(handoverProvider);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Hospital Allocation API V2");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
