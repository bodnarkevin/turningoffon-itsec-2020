using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Test.Fixtures;
using System.Threading.Tasks;
using Xunit;

namespace CaffStore.Backend.Test
{
	[Collection("Comment tests")]
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

		[Fact]
		public async Task TestDeleteCommentNotByCreator()
		{
			_testFixture.RequestContextFixture.CurrentUserId = 2;

			await Assert.ThrowsAsync<CaffStoreForbiddenException>(() => _testFixture.CommentService.DeleteMyCommentAsync(1));
		}

	}

	/// <summary>
	/// This test class is separated from the above so that de successful delete operation does not affect
	/// the getter tests by deleteing the comment from the db.
	/// </summary>
	[Collection("Comment tests")]
	public class CommentServiceDeleteTests : IClassFixture<CaffStoreTestFixture>
	{
		private readonly CaffStoreTestFixture _testFixture;

		public CommentServiceDeleteTests(CaffStoreTestFixture testFixture)
		{
			_testFixture = testFixture;
		}

		[Fact]
		public async Task TestDeleteCommentByCreator()
		{
			_testFixture.RequestContextFixture.CurrentUserId = 1;

			await _testFixture.CommentService.DeleteMyCommentAsync(1);

			await Assert.ThrowsAsync<CaffStoreNotFoundException>(() => _testFixture.CommentService.GetCommentAsync(1));

		}
	}
}
