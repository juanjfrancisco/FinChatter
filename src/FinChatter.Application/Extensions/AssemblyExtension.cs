using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinChatter.Application.Extensions
{
    public static class AssemblyExtension
    {
        public static DateTime GetCreationTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            var creationTime = File.GetLastWriteTimeUtc(filePath);

            var tz = target ?? TimeZoneInfo.Local;
            creationTime = TimeZoneInfo.ConvertTimeFromUtc(creationTime, tz);

            return creationTime;
        }
    }
}
