namespace CaffStore.Backend.Parser
{
	public class CaffParseResult
	{
		public bool Succeeded { get; set; }

		public string ErrorMessage { get; set; }

		public CaffFile Result { get; set; }
	}
}
