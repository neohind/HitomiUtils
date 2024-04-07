using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompressFolders
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sTargetBaseDir = "\\\\192.168.30.195\\Scan\\Manga";
            if (args.Length > 0)
            {
                DirectoryInfo targetDirInfo = new DirectoryInfo(args[0]);
                
                if(targetDirInfo.Exists)
                {
                    sTargetBaseDir = targetDirInfo.FullName;
                }
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);

            foreach (DirectoryInfo dirInfo in directoryInfo.GetDirectories())
            {
                Console.WriteLine($"Current Directory: {dirInfo.Name}");
                string sTargetPath = Path.Combine(sTargetBaseDir, dirInfo.Name);                

                DirectoryInfo targetDir = new DirectoryInfo(sTargetPath);
                if (!targetDir.Exists)
                {
                    targetDir.Create();
                }

                foreach (DirectoryInfo dirInfoSub in dirInfo.GetDirectories())
                {
                    Console.Write($"  Processing...: {dirInfoSub.Name}");

                    try
                    {
                        string sName = dirInfoSub.Name;
                        string sCompressFilename = sName + ".zip";
                        string sFullFilename = Path.Combine(dirInfo.FullName, sCompressFilename);
                        string sOldFullFilename = Path.Combine(dirInfoSub.FullName, sCompressFilename);
                        if(File.Exists(sOldFullFilename))
                        {
                            File.Move(sOldFullFilename, sFullFilename);
                        }

                        
                        FileInfo[] aryFiles = dirInfoSub.GetFiles("*.jpg");
                        foreach (FileInfo curFileInfo in aryFiles)
                        {
                            bool bReqRemove = false;
                            using (Bitmap bitmap = (Bitmap)Bitmap.FromFile(curFileInfo.FullName))
                            {
                                if (bitmap.Width > bitmap.Height)
                                {
                                    bReqRemove = true;
                                    string sPath = curFileInfo.Directory.FullName;
                                    string sFilenameOnly = Path.GetFileNameWithoutExtension(curFileInfo.Name);
                                    string sExt = Path.GetExtension(curFileInfo.Name);
                                    string sRightNewFilename = sFilenameOnly + "_1" + sExt;
                                    string sLeftNewFilename = sFilenameOnly + "_2" + sExt;

                                    Rectangle rectLeft = new Rectangle(0, 0, bitmap.Width / 2, bitmap.Height);
                                    Rectangle rectRight = new Rectangle(bitmap.Width / 2, 0, bitmap.Width / 2, bitmap.Height);

                                    string sNewFileNameLeft = Path.Combine(sPath, sLeftNewFilename);
                                    if(File.Exists(sNewFileNameLeft))
                                    {
                                        File.Delete(sNewFileNameLeft);
                                    }
                                    using (Bitmap bitmapLeft = bitmap.Clone(rectLeft, bitmap.PixelFormat))
                                    {
                                        bitmapLeft.Save(sNewFileNameLeft);
                                    }

                                    string sNewFileNameRight = Path.Combine(sPath, sRightNewFilename);
                                    if (File.Exists(sNewFileNameRight))
                                    {
                                        File.Delete(sNewFileNameRight);
                                    }
                                    using (Bitmap bitmapRight = bitmap.Clone(rectRight, bitmap.PixelFormat))
                                    {
                                        bitmapRight.Save(sNewFileNameRight);
                                    }
                                }
                            }
                            if (bReqRemove)
                                curFileInfo.Delete();
                        }


                        if (File.Exists(sFullFilename))
                        {
                            File.Delete(sFullFilename);
                        }

                        string sAgrg = $"a -tzip \"{sFullFilename}\" \"{dirInfoSub.FullName}\\*.*\"";

                        ProcessStartInfo processStartInfo = new ProcessStartInfo("7z.exe", sAgrg);
                        processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        Process process = Process.Start(processStartInfo);
                        process.WaitForExit();


                        string sTargetFullFilename = Path.Combine(targetDir.FullName, sCompressFilename);
                        FileInfo targetFileInfo = new FileInfo(sTargetFullFilename);
                        if (File.Exists(sTargetFullFilename))
                        {
                            FileInfo sourceFileInfo = new FileInfo(sFullFilename);
                            if (sourceFileInfo.Length != targetFileInfo.Length)
                            {
                                File.Delete(sTargetFullFilename);
                            }
                        }
                            
                        File.Move(sFullFilename, sTargetFullFilename);

                        Console.WriteLine($" ->  Process Complete");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
