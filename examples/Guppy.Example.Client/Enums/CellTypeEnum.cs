namespace Guppy.Example.Client.Enums
{
    [Flags]
    public enum CellTypeEnum
    {
        Null = 0,
        Air = 1,
        Sand = 2,
        Water = 4,
        Concrete = 8,
        Plant = 16
    }
}
