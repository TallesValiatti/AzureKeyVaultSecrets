using App.Repositories;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["AzureKeyVaultEndpoint"]), 
                                       new DefaultAzureCredential(new DefaultAzureCredentialOptions
                                       {
                                           ManagedIdentityClientId = builder.Configuration["AzureADManagedIdentityClientId"]
                                       }));

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

