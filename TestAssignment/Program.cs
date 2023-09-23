using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestAssignment.Common.Config;
using TestAssignment.Common.Contexts;
using TestAssignment.Common.Entities;
using TestAssignment.Common.Interfaces;
using TestAssignment.Configuration;
using TestAssignment.Data;
using TestAssignment.Services;
using TestAssignment.Services.Providers;
using TestAssignment.Services.Services.Auth;
using TestAssignment.Services.Services.Data;
using TestAssignment.Services.Services.ElasticSearch;
using TestAssignment.Services.Services.Model;

// creating the builder
var builder = WebApplication.CreateBuilder(args);

// getting and registering elastic search / JWT config parameters from appSettings.json
var jwtSettings = builder.Configuration.GetSection("JwtConfiguration").Get<JWTSettings>();
builder.Services.AddSingleton(builder.Configuration.GetSection("ElasticsearchConfiguration").Get<ElasticSearchSettings>());
builder.Services.AddSingleton(jwtSettings);

// Register Elasticsearch client (using it in Singleton to initialize it one time only from the start)
builder.Services.AddSingleton<IElasticSearchClientProvider, ElasticSearchClientProvider>();
// Register the Auth Provider in transient State so that a new instance of this service is created every time it is requested.
builder.Services.AddTransient<IAuthProvider, AuthProvider>();

// Register other services and dependencies
// Register Elasticsearch service (using it in AddScoped so that a single instance of the service is created for the duration of the HTTP request)
builder.Services.AddScoped<IElasticSearchService, ElasticSearchService>();

// The rest of services are registered as transient in this case. Transient services are created each time they are requested from the service provider.
// This means that every time some part of the application requests service, a new instance of the service will be created and used.
// This is suitable for services that don't need to maintain state between calls.
builder.Services.AddTransient<MovieSeeder>();
builder.Services.AddTransient<UserSeeder>();
builder.Services.AddTransient<InteractionSeeder>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IMovieService, MovieService>();

// Add controllers
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add initial background service
// this service setup the datas the first time the server runs 
// Create Datas and ElasticSearch Index + ElasticSearch datas if needed
builder.Services.AddHostedService<InitialBackgroundService>();

// Adding Database Contexts
builder.Services.AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("TestAssignment")); });
builder.Services.AddDbContext<InteractionDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("TestAssignment")); });

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true, 
        ValidateAudience = true, 
        ValidateLifetime = true, 
        ValidateIssuerSigningKey = true, 
        ValidIssuer = jwtSettings.Issuer, 
        ValidAudience = jwtSettings.Audience, 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

