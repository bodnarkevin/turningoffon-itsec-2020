using CaffStore.Backend.Interface.Bll.Services;
using System;

namespace CaffStore.Backend.Bll.Services
{
	public class TimeService : ITimeService
	{
		public DateTimeOffset Now => DateTimeOffset.Now;

		public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
	}
}
