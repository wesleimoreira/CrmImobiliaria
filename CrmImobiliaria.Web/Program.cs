using CrmImobiliaria.Application;
using CrmImobiliaria.Infrastructure;
using CrmImobiliaria.Infrastructure.Identity;
using CrmImobiliaria.Infrastructure.Persistence;
using CrmImobiliaria.Web.Components;
using CrmImobiliaria.Web.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddMudServices();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies(o => o.ApplicationCookie!.Configure(c =>
    {
        c.LoginPath = "/conta/login";
        c.AccessDeniedPath = "/conta/acesso-negado";
    }));

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<CrmDbContext>()
    .AddSignInManager()
    .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await MigrarBancoAsync(app);
await SeedIdentityAsync(app);

app.Run();

// Aplica migrations pendentes automaticamente na subida — em produção (container) não há
// como rodar "dotnet ef database update" manualmente no servidor, então o próprio app garante
// que o schema está em dia antes de aceitar requisições.
static async Task MigrarBancoAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<CrmDbContext>();
    await context.Database.MigrateAsync();
}

static async Task SeedIdentityAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    foreach (var papel in Papeis.Todos)
    {
        if (!await roleManager.RoleExistsAsync(papel))
            await roleManager.CreateAsync(new IdentityRole<Guid>(papel));
    }

    if (userManager.Users.Any())
        return;

    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var email = configuration["Seed:AdminEmail"] ?? "admin@crm.local";
    var senha = configuration["Seed:AdminPassword"] ?? "Admin@12345";

    var admin = new ApplicationUser
    {
        UserName = email,
        Email = email,
        NomeCompleto = "Administrador",
        EmailConfirmed = true,
        Ativo = true,
    };

    var resultado = await userManager.CreateAsync(admin, senha);
    if (resultado.Succeeded)
        await userManager.AddToRoleAsync(admin, Papeis.Administrador);
}
