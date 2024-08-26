using JwtOrnek.Entities;
using JwtOrnek.Helpers;
using JwtOrnek.Services.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Projemizde DbContext olarak ApplicationDbContext kullanaca??m?z belirtliyoruz.
builder.Services.AddDbContext<ApplicationDbContext>();
// appsettings.json içinde olu?turdu?umuz gizli anahtar?m?z? AppSettings ile ça??raca??m?z? söylüyoruz.
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);


// Olu?turdu?umuz gizli anahtar?m?z? byte dizisi olarak al?yoruz.
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

builder.Services.AddSingleton(appSettings);

//Projede farkl? authentication tipleri olabilece?i için varsay?lan olarak JWT ile kontrol edece?imizin bilgisini kaydediyoruz.
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})


    //JWT kullanaca??m ve ayarlar? da ?unlar olsun dedi?imiz yer ise buras?d?r.
    .AddJwtBearer(x =>
    {
        //Gelen isteklerin sadece HTTPS yani SSL sertifikas? olanlar? kabul etmesi(varsay?lan true)
        x.RequireHttpsMetadata = false;
        //E?er token onaylanm?? ise sunucu taraf?nda kay?t edilir.
        x.SaveToken = true;
        //Token içinde neleri kontrol edece?imizin ayarlar?.
        x.TokenValidationParameters = new TokenValidationParameters
        {
            //Token 3.k?s?m(imza) kontrolü
            ValidateIssuerSigningKey = true,
            //Neyle kontrol etmesi gerektigi
            IssuerSigningKey = new SymmetricSecurityKey(key),
            //Bu iki ayar ise "aud" ve "iss" claimlerini kontrol edelim mi diye soruyor
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

//Son olarak authentication kullanaca??m?z? belirtiyoruz.
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
