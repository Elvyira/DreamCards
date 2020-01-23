#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    [DrawerTarget(typeof(DisableIfAttribute))]
    public class DisableIfDecoratorDrawer : BaseDecoratorDrawer, IRefreshDrawer
    {
        private readonly MightyCache<(bool, bool, MightyInfo<bool>[])> m_disableIfCache =
            new MightyCache<(bool, bool, MightyInfo<bool>[])>();

        public override void BeginDraw(BaseMightyMember mightyMember, BaseDecoratorAttribute baseAttribute)
        {
            if (!m_disableIfCache.Contains(mightyMember)) InitDrawer(mightyMember, baseAttribute);
            var (valid, disabled, _) = m_disableIfCache[mightyMember];

            if (valid)
                GUI.enabled = !disabled;
            else
                EditorDrawUtility.DrawHelpBox($"{typeof(DisableIfAttribute).Name} needs a valid boolean condition to work");
        }

        public override void EndDraw(BaseMightyMember mightyMember, BaseDecoratorAttribute baseAttribute) => GUI.enabled = true;

        public override void InitDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            var target = mightyMember.InitAttributeTarget<DisableIfAttribute>();
            var attribute = (DisableIfAttribute) mightyAttribute;
            var property = mightyMember.Property;

            var enabled = true;
            var valid = false;
            var infos = new MightyInfo<bool>[attribute.ConditionNames.Length];
            for (var i = 0; i < attribute.ConditionNames.Length; i++)
            {
                var conditionName = attribute.ConditionNames[i];
                if (!property.GetBoolInfo(target, conditionName, out infos[i])) continue;
                enabled = enabled && !infos[i].Value;
                valid = true;
            }

            m_disableIfCache[mightyMember] = (valid, enabled, infos);
        }

        public override void ClearCache() => m_disableIfCache.ClearCache();
        
        public void RefreshDrawer(BaseMightyMember mightyMember, BaseMightyAttribute mightyAttribute)
        {
            if (!m_disableIfCache.Contains(mightyMember))
            {
                InitDrawer(mightyMember, mightyAttribute);
                return;
            }

            var (valid, _, infos) = m_disableIfCache[mightyMember];
            if (!valid) return;
            
            var canDraw = true;
            foreach (var info in infos)
            {
                info.RefreshValue();
                canDraw = canDraw && !info.Value;
            }

            m_disableIfCache[mightyMember] = (true, canDraw, infos);
        }
    }
}
#endif