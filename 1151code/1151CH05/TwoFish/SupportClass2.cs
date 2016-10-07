using System;
public class SupportClass
{
	public static System.Object PutElement(System.Collections.Hashtable hashTable, System.Object key, System.Object newValue)
	{
		System.Object element = hashTable[key];
		hashTable[key] = newValue;
		return element;
	}

	/*******************************/
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

}
