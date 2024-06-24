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
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<EventsService>();
builder.Services.AddScoped<OrganisationsService>();
builder.Services.AddScoped<EventsService>();
builder.Services.AddScoped<EditionsService>();
builder.Services.AddScoped<IEditionsRepository, EditionsRepository>();
builder.Services.AddScoped<IOrganisationsRepository, OrganisationsRepository>();
builder.Services.AddScoped<EventsService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<RoutesService>();
builder.Services.AddScoped<RoutepartsService>();
builder.Services.AddScoped<LocationlogsService>();
builder.Services.AddScoped<IRoutesRepository, RoutesRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IRoutepartsRepository, RoutepartsRepository>();
builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<ILocationlogsRepository, LocationlogsRepository>();

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
        policy.RequireAssertion(context =>
            CheckSuperAdminRequirement(context)));

    options.AddPolicy("SuperAdminOrOrganisationMPolicy", policy =>
        policy.RequireAssertion(context =>
            CheckSuperAdminRequirement(context) ||
            CheckOrganizationManagerRequirement(context)));

    options.AddPolicy("SuperAdminOrOrganisationMOrUPolicy", policy =>
    policy.RequireAssertion(context =>
        CheckSuperAdminRequirement(context) ||
        CheckOrganizationManagerRequirement(context) ||
        CheckOrganizationUserRequirement(context)));

    options.AddPolicy("SuperAdminOrOrganisationMOrUOrEventUserPolicy", policy =>
    policy.RequireAssertion(context =>
        CheckSuperAdminRequirement(context) ||
        CheckOrganizationManagerRequirement(context) ||
        CheckOrganizationUserRequirement(context) ||
        CheckEventUserRequirement(context)));
});

bool CheckSuperAdminRequirement(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        var expectedClaim = $"SuperAdmin";
        var superAdminClaim = context.User.FindFirst(c => c.Type == "Claim" && c.Value.Equals(expectedClaim));
        if (superAdminClaim != null)
        {
            return true;
        }
    }
    return false;
}

bool CheckOrganizationManagerRequirement(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        if (httpContext.Request.RouteValues.TryGetValue("organisationId", out var orgId))
        {
            var expectedClaim = $"{orgId}:OrganisationManager";
            var organizationClaim = context.User.FindFirst(c => c.Type == "Claim" && c.Value.Equals(expectedClaim));
            if (organizationClaim != null)
            {
                return true;
            }
        }
    }
    return false;
}

bool CheckOrganizationUserRequirement(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        if (httpContext.Request.RouteValues.TryGetValue("organisationId", out var orgId))
        {
            var expectedClaim = $"{orgId}:OrganisationUser";
            var organizationClaim = context.User.FindFirst(c => c.Type == "Claim" && c.Value.Equals(expectedClaim));
            if (organizationClaim != null)
            {
                return true;
            }
        }

        if (httpContext.Request.RouteValues.TryGetValue("eventId", out var evId))
        {
            var DbContext = httpContext.RequestServices.GetRequiredService<DataContext>();
            var organisationId = DbContext.Events.Where(e => e.Id.Equals(evId)).Select(e => e.OrganisationId).FirstOrDefault();
            var expectedClaim = $"{organisationId}:OrganisationUser";
            var organizationClaim = context.User.FindFirst(c => c.Type == "Claim" && c.Value.Equals(expectedClaim));
            if (organizationClaim != null)
            {
                return true;
            }
        }
    }
    return false;
}

bool CheckEventUserRequirement(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        if (httpContext.Request.RouteValues.TryGetValue("eventId", out var evId))
        {
            var expectedClaim = $"{evId}:EventUser";
            var organizationClaim = context.User.FindFirst(c => c.Type == "Claim" && c.Value.Equals(expectedClaim));
            if (organizationClaim != null)
            {
                return true;
            }
        }
        if (httpContext.Request.RouteValues.TryGetValue("organisationId", out var orgId))
        {
            httpContext.Request.RouteValues.TryGetValue("eventId", out var endpointEventId);
            var endpointEventIdInt = Convert.ToInt32(endpointEventId);
            var eventClaim = context.User.FindFirst(c => c.Type == "Claim");
            var eventId = eventClaim.Value.Split(":")[0];
            var DbContext = httpContext.RequestServices.GetRequiredService<DataContext>();
            var eventIdInt = Convert.ToInt32(eventId);
            var eventObject = DbContext.Events.Where(e => e.Id == eventIdInt).FirstOrDefault();
            var organisationIdInt = Convert.ToInt32(orgId);
            if (eventObject.OrganisationId.Equals(organisationIdInt) && eventIdInt == endpointEventIdInt)
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
