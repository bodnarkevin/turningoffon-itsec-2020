using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CaffStore.Backend.Parser
{
	enum BlockType { HEADER, CREDITS, ANIMATION }
	[StructLayout(LayoutKind.Sequential)]
	public class CIFFHeader
	{
		char[] magic;
		ulong header_size;
		ulong content_size;
		ulong width;
		ulong height;
		string caption;
		//std::vector<std::string> tags; //TODO
	}


	[StructLayout(LayoutKind.Sequential)]
	public class CIFFFile
	{
		CAFFHeader header;
		//std::vector<uint8_t> pixels; //Todo
	};

	[StructLayout(LayoutKind.Sequential)]
	public class CAFFHeader
	{
		char[] magic;
		ulong header_size;
		ulong num_anim;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class Date
	{
		ushort year;
		byte month;
		byte day;
		byte hour;
		byte minute;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class Credits
	{
		Date date;
		ulong creator_len;
		string creator;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class Animation
	{
		ulong duration;
		CIFFFile ciff_file;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class Block
	{
		byte id;
		ulong length;
		CAFFHeader header_data;
		Credits credits_data;
		Animation animation_data;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class CAFFFile
	{
		int count;
		Block[] blocks;
	}

	public static class CaffParser
	{
		
		[DllImport("CAFFParser.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		private static extern CAFFFile handleParsing(string filename);


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
