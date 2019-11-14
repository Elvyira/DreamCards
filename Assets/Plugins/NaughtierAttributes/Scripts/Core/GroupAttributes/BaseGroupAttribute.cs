namespace NaughtierAttributes
{
    public abstract class BaseGroupAttribute : BaseNaughtierAttribute
    {
        public string Name { get; private set; }
        
        public string BackgroundColorName { get; private set; }
        
        public string ContentColorName { get; private set; }
        
        public ColorValue BackgroundColor { get; private set; }
        
        public ColorValue ContentColor { get; private set; }

        protected BaseGroupAttribute(string name, string backgroundColorName, string contentColorName)
        {
            Name = name;
            BackgroundColorName = backgroundColorName;
            ContentColorName = contentColorName;
            BackgroundColor = ColorValue.Default;
            ContentColor = ColorValue.Default;
        }

        protected BaseGroupAttribute(string name, ColorValue backgroundColor, ColorValue contentColor)
        {
            Name = name;
            BackgroundColor = backgroundColor;
            ContentColor = contentColor;
        }
    }
}
