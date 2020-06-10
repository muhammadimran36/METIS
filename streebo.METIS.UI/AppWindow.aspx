<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppWindow.aspx.cs" Inherits="streebo.METIS.UI.AppWindow" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>Edit Bulk Assignment</title>
    <link rel="stylesheet" type="text/css" href="styles/main.css">
    <style type="text/css">
        #form2
        {
            background-color: #CBDDEF;
            height: 424px;
            width: 580px;
            margin-bottom: 5px;
        }
        .style13
        {
            height: 15px;
            width: 577px;
        }
        .style14
        {
            height: 50px;
            width: 577px;
        }
        .style15
        {
            height: 47px;
            width: 577px;
        }
        .btnInsertCancel
        {
            border: medium none;
            color: #FFFFFF;
            cursor: pointer;
            display: inline-block;
            font: bold 14px/100% Tahoma,Helvetica,sans-serif;
            height: 33px;
            text-align: center;
            text-decoration: none;
            text-shadow: 0 1px 1px rgba(0, 0, 0, 0.3);
            width: 89px;
        }
    </style>
    <script type="text/javascript">
        function closeWindow() {
            window.close();
            return false;
        }
    
         
                         
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <table align="center">
        <tr>
            <td class="style13" align="justify">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
            </td>
        </tr>
        <tr>
            <td class="style14" align="justify">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label1" runat="server" Text="Date of Assignment:" Font-Bold="True"
                    ForeColor="#000099" Font-Names="Calibri" Font-Size="Medium"></asp:Label>
                <telerik:RadDatePicker ID="dWeekEnding" runat="Server" AppendDataBoundItems="true"
                    Width="206px" OnSelectedDateChanged="dWeekEnding_SelectedDateChanged" FocusedDate="2013-01-01"
                    ToolTip="Date of Assignment " Height="25px" Skin="Telerik">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"
                        Skin="Telerik">
                    </Calendar>
                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="M/d/yyyy" EnableSingleInputRendering="True"
                        LabelWidth="64px" Height="25px">
                    </DateInput>
                </telerik:RadDatePicker>
            &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td class="style15" align="justify">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label2" runat="server" Text="Resource Name:" Font-Bold="True" ForeColor="#000099"
                    Font-Names="Calibri" Font-Size="Medium"></asp:Label>
                <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true"
                    AutoPostBack="true" OnSelectedIndexChanged="comboResourceName_SelectedIndexChanged"
                    Width="186px" ToolTip="Resource Name" Height="23px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="style15" align="justify">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label3" runat="server" Text="Projects:" Font-Bold="True" ForeColor="#000099"
                    Font-Names="Calibri" Font-Size="Medium"></asp:Label>
                <asp:DropDownList ID="comboProjectName" Columns="30" runat="Server" AppendDataBoundItems="true"
                    Width="186px" OnSelectedIndexChanged="comboProjectName_SelectedIndexChanged"
                    ToolTip="Projects">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="comboProjectName" ErrorMessage="Enter Project"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style15" align="justify">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label4" runat="server" Text="Enter Start Date:" Font-Bold="True" ForeColor="#000099"
                    Font-Names="Calibri" Font-Size="Medium"></asp:Label>
                <telerik:RadDatePicker ID="calenderStartDate" runat="Server" AppendDataBoundItems="true"
                    OnSelectedDateChanged="calenderStartDate_SelectedDateChanged" Height="24px" Width="215px"
                    ToolTip="Start Date" Skin="Telerik" Style="margin-left: 0px">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"
                        Skin="Telerik">
                    </Calendar>
                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="M/d/yyyy" EnableSingleInputRendering="True"
                        LabelWidth="64px" Height="24px">
                    </DateInput>
                </telerik:RadDatePicker>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="calenderStartDate" ErrorMessage="Enter Start Date"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style15" align="justify">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label5" runat="server" Text="Enter End Date:" Font-Bold="True" ForeColor="#000099"
                    Font-Names="Calibri" Font-Size="Medium"></asp:Label>
                <telerik:RadDatePicker ID="calenderEndDate" runat="Server" AppendDataBoundItems="true"
                    OnSelectedDateChanged="calenderEndDate_SelectedDateChanged" Width="215px" Height="24px"
                    ToolTip="End Date" Skin="Telerik" AutoPostBack="True">
                    <Calendar UseRowHeadersAsSelectors="False" UseColumnHeadersAsSelectors="False" ViewSelectorText="x"
                        Skin="Telerik">
                    </Calendar>
                    <DatePopupButton ImageUrl="" HoverImageUrl=""></DatePopupButton>
                    <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="M/d/yyyy" EnableSingleInputRendering="True"
                        LabelWidth="64px" Height="24px" AutoPostBack="True">
                    </DateInput>
                </telerik:RadDatePicker>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="calenderEndDate" ErrorMessage="Enter End Date"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style15" align="justify">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label6" runat="server" Text="Work Load:" Font-Bold="True" ForeColor="#000099"
                    Font-Names="Calibri" Font-Size="Medium"></asp:Label>
                <asp:TextBox ID="txtWorkLoad" runat="Server" OnTextChanged="txtWorkLoad_TextChanged"
                    ToolTip="Work Load" Width="183px" />
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtWorkLoad" ErrorMessage="Enter Work Load"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style15" align="justify">
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="InsertButton" runat="server" Text="Insert" BackColor="#27508E" OnClick="InsertButton_Click"
                    Width="82px" CssClass="btnInsertCancel" BorderColor="#00CCFF" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="CancelButton" runat="server" Text="Cancel" BackColor="#74ACDE" OnClientClick="closeWindow();"
                    Width="82px" CssClass="btnInsertCancel" />
            </td>
        </tr>
        <tr>
            <td>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label7" runat="server" 
                    Text="You cannot select the end date less than the start date " 
                    ForeColor="Red"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
