namespace NaughtierAttributes
{
	public class TagObjectAttribute : BaseSearchObjectAttribute
	{
		public readonly string Tag;

		public TagObjectAttribute(string tag, bool includeInactive, bool playUpdate) : base(includeInactive, playUpdate)
		{
			Tag = tag;
		}
	}
}