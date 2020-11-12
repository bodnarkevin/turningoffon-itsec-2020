using CaffStore.Backend.Bll.Options.KeyVault;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace CaffStore.Backend.Api.KeyVault
{
	public static class ConfigurationBuilderExtensions
	{
		public static IConfigurationBuilder AddCostributeAzureKeyVault(this IConfigurationBuilder builder)
		{
			var rootConfig = builder.Build();
			var keyVaultOptions = new KeyVaultOptions();
			rootConfig.GetSection(typeof(KeyVaultOptions).Name).Bind(keyVaultOptions);

			var azureServiceTokenProvider = new AzureServiceTokenProvider();
			var keyVaultClient = new KeyVaultClient(
				new KeyVaultClient.AuthenticationCallback(
					azureServiceTokenProvider.KeyVaultTokenCallback));
			builder.AddAzureKeyVault(
				keyVaultOptions.VaultUri, keyVaultClient, new DefaultKeyVaultSecretManager());

			return builder;
		}
	}
}
