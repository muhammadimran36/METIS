<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs"  Inherits="streebo.METIS.UI.Login"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html>
<head runat="server" profile="~/Login.aspx">
    <title>Login</title>     
    <link href="styles/main.css" rel="stylesheet" type="text/css" />
 
<link rel="icon" 
      type="image/x-icon" 
      href="C:/Users/talha.zafar/Downloads/favicon (1).ico" />

</head>
<body class="bodyLogin">

    <script type="text/javascript">
        function openWindow(url) {
            var w = window.open(url, '', 'width=1000,height=600 ,toolbar=0,status=0,location=0,menubar=0 ,directories=0,resizable=1,scrollbars=1');
            w.focus();
        } 

    </script>

    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
            <%--Needed for JavaScript IntelliSense in VS2010--%>
            <%--For VS2008 replace RadScriptManager with ScriptManager--%>
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
            <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
        </Scripts>
    </telerik:RadScriptManager>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting>
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnlogin" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="loginContainer">
	
        <div class="form">
            Language: <asp:DropDownList ID="DropDownListLanguage" runat="server">
                <asp:ListItem Value="English" Text="English"></asp:ListItem>
                <asp:ListItem Value="Russian" Text="Russian"></asp:ListItem>
                 <asp:ListItem Value="kyrgyzstan" Text="kyrgyzstan"></asp:ListItem>
                      </asp:DropDownList>
            <asp:ImageButton ID="LoginImg" runat="server" />
<%--    	    <img src="<%# HttpContext.GetGlobalResourceObject("ResourceEN", "LoginIMG").ToString() %>" alt="METIS Logo">--%>
   	 	    <form method="post" action="index.html">
        	    <%--<input name="user" type="text" class="user" placeholder="Username">--%>
        	    <asp:TextBox ID="txtUsername" runat="server" CssClass="user" 
                    ontextchanged="txtUsername_TextChanged"></asp:TextBox>
                <%--<input name="password" type="password" class="password" placeholder="Password">  --%>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="password"></asp:TextBox>
                <%--<input type="submit" class="submit" value="" />--%>  	
                <asp:Button ID="btnlogin" runat="server" OnClick="btnlogin_Click" CssClass="submit"></asp:Button> 
  	 	    </form>
            <!--<span class="loginErr"> Invalid Username or Password! </span>-->
            <asp:Label ID="lblMessage" runat="server" Text="">
                </asp:Label>
      </div>

    </div>

    
    </form>
</body>
</html>
