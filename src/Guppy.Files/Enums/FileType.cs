using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guppy.Files.Enums
{
    public enum FileType
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
