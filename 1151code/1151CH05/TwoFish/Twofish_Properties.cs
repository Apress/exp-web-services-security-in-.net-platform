// $Id: $
//
// $Log: $
// Revision 1.0  1998/03/24  raif
// + start of history.
//
// $Endlog$
/*
* Copyright (c) 1997, 1998 Systemics Ltd on behalf of
* the Cryptix Development Team. All rights reserved.
*/
namespace Twofish
{
	using System;
   using System.IO;
	
	/// <summary> This class acts as a central repository for an algorithm specific
	/// properties. It reads an (algorithm).properties file containing algorithm-
	/// specific properties. When using the AES-Kit, this (algorithm).properties
	/// file is located in the (algorithm).jar file produced by the "jarit" batch/
	/// script command.<p>
	/// *
	/// <b>Copyright</b> &copy; 1997, 1998
	/// <a href="http://www.systemics.com/">Systemics Ltd</a> on behalf of the
	/// <a href="http://www.systemics.com/docs/cryptix/">Cryptix Development Team</a>.
	/// <br>All rights reserved.<p>
	/// *
	/// <b>$Revision: $</b>
	/// </summary>
	/// <author>  David Hopwood
	/// </author>
	/// <author>  Jill Baker
	/// </author>
	/// <author>  Raif S. Naffah
	/// 
	/// </author>
	public class Twofish_Properties
	{
		static Twofish_Properties()
		{
			{
				if (GLOBAL_DEBUG)
					System.Console.Error.WriteLine(">>> " + NAME + ": Looking for " + ALGORITHM + " properties");
				System.String it = ALGORITHM + ".properties";
				//UPGRADE_ISSUE: Method 'java.lang.Class.getResourceAsStream' was not converted. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1000_javalangClassgetResourceAsStream_javalangString"'
				System.IO.Stream is_Renamed = File.Open(it, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				bool ok = is_Renamed != null;
				if (ok)
					try
					{
						//UPGRADE_ISSUE: Method 'java.util.Properties.load' was not converted. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1000_javautilPropertiesload_javaioInputStream"'
//						properties.load(is_Renamed);
						is_Renamed.Close();
						if (GLOBAL_DEBUG)
							System.Console.Error.WriteLine(">>> " + NAME + ": Properties file loaded OK...");
					}
					catch (System.Exception x)
					{
						ok = false;
					}
				if (!ok)
				{
					if (GLOBAL_DEBUG)
						System.Console.Error.WriteLine(">>> " + NAME + ": WARNING: Unable to load \"" + it + "\" from CLASSPATH.");
					if (GLOBAL_DEBUG)
						System.Console.Error.WriteLine(">>> " + NAME + ":          Will use default values instead...");
//					int n = DEFAULT_PROPERTIES.Length;
//					 for (int i = 0; i < n; i++)
//						PutElement(properties, DEFAULT_PROPERTIES[i][0], DEFAULT_PROPERTIES[i][1]);
					if (GLOBAL_DEBUG)
						System.Console.Error.WriteLine(">>> " + NAME + ": Default properties now set...");
				}
			}
		}
		internal static System.IO.StreamWriter Output
		{
			get
			{
				System.IO.StreamWriter pw;
				System.String name = getProperty("Output");
				if (name != null && name.Equals("out"))
				{
					System.IO.StreamWriter temp_writer;
					//UPGRADE_ISSUE: The equivalent of field java.lang.System.out is incompatible with the expected type in C#. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1109"'
               temp_writer = new System.IO.StreamWriter(System.Console.OpenStandardOutput());
					temp_writer.AutoFlush = true;
					pw = temp_writer;
				}
				else
				{
					System.IO.StreamWriter temp_writer2;
					//UPGRADE_ISSUE: The equivalent of field java.lang.System.err is incompatible with the expected type in C#. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1109"'
					temp_writer2 = new System.IO.StreamWriter(System.Console.OpenStandardError());
					temp_writer2.AutoFlush = true;
					pw = temp_writer2;
				}
				return pw;
			}
			
		}
		// implicit no-argument constructor
		// Constants and variables with relevant static code
		//...........................................................................
		
		internal const bool GLOBAL_DEBUG = false;
		
		internal const System.String ALGORITHM = "Twofish";
		internal const double VERSION = 0.2;
		internal static System.String FULL_NAME = ALGORITHM + " ver. " + VERSION;
		internal const System.String NAME = "Twofish_Properties";
		
		//UPGRADE_TODO: Format of property file may need to be changed. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1089"'
		internal static System.Configuration.AppSettingsReader properties = new System.Configuration.AppSettingsReader();
		
		/// <summary>Default properties in case .properties file was not found. 
		/// </summary>
		private static System.String[][] DEFAULT_PROPERTIES = {new System.String[]{"Trace.Twofish_Algorithm", "true"}, new System.String[]{"Debug.Level.*", "1"}, new System.String[]{"Debug.Level.Twofish_Algorithm", "9"}};
		
		
		
		// Properties methods (excluding load and save, which are deliberately not
		// supported).
		//...........................................................................
		
		/// <summary>Get the value of a property for this algorithm. 
		/// </summary>
		public static System.String getProperty(System.String key)
		{
			//UPGRADE_WARNING: method 'java.util.Properties.getProperty' was converted to ' ' which may throw an exception. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1101"'
			return (System.String) properties.GetValue(key, System.Type.GetType("System.String"));
		}
		
		/// <summary> Get the value of a property for this algorithm, or return
		/// <i>value</i> if the property was not set.
		/// </summary>
		public static System.String getProperty(System.String key, System.String value_Renamed)
		{
			return GetValue(key, value_Renamed);
		}
		
		/*
      /// <summary>List algorithm properties to the PrintStream <i>out</i>. 
		/// </summary>
		public static void  list(System.IO.StreamWriter out_Renamed)
		{
			System.IO.StreamWriter temp_writer;
			temp_writer = new System.IO.StreamWriter(out_Renamed);
			temp_writer.AutoFlush = true;
			list(temp_writer);
		}
      */
		
		/// <summary>List algorithm properties to the PrintWriter <i>out</i>. 
		/// </summary>
		public static void  list(System.IO.StreamWriter out_Renamed)
		{
			out_Renamed.WriteLine("#");
			out_Renamed.WriteLine("# ----- Begin " + ALGORITHM + " properties -----");
			out_Renamed.WriteLine("#");
			System.String key, value_Renamed;
			System.Collections.Specialized.NameValueCollection temp_namedvaluecollection;
			temp_namedvaluecollection = System.Configuration.ConfigurationSettings.AppSettings;
			System.Collections.IEnumerator enum_Renamed = temp_namedvaluecollection.GetEnumerator();
			//UPGRADE_TODO: method 'java.util.Enumeration.hasMoreElements' was converted to ' ' which has a different behavior. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1073_javautilEnumerationhasMoreElements"'
			while (enum_Renamed.MoveNext())
			{
				//UPGRADE_TODO: method 'java.util.Enumeration.nextElement' was converted to ' ' which has a different behavior. 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="jlca1073_javautilEnumerationnextElement"'
				key = (System.String) enum_Renamed.Current;
				value_Renamed = getProperty(key);
				out_Renamed.WriteLine(key + " = " + value_Renamed);
			}
			out_Renamed.WriteLine("#");
			out_Renamed.WriteLine("# ----- End " + ALGORITHM + " properties -----");
		}
		
		//    public synchronized void load(InputStream in) throws IOException {}
		
		public static System.Collections.IEnumerator propertyNames()
		{
			System.Collections.Specialized.NameValueCollection temp_namedvaluecollection;
			temp_namedvaluecollection = System.Configuration.ConfigurationSettings.AppSettings;
			return temp_namedvaluecollection.GetEnumerator();
		}
		
		//    public void save (OutputStream os, String comment) {}
		
		
		// Developer support: Tracing and debugging enquiry methods (package-private)
		//...........................................................................
		
		/// <summary> Return true if tracing is requested for a given class.<p>
		/// *
		/// User indicates this by setting the tracing <code>boolean</code>
		/// property for <i>label</i> in the <code>(algorithm).properties</code>
		/// file. The property's key is "<code>Trace.<i>label</i></code>".<p>
		/// *
		/// </summary>
		/// <param name="label"> The name of a class.
		/// </param>
		/// <returns>True iff a boolean true value is set for a property with
		/// the key <code>Trace.<i>label</i></code>.
		/// 
		/// </returns>
		internal static bool isTraceable(System.String label)
		{
			System.String s = getProperty("Trace." + label);
			if (s == null)
				return false;
			return s.ToUpper().Equals("TRUE");
		}
		
		/// <summary> Return the debug level for a given class.<p>
		/// *
		/// User indicates this by setting the numeric property with key
		/// "<code>Debug.Level.<i>label</i></code>".<p>
		/// *
		/// If this property is not set, "<code>Debug.Level.*</code>" is looked up
		/// next. If neither property is set, or if the first property found is
		/// not a valid decimal integer, then this method returns 0.
		/// *
		/// </summary>
		/// <param name="label"> The name of a class.
		/// </param>
		/// <returns> The required debugging level for the designated class.
		/// 
		/// </returns>
		internal static int getLevel(System.String label)
		{
			System.String s = getProperty("Debug.Level." + label);
			if (s == null)
			{
				s = getProperty("Debug.Level.*");
				if (s == null)
					return 0;
			}
			try
			{
				return System.Int32.Parse(s);
			}
			catch (System.FormatException e)
			{
				return 0;
			}
		}

      /*
      public static System.Object PutElement(System.Collections.Hashtable hashTable, System.Object key, System.Object newValue)
      {
         System.Object element = hashTable[key];
         hashTable[key] = newValue;
         return element;
      }
      */

      public static string GetValue(string key, string defaultValue)
      {
         System.Configuration.AppSettingsReader tempSettings = new System.Configuration.AppSettingsReader();
         try
         {
            return (string)tempSettings.GetValue(key, System.Type.GetType("System.String"));
         }
         catch (System.Exception exception)
         {
            return defaultValue;
         }			
      }
		
		/// <summary> Return the PrintWriter to which tracing and debugging output is to
		/// be sent.<p>
		/// *
		/// User indicates this by setting the property with key <code>Output</code>
		/// to the literal <code>out</code> or <code>err</code>.<p>
		/// *
		/// By default or if the set value is not allowed, <code>System.err</code>
		/// will be used.
		/// </summary>
	}
}