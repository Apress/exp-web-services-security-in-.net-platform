using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace RegistryWebApp
{
	public class WebForm1 : System.Web.UI.Page
	{
      protected System.Web.UI.WebControls.Label lblTitle;
      protected System.Web.UI.WebControls.Label lblStatusTitle;
      protected System.Web.UI.WebControls.Label lblStatus;
      protected System.Web.UI.WebControls.TextBox tbPassword;
      protected System.Web.UI.WebControls.TextBox tbUsername;
      protected System.Web.UI.WebControls.TextBox tbDatabase;
      protected System.Web.UI.WebControls.Label lblPassword;
      protected System.Web.UI.WebControls.Label lblUser;
      protected System.Web.UI.WebControls.Label lblDatabase;
      protected System.Web.UI.WebControls.Label lblServer;
      protected System.Web.UI.WebControls.CheckBox chkSSPI;
      protected System.Web.UI.WebControls.TextBox tbServer;
      protected System.Web.UI.WebControls.Button btnWrite;
      protected System.Web.UI.WebControls.Button btnRead;
   
		private void Page_Load(object sender, System.EventArgs e)
		{
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
         this.chkSSPI.CheckedChanged += new System.EventHandler(this.chkSSPI_CheckedChanged);
         this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
         this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
         this.Load += new System.EventHandler(this.Page_Load);

      }
		#endregion

      private void chkSSPI_CheckedChanged(object sender, System.EventArgs e)
      {
         this.lblUser.Enabled = !this.chkSSPI.Checked;
         this.tbUsername.Enabled = !this.chkSSPI.Checked;
         this.tbUsername.Text="";
         this.lblPassword.Enabled = ! this.chkSSPI.Checked;
         this.tbPassword.Enabled = ! this.chkSSPI.Checked;
         this.tbPassword.Text="";
         //RegistryStorage.connectionstrings.IntegratedSecurity = this.chkSSPI.Checked;
      }

      private void btnWrite_Click(object sender, System.EventArgs e)
      {
         try
         {
            System.Security.Principal.IIdentity	iId = System.Web.HttpContext.Current.User.Identity;
            StringBuilder	msg = new StringBuilder();
            msg.AppendFormat(@"============================================Debugging ============================================{0}IdentityAuthenticationType:  {1}{0}IdentityAuthenticated:  {2}{0}IdentityName:  {3}{0}System Environment:{0}UserDomainName:  {4}{0}UserName:  {5}{0}UserInteractive:  {6}{0}MachineName:  {7}{0}",
               Environment.NewLine, 
               iId.AuthenticationType, 
               iId.IsAuthenticated.ToString(), 
               iId.Name, 
               System.Environment.UserDomainName,
               System.Environment.UserName,
               System.Environment.UserInteractive.ToString(),
               System.Environment.MachineName);

            Debug.WriteLine(msg);

//            Utils.connectionstrings.writeSQLConnectionString(this.tbServer.Text,
//               this.tbUsername.Text,
//               this.tbPassword.Text,
//               this.tbDatabase.Text);
   
            this.lblStatus.Text = "Registry Updated";
         }
         catch(Exception ex)
         {
            this.lblStatus.Text = "Error writing to registry: " + ex.Message;
            Debug.WriteLine(ex.ToString());
         }
      }

      private void btnRead_Click(object sender, System.EventArgs e)
      {
         try
         {
            System.Security.Principal.IIdentity	iId = System.Web.HttpContext.Current.User.Identity;
            StringBuilder	msg = new StringBuilder();
            msg.AppendFormat(@"============================================Debugging ============================================{0}IdentityAuthenticationType:  {1}{0}IdentityAuthenticated:  {2}{0}IdentityName:  {3}{0}System Environment:{0}UserDomainName:  {4}{0}UserName:  {5}{0}UserInteractive:  {6}{0}MachineName:  {7}{0}",
               Environment.NewLine, 
               iId.AuthenticationType, 
               iId.IsAuthenticated.ToString(), 
               iId.Name, 
               System.Environment.UserDomainName,
               System.Environment.UserName,
               System.Environment.UserInteractive.ToString(),
               System.Environment.MachineName);

            Debug.WriteLine(msg);

            //this.lblStatus.Text = "Connection string = " + Utils.connectionstrings.SQLConnectionString;
         }
         catch(Exception ex)
         {
            this.lblStatus.Text = "Error reading registry: " + ex.Message;
            Debug.WriteLine(ex.ToString());
         }
      }
	}
}
