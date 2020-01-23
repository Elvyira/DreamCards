#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    [DrawerTarget(typeof(HierarchyIconAttribute))]
    public class HierarchyIconDrawer : BaseHierarchyDrawer
    {
        private static readonly Dictionary<int, (int, bool, Texture2D)> IconsByID = new Dictionary<int, (int, bool, Texture2D)>();

        public override void Update(MonoBehaviour monoBehaviour, BaseHierarchyAttribute baseAttribute)
        {
            var instanceID = monoBehaviour.gameObject.GetInstanceID();
            var hasChildren = monoBehaviour.transform.childCount != 0;

            if (IconsByID.ContainsKey(instanceID))
            {
                if (IconsByID[instanceID].Item1 > baseAttribute.Priority)
                    IconsByID[instanceID] = (baseAttribute.Priority, hasChildren,
                        GetTexture(monoBehaviour, (HierarchyIconAttribute) baseAttribute));
                return;
            }

            if (GetTexture(monoBehaviour, (HierarchyIconAttribute) baseAttribute) is Texture2D texture)
                IconsByID.Add(instanceID, (baseAttribute.Priority, hasChildren, texture));
        }

        public override void OnGUI(int instanceID, Rect selectionRect, BaseHierarchyAttribute baseAttribute)
        {
            if (!IconsByID.ContainsKey(instanceID)) return;
            
            var (_, hasChildren, texture2D) = IconsByID[instanceID];
            selectionRect.x -= hasChildren ? 30 : 16;
            GUI.Label(selectionRect, texture2D);
        }

        private Texture2D GetTexture(object target, HierarchyIconAttribute attribute)
        {
            var path = attribute.IconPath;
            if (attribute.PathAsCallback && target.GetValueFromMember(attribute.IconPath, out string pathValue))
                path = pathValue;

            return AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
        }
    }
}
#endif