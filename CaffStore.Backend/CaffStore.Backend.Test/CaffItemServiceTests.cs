using CaffStore.Backend.Api.Pagination.Queries;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Test.Fixtures;
using System.Threading.Tasks;
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
		public async Task TestGetPagedCaffItems()
		{
			var page = 1;
			var pageSize = 5;

			var response = await _testFixture.CaffItemService.GetPagedCaffItemsAsync(new PagedQuery
			{
				Page = 1,
				PageSize = 5
			});

			Assert.NotNull(response);
			Assert.NotNull(response.Results);
			Assert.Equal(page, response.CurrentPage);
			Assert.Equal(pageSize, response.PageSize);
			Assert.NotEqual(0, response.Results.Count);
		}

		[Fact]
		public async Task TestGetCaffItem()
		{
			var response = await _testFixture.CaffItemService.GetCaffItemAsync(1);

			Assert.NotNull(response);
			Assert.Equal(1, response.Id);
		}

		[Fact]
		public async Task TestUpdateCaffItem()
		{
			_testFixture.RequestContextFixture.CurrentUserId = 1;

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
		public async Task TestUpdateCaffItemNotByCreator()
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

		[Fact]
		public async Task TestDeleteCaffItemByCreator()
		{
			_testFixture.RequestContextFixture.CurrentUserId = 1;

			await _testFixture.CaffItemService.DeleteMyCaffItemAsync(2);

			await Assert.ThrowsAsync<CaffStoreNotFoundException>(() => _testFixture.CaffItemService.GetCaffItemAsync(2));
		}

		[Fact]
		public async Task GetCaffItemComments()
		{
			const long caffItemId = 1;

			var response = await _testFixture.CaffItemService.GetCaffItemCommentsAsync(caffItemId);

			Assert.NotNull(response);
			Assert.NotEmpty(response);
		}

		[Fact]
		public async Task AddCaffItemComment()
		{
			const long caffItemId = 1;
			const string commentText = "New comment";

			var response = await _testFixture.CaffItemService.AddCaffItemCommentAsync(caffItemId,
				new AddCommentDto
				{
					Text = commentText
				});

			Assert.NotNull(response);
			Assert.Equal(commentText, response.Text);
		}
	}
}
