namespace BlazingDocumentor.Helper
{
	public static class Pluralizer
	{
		public static string Pluralize(string word) => new Pluralize.NET.Pluralizer().Pluralize(word);
	}
}
