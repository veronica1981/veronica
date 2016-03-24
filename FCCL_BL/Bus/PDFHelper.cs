using System.IO;
using System.Text.RegularExpressions;

namespace FCCL_BL.Bus
{
	public class PDFHelper
	{
		public static int GetNumberOfPdfPages(string fileName)
		{
			using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
			{
				Regex regex = new Regex(@"/Type\s*/Page[^s]");
				MatchCollection matches = regex.Matches(sr.ReadToEnd());
				return matches.Count;
			}
		}
	}
}
