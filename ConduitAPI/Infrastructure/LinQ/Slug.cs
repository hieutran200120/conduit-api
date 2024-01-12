using System.Text.RegularExpressions;

namespace ConduitAPI.Infrastructure.LinQ
{
	public static class Slug
	{
		private static Regex InvalidCharsRegex = new Regex("[^a-z0-9\\s-]");
		private static Regex MultipleSpacesRegex = new Regex("\\s+");
		private static Regex TrimRegex = new Regex("\\s");

		public static string GenerateSlug(this string phrase)
		{
			if (phrase is null)
			{
				return null;
			}

			var str = phrase.ToLowerInvariant();
			// invalid chars
			str = InvalidCharsRegex.Replace(str, "");
			// convert multiple spaces into one space
			str = MultipleSpacesRegex.Replace(str, " ").Trim();
			// cut and trim
			str = str.Substring(0, Math.Min(str.Length, 45)).Trim();
			str = TrimRegex.Replace(str, "-"); // hyphens
			return str;
		}
	}
}
