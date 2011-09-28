using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Archivist.IO
{
   /// <summary>
   /// Provides common validation and rooting of paths.
   /// </summary>
   public static class ValidatedPath
   {
      [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      static extern uint GetShortPathName(
         [MarshalAs(UnmanagedType.LPTStr)] string lpszLongPath
         , [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszShortPath
         , uint cchBuffer
         );

      /// <summary>
      /// Ensures the given path is rooted.
      /// </summary>
      /// <remarks>
      /// If the given path is not rooted, then roots it using either:
      /// <list type="ordered">
      /// <item>The System.AppDomain.CurrentDomain.SetupInformation.PrivateBinPath if not null.</item>
      /// <item>The System.AppDomain.CurrentDomain.BaseDirectory otherwise.</item>
      /// </list>
      /// This ensures that for web applications, the most appropriate path is always used for 
      /// Web applications (PrivateBinPath) and Unit Testing, Console and WinForm applications (BaseDirectory).
      /// </remarks>
      /// <param name="path"></param>
      /// <returns>The given path, if rooted, otherwise a path rooted as described above.</returns>
      public static string AbsolutePath(string path)
      {
         string rootedPath = path;

         if (!Path.IsPathRooted(path))
         {
            string basePath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath != null ?
               AppDomain.CurrentDomain.SetupInformation.PrivateBinPath :
               AppDomain.CurrentDomain.BaseDirectory;

            rootedPath = Path.Combine(basePath, path);
         }

         return rootedPath;
      }

      /// <summary>
      /// Ensures the given path is rooted and is in 8.3 format.
      /// </summary>
      /// <remarks>
      /// Uses <see cref="Archivist.IO.ValidatedPath.AbsolutePath"/> to ensure the path is absolute.
      /// Uses the Win32 GetShortPathName API to shorten the path.
      /// </remarks>
      /// <param name="path"></param>
      /// <returns>The given path, if rooted, otherwise a path rooted as described by <see cref="Archivist.IO.ValidatedPath.AbsolutePath"/>.</returns>
      /// <seealso href="http://www.pinvoke.net/default.aspx/kernel32.GetShortPathName">PInvoke</seealso> GetShortPathName reference.
      /// <seealso href="http://msdn2.microsoft.com/en-us/library/aa364989.aspx">MSDN</seealso> GetShortPathName reference.
      public static string AbsoluteShortPath(string path)
      {
         string absolutePath = ValidatedPath.AbsolutePath(path);

         StringBuilder shortNameBuffer = new StringBuilder(256);
         uint bufferSize = (uint)shortNameBuffer.Capacity;

         GetShortPathName(absolutePath, shortNameBuffer, bufferSize);

         string shortPath = shortNameBuffer.ToString();

         if (shortPath == string.Empty)
         {
            throw new InvalidPathException(string.Format("{0} could not be shortened. The path must exist for it to be shortened.", absolutePath));
         }

         return shortPath;
      }

      /// <summary>
      /// Ensures the given path points to a folder that already exists.
      /// </summary>
      /// <param name="path"></param>
      /// <returns>The given path, if rooted, otherwise a path rooted as described by <see cref="Archivist.IO.ValidatedPath.AbsolutePath"/>.</returns>
      public static string ExistingFolderPath(string path)
      {
         string absolutePath = ValidatedPath.AbsolutePath(path);

         if (!Directory.Exists(absolutePath))
         {
            throw new DirectoryNotFoundException(string.Format("Directory does not exist: {0}", absolutePath));
         }

         return absolutePath;
      }

      /// <summary>
      /// Ensures the given path points to a file that already exists.
      /// </summary>
      /// <param name="path"></param>
      /// <returns>The given path, if rooted, otherwise a path rooted as described by <see cref="Archivist.IO.ValidatedPath.AbsolutePath"/>.</returns>
      public static string ExistingFilePath(string path)
      {
         string absolutePath = ValidatedPath.AbsolutePath(path);

         if (!File.Exists(absolutePath))
         {
            throw new FileNotFoundException(string.Format("The file does not exist: {0}", absolutePath), absolutePath);
         }

         return absolutePath;
      }
   }

}
