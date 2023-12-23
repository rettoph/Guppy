namespace Guppy.Resources
{
    internal interface ISettingValue
    {
        Setting Setting { get; }
        object Value { get; set; }
    }
}
