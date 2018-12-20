using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using GenFu;

namespace EFCoreConcurrentIssue
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(op => {
                // op.UseSqlite("Data Source=Test.db");
                // op.UseInMemoryDatabase("MemDb");
                // op.UseSqlServer("Server=localhost;Database=DemoDb;User Id=sa;Password=test@123456;");
                op.UseOracle("User Id=system;Password=oracle;Server=localhost:49161;Direct=True;Sid=xe;Port=49162;Persist Security Info=True;");
            });
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var sp = app.ApplicationServices.GetService<IServiceProvider>();
            var sc = sp.CreateScope();
            var db = sc.ServiceProvider.GetService<MyDbContext>();
            db.Database.EnsureCreated();
            Console.WriteLine("Init db ...");
            if (!db.Locations.Any())
            {
                var ls = A.ListOf<Location>(100);
                ls.ForEach(x => x.Id = 0);
                db.AddRange(ls);
                db.SaveChanges();
            }
            var lIds = db.Locations.Select(x => x.Id).ToList();
            if (!db.Events.Any())
            {
                var ls = A.ListOf<Event>(100);
                ls.ForEach(x => {
                    x.Id = 0;
                    x.LocationId = lIds[new Random().Next(0, 99)];
                });
                db.AddRange(ls);
                db.SaveChanges();
            }
            Console.WriteLine("Db inited");
            app.UseMvc();
        }
    }
}
