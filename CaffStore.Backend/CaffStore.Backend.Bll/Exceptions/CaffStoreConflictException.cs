using System;
using System.Net;
using System.Runtime.Serialization;

namespace CaffStore.Backend.Bll.Exceptions
{
	[Serializable]
	public class CaffStoreConflictException : CaffStoreBusinessException
	{
		public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

		public CaffStoreConflictException(string message) : base(message)
		{
		}

		protected CaffStoreConflictException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
