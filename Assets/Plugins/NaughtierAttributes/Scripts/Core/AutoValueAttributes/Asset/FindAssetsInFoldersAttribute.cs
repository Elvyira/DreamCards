namespace NaughtierAttributes
{
    public class FindAssetsInFoldersAttribute : BaseSearchAssetAttribute
    {
        public readonly string[] Folders;

        public FindAssetsInFoldersAttribute(params string[] folders) : base(false)
        {
            Folders = folders;
        }
        
        public FindAssetsInFoldersAttribute(string[] folders, bool playUpdate = false) : base(playUpdate)
        {
            Folders = folders;
        }

        public FindAssetsInFoldersAttribute(string name, string[] folders, bool playUpdate = false) : base(name, playUpdate)
        {
            Folders = folders;
        }
    }
}