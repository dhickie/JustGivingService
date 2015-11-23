using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Installer
{
    /// <summary>
    /// This builds in to an executable which installs the JustGiving service.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            String CATALINA_HOME = null;
            String JRE_HOME = null;
            String RAINMETER_SKINS_HOME = null;

            // First check the required environment variables exist
            Console.WriteLine("Performing pre-install checks...");
            bool result = CheckEnvironmentVariables(out CATALINA_HOME, out JRE_HOME, out RAINMETER_SKINS_HOME);

            // Now copy the service configuration files
            if (result)
            {
                Console.WriteLine("Installing configuration service...");
                result = InstallWebApp(CATALINA_HOME);
            }

            // Copy over the Rainmeter skin
            if (result)
            {
                Console.WriteLine("Installing rainmeter skin...");
                result = InstallRainmeterSkin(RAINMETER_SKINS_HOME);
            }

            // Install the windows service
            if (result)
            {
                Console.WriteLine("Installing windows service...");
                result = InstallService();
            }

            // Write the result of the installation
            if (result)
            {
                Console.WriteLine("Installation completed successfully!");
            }
            else
            {
                Console.WriteLine("Installation failed. See log for errors.");
            }

            // Pause at the end to allow them to review the log
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static bool CheckEnvironmentVariables(out String CATALINA_HOME, out String JRE_HOME, out String RAINMETER_SKINS_HOME)
        {
            bool result = true;

            CATALINA_HOME = String.Empty;
            JRE_HOME = String.Empty;
            RAINMETER_SKINS_HOME = String.Empty;

            try
            {
                CATALINA_HOME = Environment.GetEnvironmentVariable("CATALINA_HOME");
                JRE_HOME = Environment.GetEnvironmentVariable("JRE_HOME");
                RAINMETER_SKINS_HOME = Environment.GetEnvironmentVariable("RAINMETER_SKINS_HOME");

                if (CATALINA_HOME == null)
                {
                    Console.WriteLine("Error: Missing environment variable - CATALINA_HOME");
                    result = false;
                }
                if (JRE_HOME == null)
                {
                    Console.WriteLine("Error: Missing environment variables - JRE_HOME");
                    result = false;
                }
                if (RAINMETER_SKINS_HOME == null)
                {
                    Console.WriteLine("Error: Missing environment variable - RAINMETER_SKINS_HOME");
                    result = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }

        private static bool InstallWebApp(String CATALINA_HOME)
        {
            bool result = true;

            try
            {
                // Stop the Tomcat service
                result = RunCommand(Path.Combine(CATALINA_HOME, "bin\\tomcat8.exe"), "stop", waitForExit: true);

                // Check to see if a previous installation exists
                if (result)
                {
                    String webAppPath = Path.Combine(CATALINA_HOME, "webapps\\justgivingconfig");
                    if (Directory.Exists(webAppPath))
                    {
                        Directory.Delete(webAppPath, recursive: true);
                    }

                    // Now copy over the new files
                    String sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "ServiceConfig");
                    result = CopyDirectoryContents(sourcePath, webAppPath);

                    // Restart the Tomcat service
                    if (result)
                    {
                        result = RunCommand(Path.Combine(CATALINA_HOME, "bin\\tomcat8.exe"), "start", waitForExit: true);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }

        private static bool InstallRainmeterSkin(String RAINMETER_SKINS_HOME)
        {
            bool result = true;

            try
            {
                String rainmeterSrc = Path.Combine(Directory.GetCurrentDirectory(), "Rainmeter");
                String rainmeterDst = Path.Combine(RAINMETER_SKINS_HOME, "JustGiving");
                result = CopyDirectoryContents(rainmeterSrc, rainmeterDst);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }

        private static bool InstallService()
        {
            bool result = true;

            try
            {
                // Kill any old versions that are currently running
                foreach (Process process in Process.GetProcessesByName("JustGivingService"))
                {
                    process.Kill();
                }

                // Copy a shortcut of the new exe to the startup folder
                String exeLoc = Path.Combine(Directory.GetCurrentDirectory(), "Service\\JustGivingService.exe");
                String destDir = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
                result = CreateShortcut(exeLoc, destDir);

                // Start the executable
                if (result)
                {
                    result = RunCommand(exeLoc, String.Empty, waitForExit: false);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }

        private static bool CreateShortcut(String sourceFile, String destDir)
        {
            bool result = true;

            try
            {
                String srcFileName = Path.GetFileName(sourceFile);
                using (StreamWriter writer = new StreamWriter(String.Format("{0}.url", Path.Combine(destDir, srcFileName))))
                {
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine("URL=file:///" + sourceFile);
                    writer.WriteLine("IconIndex=0");
                    String iconLoc = sourceFile.Replace('\\', '/');
                    writer.WriteLine("IconFile=" + iconLoc);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }

        private static bool RunCommand(String exeLocation, String arguments, bool waitForExit)
        {
            bool result = true;

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = exeLocation;
                startInfo.UseShellExecute = true;
                startInfo.Arguments = arguments;

                Process process = new Process();
                process.StartInfo = startInfo;
                process.Start();
                if (waitForExit)
                {
                    process.WaitForExit();

                    if (process.ExitCode < 0)
                    {
                        Console.WriteLine(String.Format("Error: something went wrong while running the command {0} {1}", exeLocation, arguments));
                        result = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }

        private static bool CopyDirectoryContents(String sourcePath, String destPath)
        {
            bool result = true;

            try
            {
                // If the destination directory doesn't exist, then create it
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                // Copy all files in the source directory to the destination directory
                String[] files = Directory.GetFiles(sourcePath);
                foreach (String file in files)
                {
                    String fileName = Path.GetFileName(file);
                    String destFile = Path.Combine(destPath, fileName);
                    File.Copy(file, destFile, overwrite: true);
                }

                // Now recursively call this function for each subfolder
                DirectoryInfo rootDir = new DirectoryInfo(sourcePath);
                DirectoryInfo[] subDirs = rootDir.GetDirectories();
                foreach (DirectoryInfo subDir in subDirs)
                {
                    if (result)
                    {
                        String subDirPath = subDir.FullName;
                        String subDirName = Path.GetFileName(subDirPath);
                        String destDirPath = Path.Combine(destPath, subDirName);
                        result = CopyDirectoryContents(subDirPath, destDirPath);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                result = false;
            }

            return result;
        }
    }
}
