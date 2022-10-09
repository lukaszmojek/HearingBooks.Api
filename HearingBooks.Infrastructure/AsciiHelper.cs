using System.Text;

namespace HearingBooks.Infrastructure;

public static class AsciiHelper
{
	public static string CleanFromNonAsciiCharacters(this string value)
	{
		return Encoding.ASCII.GetString(
			Encoding.Convert(
				Encoding.UTF8,
				Encoding.GetEncoding(
					Encoding.ASCII.EncodingName,
					new EncoderReplacementFallback(string.Empty),
					new DecoderExceptionFallback()
				),
				Encoding.UTF8.GetBytes(value)
			)
		);
	}
}