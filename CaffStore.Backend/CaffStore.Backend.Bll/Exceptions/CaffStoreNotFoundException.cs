using System.Net;
using System.Runtime.Serialization;

namespace CaffStore.Backend.Bll.Exceptions
{
	public class CaffStoreNotFoundException : CaffStoreBusinessException
	{
		public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

		public CaffStoreNotFoundException(string message = "Not Found") : base(message)
		{
		}

		protected CaffStoreNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
