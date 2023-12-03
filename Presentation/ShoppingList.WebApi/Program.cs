using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ShoppingList.Application.Validators.Categories;
using ShoppingList.Persistence.IOC;
using ShoppingList.WebApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddPersistenceServiceIOC();

builder.Host.UseWindowsService();

builder.Services.AddControllers()
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<AddCategoryValidator>()); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//bu şekilde ioc ye eşleşmeyi tanıtıyorum. böylece aşağıda da ilgli tokenoptinon çağırabiliyorum.
builder.Services.Configure<TokenOption>(builder.Configuration.GetSection("TokenOption"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        //Tokenoption sınıfı model klsörü altında ve buradaki nesne ile appsetting.json içinde tanımlanan TokenOption nesnesini mapliyor.
        TokenOption tokenOption = builder.Configuration.GetSection("TokenOption").Get<TokenOption>();

        //https kullanılmayacaksa bu kısım false yapılabilir
        //options.RequireHttpsMetadata = true;

        //nelerin valide edileceğini belirtiyoruz
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidIssuer = tokenOption.Issuer,
            ValidAudience = tokenOption.Audience,
            ValidateIssuer=true,
            ValidateAudience=true,
            ClockSkew=TimeSpan.Zero,
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey))
        };
    });


builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
{
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
