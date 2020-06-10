<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResourceSummary.aspx.cs" Inherits="streebo.METIS.UI.ResourceSummary" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <title>Resource Summary</title>
      <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <link href="styles/main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        // necessary to disable the weekends on client-side navigation
        function OnDayRender(calendarInstance, args) {
            // convert the date-triplet to a javascript date
            // we need Date.getDay() method to determine 
            // which days should be disabled (e.g. every Saturday (day = 6) and Sunday (day = 0))                
            var jsDate = new Date(args.get_date()[0], args.get_date()[1] - 1, args.get_date()[2]);
            // if (jsDate.getDay() == 0 || jsDate.getDay() == 6)
            if (jsDate.getDay() != 6) {
                var otherMonthCssClass = "rcOutOfRange";
                args.get_cell().className = otherMonthCssClass;
                // replace the default cell content (anchor tag) with a span element 
                // that contains the processed calendar day number -- necessary for the calendar skinning mechanism 
                args.get_cell().innerHTML = "<span>" + args.get_date()[2] + "</span>";
                // disable selection and hover effect for the cell
                args.get_cell().DayId = "";
            }
        }
    </script>
</head>
<body>
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
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbSearch">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnExpandAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnCollapseAll">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="dpWeekEnding">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid_weekly">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                    <telerik:AjaxUpdatedControl ControlID="radwindowPopup" />
                    <telerik:AjaxUpdatedControl ControlID="cmbDepartment" />
                    <telerik:AjaxUpdatedControl ControlID="RadMenu1" />
                    <telerik:AjaxUpdatedControl ControlID="rblAssignmentType" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_bulk" />
                    <telerik:AjaxUpdatedControl ControlID="lbLogout" />
                    <telerik:AjaxUpdatedControl ControlID="lbRowID" />
                    <telerik:AjaxUpdatedControl ControlID="tbEnterBool" />
                    <telerik:AjaxUpdatedControl ControlID="Button1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radwindowPopup">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="btnClose" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnClose">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radwindowPopup" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="dpWeekEnding">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadMenu1">
                <UpdatedControls>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ibRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbDepartment">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbReportType">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" LoadingPanelID="RadAjaxLoadingPanel1" />
                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                    <telerik:AjaxUpdatedControl ControlID="cmbDepartment" />
                    <telerik:AjaxUpdatedControl ControlID="dpWeekEnding" />
                    <telerik:AjaxUpdatedControl ControlID="ibRefresh" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadToolTipManager1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radwindowPopup">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rblAssignmentType">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid_bulk" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadWindowManager1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radwindowPopup">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="10">
        <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
            style="border: 0px; padding-top: 10%;" width="200px" />
            
    </telerik:RadAjaxLoadingPanel>
    
    <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="ManualClose"
        Width="250" Height="350" runat="server" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element"
        Animation="Resize" Position="TopRight" AutoCloseDelay="1">
    </telerik:RadToolTipManager>
    
     <!-- START: Main Container -->
     <div class="mainContainer">
        
        <!-- START: Header -->
        <div class="headerPanel">
                	
    		<img class="metis" src="./images/metis_logo.png" alt="METIS Logo">
            
            <div class="action">            	
                <span class="title">Salman Kasbati &nbsp;|&nbsp;</span> 
                <span class="logout"><asp:LinkButton ID="lbLogout" runat="server" OnClick="lbLogout_OnClick" Font-Size="Smaller"
            Height="15px">Logout</asp:LinkButton></span>
            </div>
            
        </div>
        <!-- END: Header -->
        
        <!-- START: Menu -->
          <ul class="menu">	
            	<li class="selected"><a href="ResourceSummary.aspx">Resource Summary</a></li>
                <li><a href="#">Project Summary</a></li>
                <li><a href="Assignments.aspx">Assignments</a></li>            
            </ul>
         <!-- END: Menu -->
         
         <!-- START: Filer Area -->
         <div class="filter">
            <table border="0" cellspacing="5" cellpadding="5" width="100%">
                <tr>
                    <td width="150">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="CMB" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged">
                     <Items>
                                <asp:ListItem Text="All" Value="" />
                                <asp:ListItem Text="Not Assigned" Value="" />
                     </Items>
                    </asp:DropDownList>
                    </td>
                    <td width="150">
                            <telerik:RadDatePicker ID="dpWeekEnding" runat="server" ShowPopupOnFocus="false"
                            DatePopupButton-Visible="true" Width="160" MinDate="01/01/1800" MaxDate="12/31/2200"
                            OnSelectedDateChanged="OnSelectedDateChanged" AutoPostBack="true">
                            <Calendar runat="Server" OnDayRender="Calendar_OnDayRender" DisabledDayStyle-BackColor="Gray">
                                <ClientEvents OnDayRender="OnDayRender" />
                                <DisabledDayStyle BackColor="Gray"></DisabledDayStyle>
                            </Calendar>
                            <DatePopupButton Visible="true" CssClass="" ImageUrl="" HoverImageUrl="" TabIndex="-1">
                            </DatePopupButton>
                            <DateInput DisplayDateFormat="M/d/yyyy" DateFormat="M/d/yyyy" LabelWidth="" Width="160px"
                                AutoPostBack="True" AccessKey="w">
                            </DateInput>
                            </telerik:RadDatePicker>
                        </td>
                    <td width="150">
                            <telerik:RadComboBox ID="rcReportType" runat="server" CssClass="CMB" AutoPostBack="true"
                            OnSelectedIndexChanged="rcReportType_OnSelectedIndexChanged" EmptyMessage="Select Weekly/Daily">
                            <Items>
                                <telerik:RadComboBoxItem Text="Weekly" Value="1" Selected="true" />
                                <telerik:RadComboBoxItem Text="Daily" Value="2" />
                            </Items>
                            </telerik:RadComboBox>
                        </td>
                    <td width="150">
                            <telerik:RadComboBox ID="rcResc" runat="server" CssClass="CMB_search" AutoPostBack="True"
                            OnSelectedIndexChanged="rcResc_OnSelectedIndexChanged" EmptyMessage="Search Here"
                            ShowDropDownOnTextboxClick="true"
                            ShowToggleImage="false" AllowCustomText="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="All" Value="--" Selected="true" />
                            </Items>
                            </telerik:RadComboBox>
                        </td>
                    <td>&nbsp;</td>
                    <td align="right"><a href="#"><img src="./images/icon_referesh.png" width="25" height="21" border="0"></a></td>
                </tr>
                <tr>
                    <td colspan="6"><%--<a href="#"><img src="/image/icon_expand.png" alt="Expand" width="23" height="19" border="0"></a>--%>
                        <asp:ImageButton ID="btnExpandAll" runat="server" Text="Expand All" OnClick="btnExpandAllClick"
             AccessKey="." Font-Size="Smaller" ImageUrl="./images/icon_expand.png"></asp:ImageButton>
            
                        <asp:ImageButton ID="btnCollapseAll" runat="server" Text="Collapse All" OnClick="btnCollapseAllClick"
                 AccessKey="," Font-Size="Smaller" ImageUrl="./images/icon_collapse.png"></asp:ImageButton>
                            <%--<a href="#"><img src="/image/icon_collapse.png" alt="Collapse" width="23" height="19" border="0"></a>--%>
                        </td>                        
                </tr>
            </table>
         </div>
         <!-- END: Filer Area -->
      <%--   Weekly Grid--%>
         <telerik:RadGrid ID="rgWeekly" runat="server" CellSpacing="0" Height="500px" Width="100%" AutoGenerateColumns="False" Skin="Office2007" GridLines="None" AllowSorting="True" 
            OnDetailTableDataBind="rgWeekly_DetailTableDataBind"
            OnPreRender="rgWeekly_PreRender" 
            OnUpdateCommand="rgWeekly_UpdateCommand" OnItemCreated="rgWeekly_ItemCreated"
            OnItemCommand="rgWeekly_ItemCommand" OnItemDataBound="rgWeekly_ItemDataBound">
            <FilterMenu EnableImageSprites="False"></FilterMenu>
            <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true">
                    <Selecting CellSelectionMode="None" AllowRowSelect="true"></Selecting>
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="1" />
                    <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true"
                        ExpandDetailTableKey="RightArrow" CollapseDetailTableKey="LeftArrow"></KeyboardNavigationSettings>
                </ClientSettings>
            <%--Start of --%>
            <MasterTableView Name="MasterTable" Width="100%" HeaderStyle-Width="60px" AutoGenerateColumns="true"
            HeaderStyle-Wrap="false" HeaderStyle-Font-Size="Smaller" TableLayout="Fixed"
                    DataKeyNames="Resource_id"  AlternatingItemStyle-CssClass="color1" HeaderStyle-Font-Bold>
                    <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                    <DetailTables>
                        <telerik:GridTableView runat="server" DataKeyNames="Resource_id" CommandItemDisplay="None"
                            EditMode="InPlace" TabIndex="-1" Name="DetailTable" Width="96%" OnPreRender="rgWeekly_PreRender_detail"
                            AutoGenerateColumns="true" TableLayout="Fixed" HierarchyLoadMode="ServerBind"
                            HeaderStyle-Width="60px" HeaderStyle-Font-Size="Smaller">
                            <ParentTableRelation>
                                <telerik:GridRelationFields DetailKeyField="Resource_id" MasterKeyField="Resource_id" />
                            </ParentTableRelation>
                            <HeaderStyle CssClass="color2" />
                            <Columns>
                                <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                    HeaderStyle-Width="30px">
                                    <HeaderStyle Width="30px"></HeaderStyle>
                                </telerik:GridEditCommandColumn>
                                <telerik:GridDropDownColumn  HeaderText="Project Name"
                                    UniqueName="ddProject_id"
                                    HeaderStyle-Width="220px">
                                </telerik:GridDropDownColumn>
                            </Columns>
                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            <HeaderStyle Width="60px"></HeaderStyle>
                        </telerik:GridTableView>
                    </DetailTables>
                    <Columns>
                        <telerik:GridBoundColumn DataField="Resource_name" FilterControlAltText="Filter Resource_name column"
                            HeaderText="Resource Name" SortExpression="Resource_name" UniqueName="Resource_nameV"
                            HeaderStyle-Width="250px" Visible="false">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" SortExpression="Resource_name" FilterControlAltText="Filter Resource_name column"
                            DataField="Resource_name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" HeaderStyle-Width="250px">
                            <ItemTemplate>
                                <asp:HyperLink ID="targetControl2" runat="server"  Text='<%# Eval("Resource_name") %>'></asp:HyperLink>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </RowIndicatorColumn>
                    <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                        <HeaderStyle Width="20px"></HeaderStyle>
                    </ExpandCollapseColumn>
                    <EditFormSettings>
                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                        </EditColumn>
                    </EditFormSettings>
                    <HeaderStyle Width="60px"></HeaderStyle>
                </MasterTableView>
            </telerik:RadGrid>
         
         <%-- Hidden Fields--%>
            <asp:Label ID="lblResourceID" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblResourceIDSearch" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblWeekending" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblDay" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblcmbDepartment_selectedValue" runat="server" Text="" Visible="false"></asp:Label>
            <asp:Label ID="lblcmbDepartment_selectedText" runat="server" Text="" Visible="false"></asp:Label>
            <asp:TextBox ID="tbCurrentRow" runat="server" Visible="true" Width="0px" Height="0px"></asp:TextBox>
            <asp:TextBox ID="tbEnterBool" runat="server" Visible="true" Width="0px" Height="0px"></asp:TextBox>
            <asp:Label ID="lbRowID" runat="server" Text="" Visible="true" Font-Size="Smaller"
             Width="0px" Height="0px"></asp:Label>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="btnClose_OnClick"
             Visible="false" />
         <%--End of Hidden Fields--%>
        
    </div>
    </form>
</body>
</html>





















































