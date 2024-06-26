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
using Tapawingo_backend.Dtos;
using System.Security.Claims;

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
builder.Services.AddScoped<TeamroutepartsService>();
builder.Services.AddScoped<IRoutesRepository, RoutesRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IEventsRepository, EventsRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IRoutepartsRepository, RoutepartsRepository>();
builder.Services.AddScoped<IDestinationRepository, DestinationRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<ILocationlogsRepository, LocationlogsRepository>();
builder.Services.AddScoped<ITeamroutepartsRepository, TeamroutepartsRepository>();

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
    return HasClaim(context.User, "SuperAdmin");
}

bool CheckOrganizationManagerRequirement(AuthorizationHandlerContext context)
{
    if (TryGetRouteValue(context, "organisationId", out var orgId))
    {
        return HasClaim(context.User, $"{orgId}:OrganisationManager");
    }

    if (TryGetRouteValue(context, "eventId", out var evId))
    {
        var dbContext = GetDbContext(context);
        var organisationId = dbContext.Events.Where(e => e.Id == Convert.ToInt32(evId))
                                             .Select(e => e.OrganisationId)
                                             .FirstOrDefault();
        if (organisationId != 0 && HasClaim(context.User, $"{organisationId}:OrganisationManager"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "editionId", out var editionId) && IsEditionRoute(context))
    {
        var dbContext = GetDbContext(context);
        var eventOrganisationId = dbContext.Editions.Where(ed => ed.Id == Convert.ToInt32(editionId))
                                                    .Join(dbContext.Events,
                                                          ed => ed.EventId,
                                                          ev => ev.Id,
                                                          (ed, ev) => ev.OrganisationId)
                                                    .FirstOrDefault();

        if (eventOrganisationId != 0 && HasClaim(context.User, $"{eventOrganisationId}:OrganisationManager"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "routeId", out var routeId) && IsRoutePartsRoute(context))
    {
        var dbContext = GetDbContext(context);
        var route = dbContext.Routes.Where(rp => rp.Id == Convert.ToInt32(routeId)).FirstOrDefault();

        var OrganisationId = dbContext.Editions.Where(ed => ed.Id == Convert.ToInt32(route.EditionId))
                                                    .Join(dbContext.Events,
                                                          ed => ed.EventId,
                                                          ev => ev.Id,
                                                          (ed, ev) => ev.OrganisationId)
                                                    .FirstOrDefault();

        if (OrganisationId != 0 && HasClaim(context.User, $"{OrganisationId}:OrganisationManager"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "teamId", out var teamId) && IsTeamLocationLogsRoute(context))
    {
        var dbContext = GetDbContext(context);
        var editionIdOnTeamId = dbContext.Teams.Where(t => t.Id == Convert.ToInt32(teamId))
                                       .Select(t => t.EditionId)
                                       .FirstOrDefault();

        var OrganisationId = dbContext.Editions.Where(ed => ed.Id == Convert.ToInt32(editionIdOnTeamId))
                                                    .Join(dbContext.Events,
                                                          ed => ed.EventId,
                                                          ev => ev.Id,
                                                          (ed, ev) => ev.OrganisationId)
                                                    .FirstOrDefault();

        if (OrganisationId != 0 && HasClaim(context.User, $"{OrganisationId}:OrganisationManager"))
        {
            return true;
        }
    }

    return false;
}

bool CheckOrganizationUserRequirement(AuthorizationHandlerContext context)
{
    if (TryGetRouteValue(context, "organisationId", out var orgId) &&
        HasClaim(context.User, $"{orgId}:OrganisationUser"))
    {
        return true;
    }

    if (TryGetRouteValue(context, "eventId", out var evId))
    {
        var dbContext = GetDbContext(context);
        var organisationId = dbContext.Events.Where(e => e.Id == Convert.ToInt32(evId))
                                             .Select(e => e.OrganisationId)
                                             .FirstOrDefault();
        if (organisationId != 0 && HasClaim(context.User, $"{organisationId}:OrganisationUser"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "editionId", out var editionId) && IsEditionRoute(context))
    {
        var dbContext = GetDbContext(context);
        var eventOrganisationId = dbContext.Editions.Where(ed => ed.Id == Convert.ToInt32(editionId))
                                                    .Join(dbContext.Events,
                                                          ed => ed.EventId,
                                                          ev => ev.Id,
                                                          (ed, ev) => ev.OrganisationId)
                                                    .FirstOrDefault();

        if (eventOrganisationId != 0 && HasClaim(context.User, $"{eventOrganisationId}:OrganisationUser"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "routeId", out var routeId) && IsRoutePartsRoute(context))
    {
        var dbContext = GetDbContext(context);
        var route = dbContext.Routes.Where(rp => rp.Id == Convert.ToInt32(routeId)).FirstOrDefault();

        var OrganisationId = dbContext.Editions.Where(ed => ed.Id == Convert.ToInt32(route.EditionId))
                                                    .Join(dbContext.Events,
                                                          ed => ed.EventId,
                                                          ev => ev.Id,
                                                          (ed, ev) => ev.OrganisationId)
                                                    .FirstOrDefault();

        if (OrganisationId != 0 && HasClaim(context.User, $"{OrganisationId}:OrganisationUser"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "teamId", out var teamId) && IsTeamLocationLogsRoute(context))
    {
        var dbContext = GetDbContext(context);
        var editionIdOnTeamId = dbContext.Teams.Where(t => t.Id == Convert.ToInt32(teamId))
                                       .Select(t => t.EditionId)
                                       .FirstOrDefault();

        var OrganisationId = dbContext.Editions.Where(ed => ed.Id == Convert.ToInt32(editionIdOnTeamId))
                                                    .Join(dbContext.Events,
                                                          ed => ed.EventId,
                                                          ev => ev.Id,
                                                          (ed, ev) => ev.OrganisationId)
                                                    .FirstOrDefault();

        if (OrganisationId != 0 && HasClaim(context.User, $"{OrganisationId}:OrganisationUser"))
        {
            return true;
        }
    }

    return false;
}

bool CheckEventUserRequirement(AuthorizationHandlerContext context)
{
    if (TryGetRouteValue(context, "eventId", out var evId) &&
        HasClaim(context.User, $"{evId}:EventUser"))
    {
        return true;
    }

    if (TryGetRouteValue(context, "organisationId", out var orgId))
    {
        var dbContext = GetDbContext(context);
        var userEventIds = context.User.Claims.Where(c => c.Type == "Claim" && c.Value.Contains("EventUser"))
                                              .Select(c => Convert.ToInt32(c.Value.Split(':')[0]));

        foreach (var userEventId in userEventIds)
        {
            var eventOrgId = dbContext.Events.Where(e => e.Id == userEventId)
                                             .Select(e => e.OrganisationId)
                                             .FirstOrDefault();
            if (eventOrgId == Convert.ToInt32(orgId))
            {
                return true;
            }
        }
    }

    if (TryGetRouteValue(context, "editionId", out var editionId) && IsEditionRoute(context))
    {
        var dbContext = GetDbContext(context);
        var eventId = dbContext.Editions.Where(ed => ed.Id == Convert.ToInt32(editionId))
                                        .Select(ed => ed.EventId)
                                        .FirstOrDefault();

        if (eventId != 0 && HasClaim(context.User, $"{eventId}:EventUser"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "routeId", out var routeId) && IsRoutePartsRoute(context))
    {
        var dbContext = GetDbContext(context);
        var route = dbContext.Routes.Where(rp => rp.Id == Convert.ToInt32(routeId)).FirstOrDefault();

        var eventId = dbContext.Editions.Where(ed => ed.Id == route.EditionId)
                                        .Select(ed => ed.EventId)
                                        .FirstOrDefault();

        if (eventId != 0 && HasClaim(context.User, $"{eventId}:EventUser"))
        {
            return true;
        }
    }

    if (TryGetRouteValue(context, "teamId", out var teamId) && IsTeamLocationLogsRoute(context))
    {
        var dbContext = GetDbContext(context);
        var editionIdOnTeamId = dbContext.Teams.Where(t => t.Id == Convert.ToInt32(teamId))
                                       .Select(t => t.EditionId)
                                       .FirstOrDefault();

        var eventId = dbContext.Editions.Where(ed => ed.Id == editionIdOnTeamId)
                                        .Select(ed => ed.EventId)
                                        .FirstOrDefault();

        if (eventId != 0 && HasClaim(context.User, $"{eventId}:EventUser"))
        {
            return true;
        }
    }

    return false;
}

bool TryGetRouteValue(AuthorizationHandlerContext context, string key, out string value)
{
    value = null;
    if (context.Resource is HttpContext httpContext)
    {
        if (httpContext.Request.RouteValues.TryGetValue(key, out var routeValue))
        {
            value = routeValue.ToString();
            return true;
        }
    }
    return false;
}

bool HasClaim(ClaimsPrincipal user, string expectedClaim)
{
    return user.HasClaim(c => c.Type == "Claim" && c.Value.Equals(expectedClaim));
}

bool IsEditionRoute(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value;
        return path != null && path.Contains("/editions/") && (path.Contains("/routes") || path.Contains("/teams"));
    }
    return false;
}

bool IsRoutePartsRoute(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value;
        return path != null && path.Contains("/routes/") && path.Contains("/routeparts");
    }
    return false;
}

bool IsTeamLocationLogsRoute(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value;
        return path != null && path.Contains("/teams/") && path.Contains("/locationlogs");
    }
    return false;
}

DataContext GetDbContext(AuthorizationHandlerContext context)
{
    if (context.Resource is HttpContext httpContext)
    {
        return httpContext.RequestServices.GetRequiredService<DataContext>();
    }
    throw new InvalidOperationException("Unable to get DbContext from the current context.");
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
