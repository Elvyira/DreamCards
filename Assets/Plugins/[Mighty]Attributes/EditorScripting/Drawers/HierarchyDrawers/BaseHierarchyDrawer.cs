#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public abstract class BaseHierarchyDrawer : BaseMightyDrawer
    {
        public abstract void Update(MonoBehaviour monoBehaviour, BaseHierarchyAttribute baseAttribute);

        public abstract void OnGUI(int instanceID, Rect selectionRect, BaseHierarchyAttribute baseAttribute);

        public override void InitDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
        }

        public override void ClearCache()
        {
        }
    }
}
#endif