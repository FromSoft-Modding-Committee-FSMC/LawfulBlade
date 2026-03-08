namespace LawfulBladeSDK.Generator
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class GeneratorPropertyTooltipAttribute : Attribute
    {
        public string Tooltip { get; }

        public GeneratorPropertyTooltipAttribute(string tooltip)
        {
            Tooltip = tooltip;
        }
    }
}
