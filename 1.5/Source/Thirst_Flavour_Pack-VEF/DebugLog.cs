using System.Diagnostics;
using System;

namespace Thirst_Flavour_Pack.VEF;

static class ModLog
{
    [Conditional("DEBUG")]
    public static void Debug(string x)
    {
        Verse.Log.Message(x);
    }

    public static void Log(string msg)
    {
        Verse.Log.Message($"<color=#60b1c7>[Thirst_Flavour_Pack.VEF]</color> {msg ?? "<null>"}");
    }

    public static void Warn(string msg)
    {
        Verse.Log.Warning($"<color=#60b1c7>[Thirst_Flavour_Pack.VEF]</color> {msg ?? "<null>"}");
    }

    public static void Error(string msg, Exception e = null)
    {
        Verse.Log.Error($"<color=#60b1c7>[Thirst_Flavour_Pack.VEF]</color> {msg ?? "<null>"}");
        if (e != null)
            Verse.Log.Error(e.ToString());
    }

}
