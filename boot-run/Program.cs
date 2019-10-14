using Microsoft.Win32.TaskScheduler;
using System;
using System.Security.Principal;

namespace boot_run
{
    class Program
    {
        static double systemVersion = Convert.ToDouble(Environment.OSVersion.Version.Major + "." + Environment.OSVersion.Version.Minor);
        public static bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            //WindowsBuiltInRole可以枚举出很多权限，例如系统用户、User、Guest等等
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        static void Main(string[] args)
        {
            if (systemVersion < 5.1)
            {
                Console.WriteLine("Minimum system support for Windows XP.");
                Console.ReadLine();
                Environment.Exit(0);
            }

            if (IsAdministrator())
            {

                if (args.Length >= 2)
                {
                    // Display version and server state
                    Version ver = TaskService.Instance.HighestSupportedVersion;
                    bool newVer = (ver >= new Version(1, 2));

                    string appName = args[0];
                    string exePath = args[1];
                    string arguments = args.Length > 2 ? args[2] : null;

                    // Create a new task definition and assign properties
                    TaskDefinition td = TaskService.Instance.NewTask();
                    td.RegistrationInfo.Description = appName + " set by boot-run";
                    td.Settings.DisallowStartIfOnBatteries = false;
                    td.Settings.ExecutionTimeLimit = TimeSpan.Zero;

                    if (newVer)
                    {
                        td.Principal.RunLevel = TaskRunLevel.Highest;
                    }

                    // Create a trigger that will fire the task at this time every other day
                    LogonTrigger lt = new LogonTrigger();
                    lt.UserId = null;
                    lt.Enabled = true;
                    td.Triggers.Add(lt);

                    // Create an action that will launch Notepad whenever the trigger fires
                    td.Actions.Add(new ExecAction(exePath, arguments));

                    // Register the task in the root folder
                    TaskService.Instance.RootFolder.RegisterTaskDefinition(appName, td);
                }
                else
                {
                    Console.WriteLine("Use: boot-run.exe \"AppName\" \"ExeFilePath\" \"Arguments\"");
                    Console.WriteLine("Example: boot-run.exe \"NotePad\" \"C:\\Windows\\System32\\notepad.exe\" \"C:\\test.log\"");
                    Console.ReadLine();
                    Environment.Exit(0);
                }
            } else
            {
                Console.WriteLine("You need to be administrator to perform this command.");
                Console.WriteLine("请以管理员权限运行此程序");

                Console.ReadLine();
            }
        }
    }
}
