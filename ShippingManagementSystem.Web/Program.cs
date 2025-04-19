#region Using Directives

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShippingManagementSystem.Infrastructure.Data;
using System.Reflection;
using ShippingManagementSystem.Application.Settings;
using ShippingManagementSystem.Domain.UserTypes;
using ShippingManagementSystem.Application.Exptions;
using ShippingManagementSystem.Application.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using ShippingManagementSystem.Application.Filters;
using ShippingManagementSystem.Application.Services.ExternalServices.EmailServices;
using ShippingManagementSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ShippingManagementSystem.Application.Helpers;

#endregion

#region Create Builder

var builder = WebApplication.CreateBuilder(args);

#endregion

#region Services

#region API Configration

builder.Services.AddControllers();

#endregion


#region Permission Configration

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOrMerchantOrShippingRepOrHasPermission", policy =>
//        policy.RequireAssertion(context =>
//            // ????? 1: ?? ?? ??? ???????
//            context.User.IsInRole(Roles.Admin) ||
//            context.User.IsInRole(Roles.Merchant) ||
//            context.User.IsInRole(Roles.ShippingRepresentative) ||

//            // ????? 2: ?? ???? ????????
//            context.User.HasClaim(c =>
//                c.Type == Constants.Permission &&
//                c.Value == $"{Medules.ShippingRepresentatives}.{Constants.View}")
//        ));
//});

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});

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
            builder.WithOrigins("http://localhost:4200", "http://localhost:50534", "http://localhost:60711")
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
        Title = "Shiping Management System",
        Description = "API documentation for Shiping Management System",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Khaled Ayman",
            Email = "khaled654ayman0@gmail.com"
        }
        //License = new OpenApiLicense
        //{
        //    Name = "Not Found any license",
        //    Url = new Uri("https://example.com/license")
        //}
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

    // Enable XML comments if available
    //var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    //option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

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
