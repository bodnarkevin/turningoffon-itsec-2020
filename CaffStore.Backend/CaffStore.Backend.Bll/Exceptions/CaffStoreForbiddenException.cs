using System;
using System.Net;
using System.Runtime.Serialization;

namespace CaffStore.Backend.Bll.Exceptions
{
	[Serializable]
	public class CaffStoreForbiddenException : CaffStoreBusinessException
	{
		public override HttpStatusCode StatusCode => HttpStatusCode.Forbidden;

		public CaffStoreForbiddenException(string message = "Forbidden") : base(message)
		{
		}

		protected CaffStoreForbiddenException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
