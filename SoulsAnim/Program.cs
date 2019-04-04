using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace SoulsAnim
{
    class Program
    {
        const string HKX_CMD        = "hkxcmd.exe";
        const string FILES_DIR      = @"files\";
        const string KEYBOARD_MASH  = "SSFADF.exe";
        const string HKX_SKELETON   = FILES_DIR + "skeleton.hkx";
        const string FILE_PATTERN   = "*.hkx";
        const string OUT_PATTERN    = "*-out*";

        const string HK_CLASSVER    = "8";
        const string HK_CONTVER     = "hk_2010.2.0-r1";

        static List<string> missingFiles = new List<string>();

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.White;

            if (!Verify(HKX_CMD)) missingFiles.Add(HKX_CMD);
            if (!Verify(FILES_DIR + KEYBOARD_MASH)) missingFiles.Add(FILES_DIR + KEYBOARD_MASH);
            if (!Verify(HKX_SKELETON)) missingFiles.Add(HKX_SKELETON);

            if (missingFiles.Count > 0) BreakOperation();

            HandleFiles();
            ConvertFiles();
            DamnHavok();
            MoveNewFiles();

            Console.WriteLine("\nFinished. New files are in files/Out folder.\nPress the any key to close.");
            Console.ReadKey();
        }

        static void HandleFiles()
        {
            var filesPath = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIR);

            string[] files = Directory.GetFiles(filesPath, FILE_PATTERN);

            for (var i = 0; i < files.Length; i++)
            {
                FixRootNode(files[i]);
            }
        }

        static void ConvertFiles()
        {
            var extProc = new ProcessStartInfo(Path.Combine(Directory.GetCurrentDirectory(), HKX_CMD))
            {
                Arguments = "Convert " + FILES_DIR,
                UseShellExecute = false
            };

            var extStart = Process.Start(extProc);
            extStart.WaitForExit();
            extStart.Close();
        }

        static void DamnHavok()
        {
            var mashProc = new ProcessStartInfo(Path.Combine(Directory.GetCurrentDirectory(), FILES_DIR + KEYBOARD_MASH))
            {
                Arguments = "",
                UseShellExecute = false,
                WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIR)
            };

            var extStart = Process.Start(mashProc);
            extStart.WaitForExit();
            extStart.Close();
        }

        static void MoveNewFiles()
        {
            var outDir = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIR + "Out");
            var fileDir = Path.Combine(Directory.GetCurrentDirectory(), FILES_DIR);

            Directory.CreateDirectory(outDir);

            string[] files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FILES_DIR), OUT_PATTERN);

            for (var i = 0; i < files.Length; i++)
            {
                try
                {
                    var from = Path.Combine(fileDir, Path.GetFileName(files[i]));
                    var to = Path.Combine(outDir, Path.GetFileName(files[i]));

                    if (File.Exists(to)) File.Delete(to);

                    File.Move(from, to);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }
            }

        }

        static void FixRootNode(string path)
        {
            var doc = XDocument.Load(path);

            foreach (var element in doc.Descendants("hkpackfile"))
            {
                element.Attribute("classversion").Value = HK_CLASSVER;
                element.Attribute("contentsversion").Value = HK_CONTVER;
                if (element.Attribute("maxpredicate") != null) element.Attribute("maxpredicate").Remove();
                if (element.Attribute("maxpredicate") != null)  element.Attribute("predicates").Remove();
            }

            doc.Save(path);

            Console.WriteLine("Fixed " + Path.GetFileName(path));
        }

        static void BreakOperation()
        {
            for (var i = 0; i < missingFiles.Count; i++)
            {
                Console.WriteLine("Missing " + missingFiles[i] + " from directory");
            }

            Console.WriteLine("\nPress the any key to close.");
            Console.ReadKey();
        }

        static bool Verify(string path)
        {
            return File.Exists(path);
        }
    }
}
