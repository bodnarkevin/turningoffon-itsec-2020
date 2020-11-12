namespace CaffStore.Backend.Bll.Options.IdentityServer
{
	public class IdentityServerOptions
	{
		public SigningCredentialSourceType SigningCredentialSourceType { get; set; }

		public string AzureKeyVaultUri { get; set; }

		public string AzureKeyVaultSigningCertificateName { get; set; }
	}
}
