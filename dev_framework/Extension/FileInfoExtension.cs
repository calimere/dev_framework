using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class FileInfoExtension
{
    public static bool IsFileLocked(this FileInfo fileInfo)
    {
        try
        {
            using (FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                stream.Close();
        }
        catch (IOException) { 
            return true; 
        }
        return false;
    }
}