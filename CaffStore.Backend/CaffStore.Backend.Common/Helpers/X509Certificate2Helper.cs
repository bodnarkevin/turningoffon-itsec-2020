using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Security.Cryptography.X509Certificates;

namespace CaffStore.Backend.Common.Helpers
{
	public static class X509Certificate2Helper
	{
		public static X509Certificate2 GetCertificateFromAzureKeyVaultAsync(string keyVaultUri, string certificateName)
		{
			if (string.IsNullOrEmpty(keyVaultUri))
				throw new ArgumentNullException(nameof(keyVaultUri));
			if (string.IsNullOrEmpty(certificateName))
				throw new ArgumentNullException(nameof(certificateName));

			var client = new SecretClient(vaultUri: new Uri(keyVaultUri), credential: new DefaultAzureCredential());

			var secret = client.GetSecret(certificateName);
			var certificate = new X509Certificate2(Convert.FromBase64String(secret.Value.Value));

			return certificate;
		}
	}
}
