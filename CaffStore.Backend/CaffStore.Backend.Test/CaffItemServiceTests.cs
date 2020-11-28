using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using CaffStore.Backend.Test.Fixtures;
using Xunit;

namespace CaffStore.Backend.Test
{
	public class CaffItemServiceTests : IClassFixture<CaffStoreTestFixture>
	{
		private readonly CaffStoreTestFixture _testFixture;

		public CaffItemServiceTests(CaffStoreTestFixture testFixture)
		{
			_testFixture = testFixture;
		}

		[Fact]
		public async void TestGetCaffItem()
		{
			var response = await _testFixture.CaffItemService.GetCaffItemAsync(1);

			Assert.NotNull(response);
			Assert.Equal(1, response.Id);
		}

		[Fact]
		public async void TestUpdateCaffItem()
		{
			const string newTitle = "New Title";
			const string newDescription = "New Description";

			var response = await _testFixture.CaffItemService.UpdateMyCaffItemAsync(1, new UpdateCaffItemDto
			{
				Title = newTitle,
				Description = newDescription
			});

			Assert.NotNull(response);
			Assert.Equal(newTitle, response.Title);
			Assert.Equal(newDescription, response.Description);
		}

		[Fact]
		public async System.Threading.Tasks.Task TestUpdateCaffItemNotByCreator()
		{
			_testFixture.RequestContextFixture.CurrentUserId = 2;

			const string newTitle = "New Title";
			const string newDescription = "New Description";

			await Assert.ThrowsAsync<CaffStoreForbiddenException>(() => _testFixture.CaffItemService.UpdateMyCaffItemAsync(1,
				new UpdateCaffItemDto
				{
					Title = newTitle,
					Description = newDescription
				}));
		}
	}
}
