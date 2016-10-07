using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Web.Security;

namespace FormsAuthDB
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class WebForm1 : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.TextBox TextBox1;
		protected System.Web.UI.WebControls.TextBox TextBox2;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.Label ErrorLabel;
		protected System.Web.UI.WebControls.Label Label2;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button1_Click(object sender, System.EventArgs e)
		{
			// Initialize FormsAuthentication (reads the configuration and gets 
			// the cookie values and encryption keys for the given application)
			FormsAuthentication.Initialize();
  
			// Create connection and command objects
			SqlConnection conn = 
				new SqlConnection("Data Source=PETER;Database=Northwind;User ID=sa;password=;");
			conn.Open();
				//new SqlConnection("Data Source=PETER;Database=Northwind;Integrated Security=yes");
			SqlCommand cmd = conn.CreateCommand();
			cmd.CommandText = "SELECT roles FROM Employees WHERE username=@username " +
				"AND password=@password";
 
			// Fill our parameters
			cmd.Parameters.Add("@username", SqlDbType.NVarChar, 64).Value = TextBox1.Text;
         // Hash the password
         HashAlgorithm hashalgorithm = HashAlgorithm.Create("sha1");
         byte [] byteIn = System.Text.ASCIIEncoding.ASCII.GetBytes(TextBox2.Text);
         string hashedPass = System.Convert.ToBase64String(hashalgorithm.ComputeHash(byteIn));

			cmd.Parameters.Add("@password", SqlDbType.NVarChar, 128).Value = hashedPass;
			
         FormsAuthentication.HashPasswordForStoringInConfigFile(TextBox2.Text,"sha1");
 
			// Execute the command
			SqlDataReader reader = cmd.ExecuteReader();
			if (reader.Read())
			{
				// Create a new ticket used for authentication
				FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
					1, // Ticket version
					TextBox1.Text, // Username to be associated with this ticket
					DateTime.Now, // Date/time issued
					DateTime.Now.AddMinutes(30), // Date/time to expire
					true, // "true" for a persistent user cookie (could be a checkbox on form)
					reader.GetString(0), // User-data  (the roles from the user record)
					FormsAuthentication.FormsCookiePath); // Path cookie is valid for
 
				// Hash the cookie for transport over the wire
				string hash = FormsAuthentication.Encrypt(ticket);
				HttpCookie cookie = new HttpCookie(
					FormsAuthentication.FormsCookieName, // Name of auth cookie
					hash); // Hashed ticket
 
				// Add the cookie to the list for outbound response
				Response.Cookies.Add(cookie);
 
				// Redirect to requested URL, or homepage if no previous page requested
				string returnUrl = Request.QueryString["ReturnUrl"];
				if (returnUrl == null) returnUrl = "LoggedIn.aspx";
   
				// Don't call the FormsAuthentication.RedirectFromLoginPage since it could
				// replace the authentication ticket we just added...
				Response.Redirect(returnUrl);
			}
			else
			{
				//  Username and or password not found in our database...
				ErrorLabel.Text = "Username / password incorrect. Please login again.";
				ErrorLabel.Visible = true;
			}

		}
	}
}
