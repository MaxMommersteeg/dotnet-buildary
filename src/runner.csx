#r "nuget:PowerArgs, 3.0.0"
#load "nuget:simple-targets-csx, 6.0.0"

public static class Runner
{
    public static T ParseOptions<T>(IList<string> args) where T: BuildOptions
    {
        T options = null;

        try
        {
            options = PowerArgs.Args.Parse<T>(args.ToArray());
        }
        catch (PowerArgs.ArgException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(PowerArgs.ArgUsage.GenerateUsageFromTemplate<T>());
            System.Environment.Exit(1);
        }

        if(options == null)
        {
            // It was a help command (-help);
            System.Environment.Exit(0);
        }

        return options;
    }

    public static void Run(BuildOptions options, IDictionary<string, SimpleTargets.Target> targets)
    {
        SimpleTargetsRunner.Run(new string[]{ options.Target }, targets, Console.Out);
    }

    public class BuildOptions
    {
        [PowerArgs.HelpHook, PowerArgs.ArgShortcut("-?")]
        public bool Help { get; set; }

        [PowerArgs.ArgDefaultValue("default"), PowerArgs.ArgPosition(0)]
        public string Target { get; set; }
    }
}