using CaffStore.Backend.Interface.Bll.RequestContext;
using Moq;

namespace CaffStore.Backend.Test.Fixtures
{
	public class RequestContextFixture
	{
		private long? _currentUserId;

		public long? CurrentUserId
		{
			get => _currentUserId;
			set
			{
				_currentUserId = value;
				RequestContextMock.Setup(context => context.CurrentUserId).Returns(_currentUserId);
			}
		}

		public Mock<IHttpRequestContext> RequestContextMock { get; }
		public IHttpRequestContext RequestContext { get; }

		public RequestContextFixture()
		{
			RequestContextMock = new Mock<IHttpRequestContext>();
			RequestContext = RequestContextMock.Object;

			CurrentUserId = 1;
		}
	}
}
