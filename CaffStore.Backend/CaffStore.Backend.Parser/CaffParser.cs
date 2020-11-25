using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using System.Drawing.Imaging;

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

		[DllImport("C:/Users/cloud/source/repos/turningoffon-itsec-2020/CaffStore.Backend/Debug/CaffStore.Backend.Parser.Native.dll", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		public static extern string parseToJson(IntPtr pArray, int nSize, out IntPtr preview, out int size, out bool isError);

		private static byte[] ReadBytes(Stream input)
        {
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}

				return ms.ToArray();
			}
		}


		public static async Task<CaffParseResult> ParseCaffFileAsync(Stream fileStream)
		{
			// TODO fill with real data
			byte[] buffer = ReadBytes(fileStream);
			int size = Marshal.SizeOf(buffer[0]) * buffer.Length;
			IntPtr pnt = Marshal.AllocHGlobal(size);

			try
            {
				Marshal.Copy(buffer, 0, pnt, buffer.Length);
            } catch
            {
				//Free unmanaged memory
            }

			bool error = true;
			int preSize;
			IntPtr preview;

			string parsedJson = parseToJson(pnt, buffer.Length, out preview, out preSize, out error);

			if (error)
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
				var previewDummy = new Bitmap(100, 200);
				previewDummy.SetPixel(1, 1, Color.Red);

				return new CaffParseResult
				{
					Succeeded = false,
					Message = "Failure",
					Preview = previewDummy,
					Result = caffData,
				};
			} else
            {
				CaffDataDto caffDataFromParser = JsonConvert.DeserializeObject<CaffDataDto>(parsedJson);

				byte[] previewArray = new byte[preSize];
				Marshal.Copy(preview, previewArray, 0, preSize);

				Bitmap bmp = new Bitmap(caffDataFromParser.Animations.ToList()[0].CiffData.Width, caffDataFromParser.Animations.ToList()[0].CiffData.Height, PixelFormat.Format24bppRgb);
				BitmapData bmData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
				IntPtr pNative = bmData.Scan0;
				Marshal.Copy(previewArray, 0, pNative, caffDataFromParser.Animations.ToList()[0].CiffData.Width * caffDataFromParser.Animations.ToList()[0].CiffData.Height * 3);
				bmp.UnlockBits(bmData);

				Marshal.FreeHGlobal(pnt);

				return new CaffParseResult
				{
					Succeeded = true,
					Message = "Success",
					Preview = bmp,
					Result = caffDataFromParser
				};
            }
		}
	}
}
