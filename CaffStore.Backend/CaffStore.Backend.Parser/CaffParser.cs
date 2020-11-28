using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace CaffStore.Backend.Parser
{
	public static class CaffParser
	{

		[DllImport("Library/CaffParser.so", CharSet=CharSet.Ansi, CallingConvention=CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.LPStr)]
		private static extern string parseToJson(IntPtr pArray, int nSize, out IntPtr preview, out int size, out bool isError);

		public static async Task<CaffParseResult> ParseCaffFileAsync(Stream fileStream)
		{
			byte[] buffer;

			using (MemoryStream ms = new MemoryStream())
			{
				fileStream.CopyTo(ms);
				buffer = ms.ToArray();
			}

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

			string resultJson = parseToJson(pnt, buffer.Length, out preview, out preSize, out error);

			if (error)
			{
				return new CaffParseResult
				{
					Succeeded = false,
					Message = resultJson,
					Preview = null,
					Result = null,
				};
			} else
            {
				
				CaffDataDto caffDataFromParser = JsonConvert.DeserializeObject<CaffDataDto>(resultJson);

				byte[] previewArray = new byte[preSize];
				Marshal.Copy(preview, previewArray, 0, preSize);

				int ciffWidth = caffDataFromParser.Animations.ToList()[0].CiffData.Width;
				int ciffHeight = caffDataFromParser.Animations.ToList()[0].CiffData.Height;

				Bitmap bmp = new Bitmap(ciffWidth, ciffHeight);
				int x = 0;
				int y = 0;

				for (int i = 0; i < previewArray.Length; i += 3)
				{
					byte r = previewArray[i];
					byte g = previewArray[i + 1];
					byte b = previewArray[i + 2];
					bmp.SetPixel(x, y, Color.FromArgb(r, g, b));

					if (x < ciffWidth - 1)
					{
						x++;
					} else
					{
						x = 0;
						y++;
					}
				}

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
