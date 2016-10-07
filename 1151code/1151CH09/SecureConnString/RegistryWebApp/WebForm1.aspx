<%@ Page language="c#" Codebehind="WebForm1.aspx.cs" AutoEventWireup="false" Inherits="RegistryWebApp.WebForm1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
   <HEAD>
      <title>WebForm1</title>
      <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
      <meta name="CODE_LANGUAGE" Content="C#">
      <meta name="vs_defaultClientScript" content="JavaScript">
      <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
   </HEAD>
   <body MS_POSITIONING="GridLayout">
      <form id="Form1" method="post" runat="server">
         <asp:Label id="lblTitle" style="Z-INDEX: 101; LEFT: 210px; POSITION: absolute; TOP: 101px"
            runat="server" Font-Size="Large">SQL Connection String Registry Example</asp:Label>
         <asp:TextBox id="tbPassword" style="Z-INDEX: 110; LEFT: 282px; POSITION: absolute; TOP: 341px"
            tabIndex="4" runat="server"></asp:TextBox>
         <asp:TextBox id="tbUsername" style="Z-INDEX: 109; LEFT: 283px; POSITION: absolute; TOP: 310px"
            tabIndex="4" runat="server"></asp:TextBox>
         <asp:TextBox id="tbDatabase" style="Z-INDEX: 108; LEFT: 286px; POSITION: absolute; TOP: 227px"
            tabIndex="2" runat="server"></asp:TextBox>
         <asp:Label id="lblPassword" style="Z-INDEX: 106; LEFT: 168px; POSITION: absolute; TOP: 348px"
            runat="server">Password:</asp:Label>
         <asp:Label id="lblUser" style="Z-INDEX: 105; LEFT: 167px; POSITION: absolute; TOP: 314px" runat="server">User Name:</asp:Label>
         <asp:Label id="lblDatabase" style="Z-INDEX: 103; LEFT: 164px; POSITION: absolute; TOP: 231px"
            runat="server">Database Name:</asp:Label>
         <asp:Label id="lblServer" style="Z-INDEX: 102; LEFT: 164px; POSITION: absolute; TOP: 193px"
            runat="server">Server Name:</asp:Label>
         <asp:CheckBox id="chkSSPI" style="Z-INDEX: 104; LEFT: 238px; POSITION: absolute; TOP: 270px" tabIndex="3"
            runat="server" Text="Using Integrated Security" AutoPostBack="True"></asp:CheckBox>
         <asp:TextBox id="tbServer" style="Z-INDEX: 107; LEFT: 285px; POSITION: absolute; TOP: 191px"
            tabIndex="1" runat="server"></asp:TextBox>
         <asp:Button id="btnWrite" style="Z-INDEX: 111; LEFT: 191px; POSITION: absolute; TOP: 422px"
            tabIndex="5" runat="server" Text="Write To Registry"></asp:Button>
         <asp:Button id="btnRead" style="Z-INDEX: 112; LEFT: 379px; POSITION: absolute; TOP: 423px" tabIndex="6"
            runat="server" Text="Read Registry"></asp:Button>
         <asp:Label id="lblStatusTitle" style="Z-INDEX: 113; LEFT: 271px; POSITION: absolute; TOP: 480px"
            runat="server">Status:</asp:Label>
         <asp:Label id="lblStatus" style="Z-INDEX: 114; LEFT: 341px; POSITION: absolute; TOP: 480px"
            runat="server"></asp:Label>
      </form>
   </body>
</HTML>
