using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using OSPlatformStruct = System.Runtime.InteropServices.OSPlatform;

namespace Guppy.Core.Common.Helpers
{
    public static class NativeHelper
    {
        public static OSPlatform? OSPlatform { get; }
        public static string[] Extensions { get; }

        static NativeHelper()
        {
            var platformExtensions = new Dictionary<OSPlatformStruct, string[]>()
            {
                [OSPlatformStruct.Windows] = new[] { "dll" },
                [OSPlatformStruct.Linux] = new[] { "so" }
            };

            if (NativeHelper.GetOSPlatformExtensions(platformExtensions, out var platform, out var extensions))
            {
                NativeHelper.OSPlatform = platform;
                NativeHelper.Extensions = extensions!;

                return;
            }

            NativeHelper.OSPlatform = null;
            NativeHelper.Extensions = Array.Empty<string>();
        }

        public static void Load(string directory, params string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                foreach (string extension in NativeHelper.Extensions)
                {
                    string path = Path.Combine(directory, $"{fileName}.{extension}");

                    if (File.Exists(path) == false)
                    {
                        throw new FileNotFoundException(path);
                    }

                    try
                    {
                        NativeLibrary.Load(path);
                    }
                    catch (Exception ex)
                    {
                        throw GuppyLogger.LogException($"Exception loading native library {path}", ex);
                    }
                }
            }
        }

        private static bool GetOSPlatformExtensions(
            Dictionary<OSPlatformStruct, string[]> platformExtensions,
            [MaybeNullWhen(false)] out OSPlatformStruct? platform,
            [MaybeNullWhen(false)] out string[]? extensions)
        {
            foreach ((OSPlatform platformVal, string[] extensionsVal) in platformExtensions)
            {
                if (RuntimeInformation.IsOSPlatform(platformVal))
                {
                    platform = platformVal;
                    extensions = extensionsVal;

                    return true;
                }
            }

            platform = default;
            extensions = default;

            return false;
        }
    }
}
