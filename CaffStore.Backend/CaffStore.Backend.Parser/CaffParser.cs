using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CaffStore.Backend.Parser
{
	public static class CaffParser
	{
		public static async Task<CaffParseResult> ParseCaffFileAsync(Stream fileStream)
		{
			// TODO fill with real data

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
			var preview = new Bitmap(100, 200);
			preview.SetPixel(1, 1, Color.Red);

			return new CaffParseResult
			{
				Succeeded = true,
				Message = "Success",
				Preview = preview,
				Result = caffData,
			};
		}
	}
}
