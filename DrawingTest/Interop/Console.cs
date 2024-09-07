using Dalamud.Plugin.Services;
using HaselCommon;

namespace DrawingTest.Interop;

public static class Console
{
    public static void log(string message) => Service.Get<IPluginLog>().Information(message);
    public static void info(string message) => Service.Get<IPluginLog>().Information(message);
    public static void warn(string message) => Service.Get<IPluginLog>().Warning(message);
    public static void error(string message) => Service.Get<IPluginLog>().Error(message);
    public static void debug(string message) => Service.Get<IPluginLog>().Debug(message);
    public static void trace(string message) => Service.Get<IPluginLog>().Verbose(message);
}
