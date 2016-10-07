using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

namespace FormsAuthDB
{
	/// <summary>
	/// Summary description for LoggedIn.
	/// </summary>
	public class LoggedIn : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			try
			{
				// if they haven't logged in this will fail and we can send them to
				// the login page
				FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
			}
			// whatever bad happened, let's just send them back to login page for now...
			catch(Exception ex )
			{
				Response.Redirect("Default.aspx");  // whatever your login page is
			}

                                // is this an Administrator role?
				if (User.IsInRole("Administrator"))
				{
					Response.Write("Welcome Big Admin!");
					// ok let's enumerate their roles for them...
					FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
					FormsAuthenticationTicket ticket = id.Ticket;
					string userData = ticket.UserData;
					string[] roles = userData.Split(',');
					foreach(string role in roles)
					{
						Response.Write("You are: " + role.ToString()+"<BR>");
					}
					Response.Write ("You get to see the Admin link:<BR><A href=\"Admin/Adminstuff.aspx\">Admin Only</a>");
				} 
				else 
				{
                                        // ok, they got in but we know they aren't an Administrator...
					Response.Write("Ya got logged in, but you ain't an Administrator!");
					FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
					FormsAuthenticationTicket ticket = id.Ticket;
	 
					string userData = ticket.UserData;
					string[] roles = userData.Split(',');
					foreach(string role in roles)
					{
						Response.Write("You are: " +role.ToString()+"<BR>");
					}
 
		}
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
