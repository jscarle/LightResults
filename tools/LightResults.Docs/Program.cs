using System.Reflection;
using System.Runtime.Versioning;
using XmlDocMarkdown.Core;

var targetFramework = GetTargetFramework();

return XmlDocMarkdownApp.Run(["LightResults", $@"..\..\..\..\..\docs\{targetFramework}"]);

string GetTargetFramework()
{
    var targetFrameworkName = ((TargetFrameworkAttribute?)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(TargetFrameworkAttribute), false).SingleOrDefault())?.FrameworkName ?? "";
    return targetFrameworkName switch
    {
        ".NETFramework,Version=v4.8.1" => "netstandard2.0",
        ".NETCoreApp,Version=v6.0" => "net6.0",
        ".NETCoreApp,Version=v7.0" => "net7.0",
        ".NETCoreApp,Version=v8.0" => "net8.0",
        _ => ""
    };
} 
