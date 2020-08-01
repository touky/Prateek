namespace Mayfair.CoreContent.Code.Tags
{
    using Prateek.KeynameFramework.Interfaces;

    public abstract partial class Tags
    {
        public interface IPropertyColor { }

        public abstract class PropertyColor     : MasterKeyword { }
        public abstract class Brown             : PropertyColor, IPropertyColor { }
        public abstract class LightBlue         : PropertyColor, IPropertyColor { }
        public abstract class Purple    : PropertyColor, IPropertyColor { }
        public abstract class Orange    : PropertyColor, IPropertyColor { }
        public abstract class Red       : PropertyColor, IPropertyColor { }
        public abstract class Yellow    : PropertyColor, IPropertyColor { }
        public abstract class Green     : PropertyColor, IPropertyColor { }
        public abstract class Blue      : PropertyColor, IPropertyColor { }
    }
}