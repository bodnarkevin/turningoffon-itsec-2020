using AutoMapper;

namespace CaffStore.Backend.Test.Fixtures
{
	public class MapperFixture
	{
		public IMapper Mapper { get; set; }

		public MapperFixture()
		{
			var configuration = new MapperConfiguration(cfg =>
					cfg.AddMaps("CaffStore.Backend.Bll")
				);

			Mapper = configuration.CreateMapper();
		}
	}
}
