using System;

namespace NaughtierAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowAssetPreviewAttribute : BaseElementDecoratorAttribute
    {
        public int Size { get; private set; }
        public Align Align { get; private set; }
        public string BackgroundColor { get; private set; }
        public string ContentColor { get; private set; }

        public ShowAssetPreviewAttribute(int size = 64, Align align = Align.Left, string backgroundColor = null,
            string contentColor = null)
        {
            this.Size = size;
            Align = align;
            BackgroundColor = backgroundColor;
            ContentColor = contentColor;
        }

        public ShowAssetPreviewAttribute(Align align, string backgroundColor = null, string contentColor = null)
        {
            this.Size = 64;
            Align = align;
            BackgroundColor = backgroundColor;
            ContentColor = contentColor;
        }
    }
}