using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Services;

namespace AddressLookup
{
	/// <summary>
	/// Summary description for Service1.
	/// </summary>
	public class Service1 : System.Web.Services.WebService
	{
		private SqlConnection DBConnection;

		public Service1()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();

			try
			{
				string szConn = "server=nantz;uid=sa;pwd=;database=CSAddress";
				DBConnection = new SqlConnection(szConn);

				string szSelectQuery = "SELECT State FROM USZips WHERE Zip = -1";
				SqlCommand SCommand = new SqlCommand(szSelectQuery, DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				sqlDR.Close();
				DBConnection.Close();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		[WebMethod] public string GetStateFromZip(string szZip)
		{
			string szReturn="";

			try
			{
				if(szZip.Length<=0)
					return "";

				string szSelectQuery = "SELECT State FROM USZips WHERE Zip = " + szZip;
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);

				if(sqlDR.Read())
					szReturn = sqlDR.GetString(0);				

				sqlDR.Close();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}

			return szReturn;
		}

		[WebMethod] public string GetCityFromZip(string szZip)
		{
			string szReturn="";

			try
			{
				if(szZip.Length<=0)
					return "";

				string szSelectQuery = "SELECT City FROM USZips WHERE Zip = " + szZip;
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				if(sqlDR.Read())
					szReturn = sqlDR.GetString(0);
				
				sqlDR.Close();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}
				
			return szReturn;
		}

		[WebMethod] public ArrayList GetCountryNames( )
		{
			ArrayList arList = new ArrayList();
			try
			{
				string szSelectQuery = "SELECT SzCountryName FROM Countries";	
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				while(sqlDR.Read())
				{
					if(arList.IndexOf(sqlDR.GetString(0))==-1)
						arList.Add(sqlDR.GetString(0));
				}

				sqlDR.Close();
				arList.Sort();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}

			return arList;
		}
		
		[WebMethod] public ArrayList GetStatesFromCity(string szCity)
		{
			ArrayList arList = new ArrayList();
			try
			{
				string szSelectQuery = "SELECT State FROM USZips WHERE City = '" + szCity + "'";	
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				while(sqlDR.Read())
				{
					if(arList.IndexOf(sqlDR.GetString(0))==-1)
						arList.Add(sqlDR.GetString(0));
				}

				sqlDR.Close();
				arList.Sort();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}

			return arList;
		}

		[WebMethod] public ArrayList GetZipsFromCityState(string szCity, string szState)
		{
			ArrayList arList = new ArrayList();

			try
			{
				string szSelectQuery = "SELECT Zip FROM USZips WHERE City = '" + szCity + "'" + " AND State = '" + szState + "'";	
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				while(sqlDR.Read())
				{
					if(arList.IndexOf(sqlDR.GetString(0))==-1)
						arList.Add(sqlDR.GetString(0));
				}
				
				sqlDR.Close();
				arList.Sort();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}
			
			return arList;
		}

		[WebMethod] public ArrayList GetPartialZips(string szZip)
		{
			ArrayList arList = new ArrayList();

			try
			{
				string szSelectQuery = "SELECT Zip FROM USZips WHERE Zip LIKE ('" + szZip + "%')";	
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				while(sqlDR.Read())
				{
					if(arList.IndexOf(sqlDR.GetString(0))==-1)
						arList.Add(sqlDR.GetString(0));
				}
				
				sqlDR.Close();
				arList.Sort();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}

			return arList;
		}

		[WebMethod] public bool DoesZipExist(string szZip)
		{
			bool bReturn=false;

			try
			{
				if(szZip.Length<=0)
					return false;

				string szSelectQuery = "SELECT Zip FROM USZips WHERE Zip = '" + szZip + "'";
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				if(sqlDR.Read())
					bReturn = true;
				
				sqlDR.Close();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}

			return bReturn;
		}

		[WebMethod] public ArrayList GetCities(string szCity)
		{
			ArrayList arList = new ArrayList();

			try
			{
				string szSelectQuery = "SELECT City FROM USZips WHERE City LIKE ('" + szCity + "%') OR SOUNDEX(City) = SOUNDEX('" + szCity + "')";
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				while(sqlDR.Read())
				{
					if(arList.IndexOf(sqlDR.GetString(0))==-1)
						arList.Add(sqlDR.GetString(0));
				}
				
				sqlDR.Close();
				arList.Sort();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}

			return arList;
		}

		[WebMethod] public bool CityExists(string szCity)
		{
			bool bRetVal=false;

			try
			{
				if(szCity.Length<=0)
					return false;

				string szSelectQuery = "SELECT City FROM USZips WHERE City = '" + szCity + "'";
				SqlCommand SCommand = new SqlCommand(szSelectQuery,DBConnection);
				SCommand.Connection.Open();
				SqlDataReader sqlDR = SCommand.ExecuteReader(CommandBehavior.CloseConnection);
				
				if(sqlDR.Read())
					bRetVal=true;
				
				sqlDR.Close();
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Exception caught." + ex.ToString());
				throw ex;
			}

			return bRetVal;
		}
	}
}
