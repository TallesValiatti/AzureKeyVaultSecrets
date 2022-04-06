using App.Repositories;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration.AzureKeyVault;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((context, config) =>
{
    var settings = config.Build();
    var keyVaultEndpoint = settings["AzureKeyVaultEndpoint"];

    if (!string.IsNullOrEmpty(keyVaultEndpoint))
    {
        var azureServiceTokenProvider = new AzureServiceTokenProvider();
        var authCallback = new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);
        var keyVaultClient = new KeyVaultClient(authCallback);

        config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
    }
});

builder.Services.AddScoped<DbContext>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();


app.MapGet("/user", async (IUserRepository userRepository) =>
{
    return await userRepository.GetAllAsync();
})
.WithName("GetUsersAsync");

app.Run();

