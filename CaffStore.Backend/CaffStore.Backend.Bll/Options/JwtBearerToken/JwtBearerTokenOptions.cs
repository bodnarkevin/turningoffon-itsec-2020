namespace CaffStore.Backend.Bll.Options.JwtBearerToken
{
	public class JwtBearerTokenOptions
	{
		public string Authority { get; set; }

		public bool RequireHttpsMetadata { get; set; } = true;
	}
}
