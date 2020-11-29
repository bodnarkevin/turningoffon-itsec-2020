using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Test.Fixtures;
using System.Threading.Tasks;
using Xunit;

namespace CaffStore.Backend.Test
{
	public class CommentServiceTests : IClassFixture<CaffStoreTestFixture>
	{
		private readonly CaffStoreTestFixture _testFixture;

		public CommentServiceTests(CaffStoreTestFixture testFixture)
		{
			_testFixture = testFixture;
		}

		[Fact]
		public async Task TestGetComment()
		{
			const long commentId = 1;

			var response = await _testFixture.CommentService.GetCommentAsync(commentId);

			Assert.NotNull(response);
			Assert.NotNull(response.Text);
		}

		[Fact]
		public async Task TestUpdateComment()
		{
			_testFixture.RequestContextFixture.CurrentUserId = 1;

			const string newText = "New Text";

			var response = await _testFixture.CommentService.UpdateMyCommentAsync(1,
				new UpdateCommentDto
				{
					Text = newText
				});

			Assert.NotNull(response);
			Assert.Equal(newText, response.Text);
		}

		[Fact]
		public async Task TestUpdateCommentNotByCreator()
		{
			_testFixture.RequestContextFixture.CurrentUserId = 2;

			const string newText = "New Text";

			await Assert.ThrowsAsync<CaffStoreForbiddenException>(() => _testFixture.CommentService.UpdateMyCommentAsync(1,
				new UpdateCommentDto
				{
					Text = newText
				}));
		}
	}
}
