<%@ Page language="c#" Codebehind="Default.aspx.cs" AutoEventWireup="false" Inherits="FormsAuthDB.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
   <HEAD>
      <title>WebForm1</title>
      <meta name="GENERATOR" Content="Microsoft Visual Studio 7.0">
      <meta name="CODE_LANGUAGE" Content="C#">
      <meta name="vs_defaultClientScript" content="JavaScript">
      <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
   </HEAD>
   <body MS_POSITIONING="GridLayout">
      <form id="Form1" method="post" runat="server">
         <asp:Button id="Button1" style="Z-INDEX: 101; LEFT: 528px; POSITION: absolute; TOP: 72px" runat="server" Text="LOG IN" Width="104px"></asp:Button>
         <asp:TextBox id="TextBox1" style="Z-INDEX: 102; LEFT: 96px; POSITION: absolute; TOP: 72px" runat="server" Width="160px"></asp:TextBox>
         <asp:TextBox id="TextBox2" style="Z-INDEX: 103; LEFT: 296px; POSITION: absolute; TOP: 72px" runat="server"></asp:TextBox>
         <asp:Label id="Label1" style="Z-INDEX: 104; LEFT: 104px; POSITION: absolute; TOP: 40px" runat="server" Width="152px" Height="24px">UserName</asp:Label>
         <asp:Label id="Label2" style="Z-INDEX: 105; LEFT: 312px; POSITION: absolute; TOP: 40px" runat="server" Width="128px">Password</asp:Label>
         <asp:Label id="ErrorLabel" style="Z-INDEX: 106; LEFT: 128px; POSITION: absolute; TOP: 120px" runat="server" Width="328px" Height="32px"></asp:Label>
      </form>
   </body>
</HTML>
