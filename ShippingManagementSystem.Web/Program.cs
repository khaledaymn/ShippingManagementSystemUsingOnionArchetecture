#region Using Directives

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShippingManagementSystem.Application.Exptions;
using ShippingManagementSystem.Application.Filters;
using ShippingManagementSystem.Application.Helper;
using ShippingManagementSystem.Application.Services.ExternalServices.EmailServices;
using ShippingManagementSystem.Application.Settings;
using ShippingManagementSystem.Application.UnitOfWork;
using ShippingManagementSystem.Domain.Interfaces;
using ShippingManagementSystem.Domain.UserTypes;
using ShippingManagementSystem.Infrastructure.Data;
using ShippingManagementSystem.Web.Swagger.Filter;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

#endregion

#region Create Builder

var builder = WebApplication.CreateBuilder(args);

#endregion

#region Services

#region API Configration

builder.Services.AddControllers();

#endregion

#region Permission Configration

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionOrRolesHandler>();

//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
//builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

//builder.Services.Configure<SecurityStampValidatorOptions>(options =>
//{
//    options.ValidationInterval = TimeSpan.Zero;
//});

#endregion

#region Authentication Configration

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

#endregion

#region Cors Configration

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200", 
                "https://shippnigmanagementsystem.vercel.app",
                "https://shipping-management-system.vercel.app")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
            });
});

#endregion

#region Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region Identity Configration

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ApplicationDbContext>()
 .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

#endregion

#region Get Section Configration

#region JWT Configration

builder.Services.Configure<JWT>(builder.Configuration.GetSection(nameof(JWT)));

#endregion

#region Admin Configration

builder.Services.Configure<AdminLogin>(builder.Configuration.GetSection(nameof(AdminLogin)));

#endregion

#region Email Configration

builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection(nameof(EmailConfiguration)));

#endregion

#endregion

#region Dependancy Injection Configration

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

#endregion

#region OpenAPI Configration
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#endregion

#region Exption Handling

//builder.Services.Configure<ApiBehaviorOptions>(Options =>
//{
//    Options.InvalidModelStateResponseFactory = (ActionContext) =>
//    {
//        var errors = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0).SelectMany(p => p.Value.Errors).Select(e => e.ErrorMessage).ToArray();
//        var reponse = new APIResponseValidationError(errors);
//        return new BadRequestObjectResult(reponse);
//    };
//});

#endregion

#region Swagger Configration

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "📦 ShipFlow: Professional Logistics & Supply Chain API",
        Description = "### 🌐 Overview\n" +
      "**ShipFlow** is a comprehensive, enterprise-grade shipping management system designed to streamline the lifecycle of domestic logistics. " +
      "It provides a unified digital environment for **Merchants**, **Shipping Representatives**, and **Administrators** to coordinate complex delivery operations.\n\n" +
      "#### 🚀 Core System Capabilities\n" +
      "* **Automated Dispatch Engine**: A high-precision workflow that manages order transitions across multiple states (`New`, `Pendding`, `Delivered`) while maintaining strict data integrity.\n" +
      "* **Dynamic Pricing Framework**: An intelligent calculation engine that resolves shipping fees based on **Standard Weights**, **City Surcharges**, and customized **Merchant Special Rates**.\n" +
      "* **Administrative Governance**: Real-time **BI Dashboards** and analytics for monitoring revenue, delivery efficiency, and workforce performance across all geographical branches.\n" +
      "* **Architectural Excellence**: Engineered with the **Specification Pattern** and **Unit of Work** to ensure atomic database transactions and a scalable, decoupled business logic layer.\n\n" +
      "#### 🔑 Identity & Access Governance\n" +
      "Secured via **ASP.NET Core Identity** and **JWT Bearer Authentication**. The API implements a granular **Permission-Based Access Control (PBAC)** model, allowing administrators to define precise module-level permissions for every employee group.\n\n" +
      "#### 🛡️ Compliance & Auditability\n" +
      "Every shipment and user action is protected by a **Soft-Delete Lifecycle** and a standardized **Rejected Reasons Library**, ensuring a permanent audit trail for financial and operational transparency.",
        Contact = new OpenApiContact
        {
            Name = "Khaled Ayman",
            Email = "khaled654ayman0@gmail.com",
            Url = new Uri("https://github.com/khaledaymn")
        }
    });

    // JWT Authentication setup
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });

    option.ExampleFilters();
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        option.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
    option.TagActionsBy(api =>
    {
        var tag = api.ActionDescriptor.EndpointMetadata
                     .OfType<SwaggerOperationAttribute>()
                     .FirstOrDefault()?.Tags;

        if (tag != null && tag.Length > 0)
        {
            return tag;
        }

        return new[] { api.ActionDescriptor.RouteValues["controller"] };
    });
    option.CustomSchemaIds(type => type.FullName?.Replace("ECommerce.", ""));
    option.EnableAnnotations();
    option.DocumentFilter<TagDescriptionsDocumentFilter>();
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

#endregion

#endregion

#region Build Application

var app = builder.Build();

#endregion

#region Check Environment

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var prefix = string.IsNullOrEmpty(options.RoutePrefix) ? "." : "..";
    options.SwaggerEndpoint($"{prefix}/swagger/v1/swagger.json", "Shiping Management System v1");
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    options.DisplayRequestDuration();
});

#endregion

#region Middleware

//app.UseMiddleware<ConvensionExceptionMiddleWare>();

app.UseHttpsRedirection();

app.UseCors("AllowOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion
