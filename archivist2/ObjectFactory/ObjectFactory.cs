namespace Archivist.ObjectFactory
{
   using log4net;
   using Spring.Context;
   using Spring.Context.Support;
   using System.IO;

   /// <summary>
   /// Singleton for providing global access to objects within the Spring context.
   /// <remarks>
   /// On first access, loads the context from a file named Objects.xml, located in the AppDomain's base path.
   /// </remarks>
   /// </summary>
   public sealed class ObjectFactory
   {
      private IApplicationContext ctx = null;
      private static readonly ObjectFactory instance = new ObjectFactory();

      #region Instrumentation
      private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      #endregion

      /// <summary>
      /// Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
      /// </summary>
      static ObjectFactory()
      {
      }

      /// <summary>
      /// Prevent public construction.
      /// </summary>
      private ObjectFactory()
      {

      }

      /// <summary>
      /// Expose access to the single instance.
      /// </summary>
      public static ObjectFactory Instance
      {
         get
         {
            #region Instrumentation
            if (log.IsInfoEnabled)
            {
               log.Info(System.Reflection.MethodInfo.GetCurrentMethod().Name);
            }
            #endregion

            instance.Initialise("Objects.xml");
            return instance;
         }
      }

      /// <summary>
      /// Initialise the one and only context with the path to a valid context file.
      /// </summary>
      private void Initialise(string contextFilePath)
      {
		  if (!File.Exists(contextFilePath))
		  {
			  throw new FileNotFoundException(string.Format("The file does not exist: {0}", contextFilePath), contextFilePath);
		  }

         #region Instrumentation
         if (log.IsInfoEnabled)
         {
            log.InfoFormat("{0}: contextFilePath is {1}", System.Reflection.MethodInfo.GetCurrentMethod().Name, contextFilePath);
         }
         #endregion

         if (ctx == null)
         {
            ctx = new XmlApplicationContext(new string[] { contextFilePath });
         }
      }

      /// <summary>
      /// Get an object of the given type from the object context.
      /// </summary>
      public object GetObject(string objectName)
      {
         if (log.IsInfoEnabled) log.Info(System.Reflection.MethodInfo.GetCurrentMethod().Name);

         return ctx.GetObject(objectName);
      }
   }
}
