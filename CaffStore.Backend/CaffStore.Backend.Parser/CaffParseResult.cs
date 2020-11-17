using System.Drawing;
using CaffStore.Backend.Interface.Bll.Dtos.Caff;

namespace CaffStore.Backend.Parser
{
	public class CaffParseResult
	{
		public bool Succeeded { get; set; }

		public string Message { get; set; }

		public CaffDataDto Result { get; set; }

		public Bitmap Preview { get; set; }
	}
}
