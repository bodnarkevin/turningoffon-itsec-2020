using CaffStore.Backend.Interface.Bll.Services;
using Moq;

namespace CaffStore.Backend.Test.Fixtures
{
	public class FileServiceFixture
	{
		public Mock<IFileService> FileServiceMock { get; }
		public IFileService FileService { get; }

		public FileServiceFixture()
		{
			FileServiceMock = new Mock<IFileService>();
			FileService = FileServiceMock.Object;
		}
	}
}
