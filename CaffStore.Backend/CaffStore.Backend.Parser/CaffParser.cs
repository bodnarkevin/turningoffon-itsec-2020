using CaffStore.Backend.Interface.Bll.Dtos.Caff;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CaffStore.Backend.Parser
{
	public static class CaffParser
	{
		public static async Task<CaffParseResult> ParseCaffFileAsync(Stream fileStream)
		{
			var tags = new[] { "tag1", "tag2", "tag3" };
			var ciffData = new CiffDataDto
			{
				Caption = "ciff1",
				Height = 100,
				Width = 200,
				Tags = tags,
			};
			var animation = new CaffAnimationDataDto
			{
				Order = 1,
				Duration = 500,
				CiffData = ciffData,
			};
			var caffData = new CaffDataDto
			{
				Creator = "creator1",
				Creation = DateTime.Now,
				Animations = new[] { animation },
			};
			return new CaffParseResult
			{
				Succeeded = true,
				Message = "Success",
				Preview = null,
				Result = caffData,
			};
		}
	}
}
