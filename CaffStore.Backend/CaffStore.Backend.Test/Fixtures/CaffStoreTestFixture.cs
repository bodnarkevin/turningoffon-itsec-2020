using CaffStore.Backend.Api.Controllers;
using CaffStore.Backend.Bll.Services;
using CaffStore.Backend.Interface.Bll.Services;

namespace CaffStore.Backend.Test.Fixtures
{
	public class CaffStoreTestFixture
	{
		public TimeServiceFixture TimeServiceFixture { get; }
		public RequestContextFixture RequestContextFixture { get; set; }
		public DatabaseFixture Database { get; }
		public MapperFixture MapperFixture { get; }
		public FileServiceFixture FileServiceFixture { get; }
		public ICaffItemService CaffItemService { get; }

		public CaffStoreTestFixture()
		{
			TimeServiceFixture = new TimeServiceFixture();
			RequestContextFixture = new RequestContextFixture();
			Database = new DatabaseFixture(TimeServiceFixture.TimeService, RequestContextFixture.RequestContext);
			MapperFixture = new MapperFixture();
			FileServiceFixture = new FileServiceFixture();
			CaffItemService = new CaffItemService(Database.Context, RequestContextFixture.RequestContext,
				FileServiceFixture.FileService, MapperFixture.Mapper);
		}
	}
}
