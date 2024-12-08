using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modsen.API;
using Modsen.Application;
using Modsen.Domain;
using Modsen.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ModsenContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
builder.Services.AddScoped<GetAllUsersUseCase>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<GetEventsUseCase>();
builder.Services.AddScoped<CreateEventUseCase>();
builder.Services.AddScoped<UpdateEventUseCase>();
builder.Services.AddScoped<DeleteEventUseCase>();
builder.Services.AddScoped<AddImagesToEventUseCase>();

builder.Services.AddScoped<SaveImagesUseCase>();
builder.Services.AddScoped<DeleteUnusedImagesUseCase>();
builder.Services.AddScoped<DeleteImagesUseCase>();
builder.Services.AddScoped<ImageService>();

builder.Services.AddScoped<EmailService>();

builder.Services.AddScoped<GetEventMembersUseCase>();
builder.Services.AddScoped<GetMemberByIdUseCase>();
builder.Services.AddScoped<RegisterMemberUseCase>();
builder.Services.AddScoped<UnregisterMemberUseCase>();

builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly);

builder.Services.AddSingleton<IExceptionHandler, ExceptionHandlerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<GenerateAccessTokenUseCase>();
builder.Services.AddScoped<GenerateRefreshTokenUseCase>();
builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<IEventChecker, EventChecker>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<ITokenClaimsService, TokenClaimsService>();

builder.Services.AddScoped<IPasswordHashingService, PasswordHashingService>();
builder.Services.AddScoped<ITokenClaimsService, TokenClaimsService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<SaveImagesUseCase>();
builder.Services.AddScoped<DeleteUnusedImagesUseCase>();
builder.Services.AddScoped<SendEmailUseCase>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ImageService>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<CheckMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
