using CaffStore.Backend.Interface.Bll.Services;
using Moq;
using System;

namespace CaffStore.Backend.Test.Fixtures
{
	public class TimeServiceFixture
	{
		public DateTimeOffset UtcNow { get; }
		public Mock<ITimeService> TimeServiceMock { get; }
		public ITimeService TimeService { get; }

		public TimeServiceFixture()
		{
			UtcNow = DateTimeOffset.UtcNow;

			TimeServiceMock = new Mock<ITimeService>();
			TimeServiceMock.Setup(service => service.Now).Returns(UtcNow);
			TimeServiceMock.Setup(service => service.UtcNow).Returns(UtcNow);

			TimeService = TimeServiceMock.Object;
		}
	}
}
