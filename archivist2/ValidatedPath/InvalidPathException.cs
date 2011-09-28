using System;

namespace Archivist.IO
{
   /// <summary>
   /// An exception to throw when a given path is invalid (not rooted, or cannot be created).
   /// </summary>
   [Serializable]
   public class InvalidPathException : Exception
   {
      /// <summary>
      /// Default constructors.
      /// </summary>
      /// <param name="message"></param>
      public InvalidPathException(string message)
         : base(message)
      {
      }

      /// <summary>
      /// Default constructors.
      /// </summary>
      /// <param name="message"></param>
      /// <param name="innerException"></param>
      public InvalidPathException(string message, Exception innerException)
         : base(message, innerException)
      {
      }
   }
}
