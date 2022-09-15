using Back.UnitOfWork;
using AutoMapper;
using Back.Infrastructure;
using Back.Interfaces;
using Back.Mapping;
using Back.Repositories;
using Back.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Back.Miscellaneous;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Back.Interfaces.Strategy;
using Back.LogStrategy;

namespace Back
{
	public class Startup
	{
		private readonly string _cors = "cors";
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			//swagger configuration
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Back", Version = "v1" });
				
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type=ReferenceType.SecurityScheme,
								Id="Bearer"
							}
						},
						new string[]{}
					}
				});
			});

			//adding service injections
			services.AddScoped<IPersonService, PersonService>();
			services.AddScoped<IFriendshipService, FriendshipService>();

			//add strategy injection
			services.AddScoped<IStrategyLogger, StrategyLogger>();

			//database related injections
			services.AddDbContext<DatabaseContext>(options => options.
				UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString")));
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IPersonRepository, PersonRepository>();
			services.AddScoped<IFriendshipRepository, FriendshipRepository>();
			services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();

			//adding mapper
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			IMapper mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);

			//authorization configuration
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = "http://localhost:5000",
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]))
				};
			});

			//adding cors
			services.AddCors(options =>
			{
				options.AddPolicy(name: _cors, builder => {
					builder.WithOrigins("http://localhost:3000")//Ovde navodimo koje sve aplikacije smeju kontaktirati nasu,u ovom slucaju nas Angular front
						   .AllowAnyHeader()
						   .AllowAnyMethod()
						   .AllowCredentials();
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Back v1"));
			}

			app.UseCors(_cors);

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
