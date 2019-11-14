using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProgressBarAttribute : BaseDrawerAttribute
    {
        public string Name { get; private set; }
        public float MaxValue { get; private set; }
        public ColorValue Color { get; private set; }
        public string ColorName { get; private set; }

        public ProgressBarAttribute(string name = "", float maxValue = 100, ColorValue color = ColorValue.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
        }
        
        public ProgressBarAttribute(string name = "", float maxValue = 100, string colorName = "")
        {
            Name = name;
            MaxValue = maxValue;
            ColorName = colorName;
        }
    }
}