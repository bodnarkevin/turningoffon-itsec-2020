using System;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface ITimeService
	{
		DateTimeOffset Now { get; }
		DateTimeOffset UtcNow { get; }
	}
}
