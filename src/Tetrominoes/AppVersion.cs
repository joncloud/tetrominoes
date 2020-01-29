using System;

namespace Tetrominoes
{
    public static class AppVersion
    {
        public static readonly string BuildDate = DateTime.UtcNow.ToString("s");
        public const string GitRef = "local";

        public static string Display { get; } = $"Version {GitRef} on {BuildDate}";
    }
}
