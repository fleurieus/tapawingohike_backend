using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using Tapawingo_backend;
using Tapawingo_backend.Data;
using Tapawingo_backend.Interface;
using Tapawingo_backend.Middleware;
using Tapawingo_backend.Models;
using Tapawingo_backend.Repository;
using Tapawingo_backend.Services;
using Tapawingo_backend.Helper;
using static Tapawingo_backend.Helper.CustomAuthRequirements;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<EventsService>();
builder.Services.AddScoped<OrganisationsService>();
builder.Services.AddScoped<EventsService>();
builder.Services.AddScoped<EditionsService>();
builder.Services.AddScoped<IEditionsRepository, EditionsRepository>();
builder.Services.AddScoped<IOrganisationsRepository, OrganisationsRepository>();
builder.Services.AddScoped<EventsService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<RoutesService>();
builder.Services.AddScoped<IRoutesRepository, RoutesRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();

// Add database connection
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string is not found"));
});

// Add Identity & JWT authentication
// Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<DataContext>()
  .AddDefaultTokenProviders();

// Add authorization to Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter 'Bearer [jwt]'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityRequirement(new OpenApiSecurityRequirement { { scheme, Array.Empty<string>() } });
});

// TODO the logger may need to be changed to also logging the methods and function used
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var secret = builder.Configuration["JWT:Secret"] ?? throw new InvalidOperationException("Secret not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        // ClockSkew = new TimeSpan(0, 0, 5) This is only needed if the server and client are not in sync
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminPolicy", policy =>
        policy.RequireClaim("SuperAdminRole"));

    options.AddPolicy("OrganisationPolicy", policy =>
        policy.Requirements.Add(new CustomAuthRequirements.OrganizationRequirement(string.Empty)));


    options.AddPolicy("SuperAdminOrOrganisationPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == "SuperAdminRole") ||
            CheckOrganizationRequirement(context)));
});

// Method to check OrganizationRequirement
bool CheckOrganizationRequirement(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        if (httpContext.Request.RouteValues.TryGetValue("organisationId", out var orgId))
        {
            var expectedClaim = $"{orgId}:OrganisationManager";
            var organizationClaim = context.User.FindFirst(c => c.Type == "OrganisationRole" && c.Value.Equals(expectedClaim));
            if (organizationClaim != null)
            {
                return true;
            }
        }
    }
    return false;
}

builder.Services.AddSingleton<IAuthorizationHandler, OrganizationHandler>();

builder.Services.AddRequestTimeouts();

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ControllerLogging>();
app.UseMiddleware<ServiceLogging>();
app.UseMiddleware<RepositoryLogging>();
app.UseMiddleware(typeof(GlobalErrorHandling));

app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    await RoleInitializer.InitializeAsync(roleManager);
//}


// Add SuperAdmin user to the database
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    await AdminInitializer.InitializeAsync(roleManager, userManager);
}

app.Run();
