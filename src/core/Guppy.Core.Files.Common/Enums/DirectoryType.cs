namespace Guppy.Core.Files.Common.Enums
{
    public enum DirectoryType
    {
        /// <summary>
        /// Defaults the path root to %appdata%/GAME_NAME
        /// </summary>
        AppData,

        /// <summary>
        /// Defaults path root to game install location
        /// </summary>
        CurrentDirectory,

        /// <summary>
        /// Assumes a full path is given
        /// </summary>
        Source
    }
}
