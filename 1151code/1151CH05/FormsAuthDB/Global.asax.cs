using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
 using System.Security.Principal;

namespace FormsAuthDB 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		public Global()
		{
			InitializeComponent();
		}	
		
		
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			if (HttpContext.Current.User != null)
			{
				if (HttpContext.Current.User.Identity.IsAuthenticated)
				{
					if (HttpContext.Current.User.Identity is FormsIdentity)
					{
						  // Get Forms Identity From Current User
						FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
						  // Get Forms Ticket From Identity object
						FormsAuthenticationTicket ticket = id.Ticket;
						  // Retrieve stored user-data (our roles from db)
						string userData = ticket.UserData;
						string[] roles = userData.Split(',');
						  // Create a new Generic Principal Instance and assign to Current User
						HttpContext.Current.User = new GenericPrincipal(id, roles);
					}
				}
			}
		}


		protected void Application_Error(Object sender, EventArgs e)
		{

		}

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}
			
		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}

