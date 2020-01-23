#if UNITY_EDITOR
namespace MightyAttributes.Editor
{
    public abstract class BaseWrapperDrawer : BaseMightyDrawer
    {
        public override void InitDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
        }

        public override void ClearCache()
        {
        }
    }
}
#endif