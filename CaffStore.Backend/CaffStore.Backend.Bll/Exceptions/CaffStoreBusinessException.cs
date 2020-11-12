using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace CaffStore.Backend.Bll.Exceptions
{
	[Serializable]
	public class CaffStoreBusinessException : Exception
	{
		public virtual HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

		public virtual IEnumerable<string> Errors { get; }

		public CaffStoreBusinessException(string message) : base(message)
		{
		}

		public CaffStoreBusinessException(string message, IEnumerable<string> errors) : base(message)
		{
			Errors = errors;
		}

		protected CaffStoreBusinessException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
