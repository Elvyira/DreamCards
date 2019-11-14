namespace NaughtierAttributes
{
    public class FindAssetInFoldersAttribute : BaseSearchAssetAttribute
    {
        public readonly string[] Folders;
        
        public FindAssetInFoldersAttribute(params string[] folders) : base(false)
        {
            Folders = folders;
        }
        
        public FindAssetInFoldersAttribute(string[] folders, bool playUpdate = false) : base(playUpdate)
        {
            Folders = folders;
        }

        public FindAssetInFoldersAttribute(string name, string[] folders, bool playUpdate = false) : base(name, playUpdate)
        {
            Folders = folders;
        }
    }
}