<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResSum.aspx.cs" Inherits="streebo.METIS.UI.ResSum" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Src="DetailsCS.ascx" TagName="Details" TagPrefix="uc1" %>
<html>
<link href="general.css" rel="stylesheet" type="text/css" />
<head runat="server">
    <title>Resource Summary</title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <style type="text/css">
        .color2 {
            background-image: none !important;
            background-color: Lightgray !important;
        }

        .ddlResource {
            align: left;
        }

        .style1 {
            height: 20px;
        }
    </style>
    <link href="styles/main.css" rel="stylesheet" type="text/css" />
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

        <script type="text/javascript">
            function KeyPressed(sender, eventArgs) {
                if (eventArgs.get_keyCode() == 13) {
                    eventArgs.set_cancel(true)
                    var textbox = document.getElementsByName('tbEnterBool')[0]
                    textbox.value = 'false';
                }
            }
            function ActiveRowChanged(sender, eventArgs) {
                //alert("The active row is: " + eventArgs.get_itemIndexHierarchical());
                var row = eventArgs.get_itemIndexHierarchical();
                var textbox = document.getElementsByName('tbCurrentRow')[0]
                textbox.value = row;


            }


            // necessary to disable the weekends on client-side navigation
            function OnDayRender(calendarInstance, args) {
                // convert the date-triplet to a javascript date
                // we need Date.getDay() method to determine 
                // which days should be disabled (e.g. every Saturday (day = 6) and Sunday (day = 0))                
                var jsDate = new Date(args.get_date()[0], args.get_date()[1] - 1, args.get_date()[2]);
                // if (jsDate.getDay() == 0 || jsDate.getDay() == 6)
                if (jsDate.getDay() != 1) {
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

        <script type="text/javascript">

            function RowClick(sender, eventArgs) {

                var rId = eventArgs.getDataKeyValue("Resource_id");
                var URL = "AppWindow.aspx?id=" + rId;

                window.open(URL, 'Resource_id', 'height=400px, width=500px, top=134px, left=250px, resizable= Yes');


            }

            function closeWindow(rWindow) {
                rWindow.hide();
                rWindow.close();

            }

            function divScroll(Count) {

                var iDivCount = parseInt(Count, 10);
                var divsVariable = new Array();
                var RadGrid_weekly_Frozen = document.getElementById('RadGrid_weekly_Frozen');


                if (parseInt(Count, 10) >= 1) {

                    for (var i = 1; i <= parseInt(Count, 10) ; i++) {


                        divsVariable[i] = document.getElementById('div_dt' + i);

                        if (divsVariable[i] != null)
                            divsVariable[i].scrollLeft = RadGrid_weekly_Frozen.scrollLeft;
                    }
                }
            }

            function onRequestStart(sender, args) {

                if (args.get_eventTarget().indexOf("btnExport") >= 0 ||

                args.get_eventTarget().indexOf("btnExport") >= 0 ||

                args.get_eventTarget().indexOf("btnExport") >= 0) {

                    args.set_enableAjax(false);

                }

            }

        </script>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="btnExport">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid_weekly" />
                        <telerik:AjaxUpdatedControl ControlID="btnExport" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
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
            <ClientEvents OnRequestStart="onRequestStart" />
        </telerik:RadAjaxManager>
        <telerik:RadToolTipManager ID="RadToolTipManager1" OffsetY="-1" HideEvent="LeaveToolTip"
            Width="225" Height="225" runat="server" OnAjaxUpdate="OnAjaxUpdate" RelativeTo="Element"
            Animation="Slide" Position="TopRight" AutoCloseDelay="1">
        </telerik:RadToolTipManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Transparency="10">
            <img alt="Loading..." src='<%= RadAjaxLoadingPanel.GetWebResourceUrl(Page, "Telerik.Web.UI.Skins.Default.Ajax.loading.gif") %>'
                style="border: 0px; padding-top: 10%;" width="200px" />
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true"
            Width="860px" Height="500px">
            <Windows>
                <telerik:RadWindow ID="radwindowPopup" runat="server" VisibleOnPageLoad="false" Modal="true"
                    BackColor="#DADADA" VisibleStatusbar="false" Behaviors="None" Title="">
                    <Shortcuts>
                        <telerik:WindowShortcut CommandName="Close" Shortcut="Alt+x" />
                        <telerik:WindowShortcut CommandName="Maximize" Shortcut="Ctrl+Space" />
                        <telerik:WindowShortcut CommandName="Restore" Shortcut="Ctrl+F3" />
                        <telerik:WindowShortcut CommandName="Minimize" Shortcut="Ctrl+m" />
                    </Shortcuts>
                    <ContentTemplate>
                        <div style="padding: 0 0 0 97%;">
                            <asp:ImageButton ID="btnClose" runat="server" ImageUrl="~/Image/close.jpg" OnClick="btnClose_OnClick"
                                ToolTip="Close Window" />
                        </div>
                        <asp:RadioButtonList ID="rblAssignmentType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblAssignmentType_SelectedIndexChanged"
                            RepeatDirection="Horizontal" CellPadding="4" CellSpacing="4" Font-Size="Small"
                            Visible="false">
                            <asp:ListItem Value="Normal_Assignment" Selected="True">Normal Assignment</asp:ListItem>
                            <asp:ListItem Value="Bulk_Assignment">Bulk Assignment</asp:ListItem>
                        </asp:RadioButtonList>
                        <telerik:RadGrid ID="RadGrid2" runat="server" AllowSorting="true" PageSize="10" BorderStyle="None"
                            Skin="Transparent" OnItemCommand="RadGrid2_ItemCommand" OnUpdateCommand="RadGrid2_UpdateCommand"
                            OnInsertCommand="RadGrid2_InsertCommand" OnDeleteCommand="RadGrid2_OnDeleteCommand"
                            OnItemDataBound="RadGrid2_ItemDataBound" AllowAutomaticInserts="true" AllowAutomaticUpdates="true"
                            Width="100%">
                            <ClientSettings EnableRowHoverStyle="false" AllowKeyboardNavigation="true" ClientEvents-OnRowClick="RowClick">
                                <Selecting AllowRowSelect="false" />
                                <Selecting CellSelectionMode="None"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="LeftArrow" />

                            </ClientSettings>
                            <MasterTableView EditMode="InPlace" AutoGenerateColumns="False" DataKeyNames="S_no"
                                CommandItemDisplay="Top" CurrentResetPageIndexAction="SetPageIndexToFirst" Dir="LTR"
                                Frame="Border" TableLayout="Auto">
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridBoundColumn DataField="S_no" FilterControlAltText="Filter S_no column"
                                        HeaderText="S_no" SortExpression="S_no" UniqueName="S_no" Visible="false" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridDateTimeColumn DataField="Week_endings" HeaderText="Week_endings" SortExpression="Week_endings"
                                        UniqueName="Week_endings" ReadOnly="false" ItemStyle-Width="100px">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridDropDownColumn DataField="Resource_id" DataSourceID="SqlDSResource"
                                        HeaderText="Resource_name" ListTextField="Resource_name" ListValueField="Resource_id"
                                        UniqueName="Resource_id" ItemStyle-Width="100px">
                                    </telerik:GridDropDownColumn>
                                    <telerik:GridDropDownColumn DataField="Project_id" DataSourceID="SqlDSProjects" HeaderText="Project_name"
                                        ListTextField="Project_name" ListValueField="Project_id" UniqueName="Project_id">
                                    </telerik:GridDropDownColumn>
                                    <telerik:GridBoundColumn DataField="Monday" FilterControlAltText="Filter Monday column"
                                        HeaderText="Monday" SortExpression="Monday" UniqueName="Monday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Tuesday" FilterControlAltText="Filter Tuesday column"
                                        HeaderText="Tuesday" SortExpression="Tuesday" UniqueName="Tuesday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Wednesday" FilterControlAltText="Filter Wednesday column"
                                        HeaderText="Wednesday" SortExpression="Wednesday" UniqueName="Wednesday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Thursday" FilterControlAltText="Filter Thursday column"
                                        HeaderText="Thursday" SortExpression="Thursday" UniqueName="Thursday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Friday" FilterControlAltText="Filter Friday column"
                                        HeaderText="Friday" SortExpression="Friday" UniqueName="Friday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Saturday" FilterControlAltText="Filter Saturday column"
                                        HeaderText="Saturday" SortExpression="Saturday" UniqueName="Saturday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Sunday" FilterControlAltText="Filter Sunday column"
                                        HeaderText="Sunday" SortExpression="Sunday" UniqueName="Sunday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Work_load" FilterControlAltText="Filter Work_load column"
                                        HeaderText="Work_load" SortExpression="Work_load" UniqueName="Work_load" Visible="false"
                                        ReadOnly="true" ItemStyle-Width="40px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Bulk_Ass" FilterControlAltText="Filter Bulk_Ass column"
                                        HeaderText="Bulk_Ass" SortExpression="Bulk_Ass" UniqueName="Bulk_Ass" Visible="false"
                                        ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn ConfirmText="Delete this TimeSheet?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn">
                                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                    </telerik:GridButtonColumn>
                                </Columns>
                                <EditFormSettings>
                                    <PopUpSettings Modal="true" />
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <telerik:RadGrid ID="RadGrid_bulk" runat="server" AllowSorting="true" PageSize="10"
                            BorderStyle="None" Skin="Transparent" OnItemCommand="RadGrid_bulk_ItemCommand"
                            OnUpdateCommand="RadGrid_bulk_UpdateCommand" OnInsertCommand="RadGrid_bulk_InsertCommand"
                            OnItemDataBound="RadGrid_bulk_ItemDataBound" AllowAutomaticInserts="true" AllowAutomaticUpdates="true"
                            Width="100%" OnDeleteCommand="RadGrid_bulk_OnDeleteCommand" Visible="false">
                            <%--EnableViewState="true"--%>
                            <ClientSettings EnableRowHoverStyle="false" AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                                <Selecting AllowRowSelect="True" />
                                <Selecting CellSelectionMode="None"></Selecting>
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="S_no" CommandItemDisplay="Top"
                                EditMode="InPlace" CurrentResetPageIndexAction="SetPageIndexToFirst" Dir="LTR"
                                Frame="Border" TableLayout="Auto">
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridBoundColumn DataField="S_no" FilterControlAltText="Filter S_no column"
                                        HeaderText="S_no" SortExpression="S_no" UniqueName="S_no" Visible="false" ReadOnly="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridDateTimeColumn DataField="Week_endings" HeaderText="Date of Assignment"
                                        SortExpression="Week_endings" UniqueName="Week_endings" ReadOnly="false">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridDropDownColumn DataField="Resource_id" DataSourceID="SqlDSResource"
                                        HeaderText="Resource_name" ListTextField="Resource_name" ListValueField="Resource_id"
                                        UniqueName="Resource_id">
                                    </telerik:GridDropDownColumn>
                                    <telerik:GridDropDownColumn DataField="Project_id" DataSourceID="SqlDSProjects" HeaderText="Project_name"
                                        ListTextField="Project_name" ListValueField="Project_id" UniqueName="Project_id">
                                    </telerik:GridDropDownColumn>
                                    <telerik:GridBoundColumn DataField="Monday" FilterControlAltText="Filter Monday column"
                                        HeaderText="Monday" SortExpression="Monday" UniqueName="Monday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Tuesday" FilterControlAltText="Filter Tuesday column"
                                        HeaderText="Tuesday" SortExpression="Tuesday" UniqueName="Tuesday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Wednesday" FilterControlAltText="Filter Wednesday column"
                                        HeaderText="Wednesday" SortExpression="Wednesday" UniqueName="Wednesday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Thursday" FilterControlAltText="Filter Thursday column"
                                        HeaderText="Thursday" SortExpression="Thursday" UniqueName="Thursday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Friday" FilterControlAltText="Filter Friday column"
                                        HeaderText="Friday" SortExpression="Friday" UniqueName="Friday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Saturday" FilterControlAltText="Filter Saturday column"
                                        HeaderText="Saturday" SortExpression="Saturday" UniqueName="Saturday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Sunday" FilterControlAltText="Filter Sunday column"
                                        HeaderText="Sunday" SortExpression="Sunday" UniqueName="Sunday" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridDateTimeColumn DataField="start_bulk" HeaderText="start_bulk" SortExpression="start_bulk"
                                        UniqueName="start_bulk" ReadOnly="false">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridDateTimeColumn DataField="end_bulk" HeaderText="end_bulk" SortExpression="end_bulk"
                                        UniqueName="end_bulk" ReadOnly="false">
                                    </telerik:GridDateTimeColumn>
                                    <telerik:GridBoundColumn DataField="Work_load" FilterControlAltText="Filter Work_load column"
                                        HeaderText="Work_load" SortExpression="Work_load" UniqueName="Work_load" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="Bulk_Ass" FilterControlAltText="Filter Bulk_Ass column"
                                        HeaderText="Bulk_Ass" SortExpression="Bulk_Ass" UniqueName="Bulk_Ass" Visible="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn ConfirmText="Delete this TimeSheet?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn">
                                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                    </telerik:GridButtonColumn>
                                </Columns>
                                <EditFormSettings>
                                    <PopUpSettings Modal="true" />
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </ContentTemplate>
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>
        <!-- START: Main Container -->
        <div class="mainContainer">
            <!-- START: Header -->
            <div class="headerPanel">
                <img class="metis" src="images/metis_logo.png" alt="METIS Logo">
                <div class="action">
                    <span class="title" runat="server">
                        <%= PrintUserName(Convert.ToString(Session["user"])) %>&nbsp;|&nbsp;</span>&nbsp;<span
                            class="logout"><asp:LinkButton ID="lbLogout" runat="server" OnClick="lbLogout_OnClick"
                                Font-Size="Smaller" Height="15px">Logout</asp:LinkButton> </span>

                   &nbsp;|&nbsp;  <asp:Label ID="LabelLanguage" runat="server" Text="Language:" 
                            ></asp:Label>  <asp:DropDownList ID="DropDownListLanguage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownListLanguage_SelectedIndexChanged">
                <asp:ListItem Value="English" Text="English"></asp:ListItem>
                <asp:ListItem Value="Russian" Text="Russian"></asp:ListItem>
                 <asp:ListItem Value="kazakh" Text="Kazakh"></asp:ListItem>
                      </asp:DropDownList>
                </div>
            </div>
            <!-- END: Header -->
            <!-- START: Content -->
            <div class="mainContentPanel">
                <!-- START: Menu -->
                <ul class="menu">
                    <li class="selected"><a tabindex="0" href="ResSum.aspx"><asp:Label ID="lblResourceSummary" runat="server" Text="Label"></asp:Label></a></li>
                    <li><a tabindex="1" href="ProjectSum.aspx"><asp:Label ID="lblProjectSummary" runat="server" Text="Label"></asp:Label></a></li>
                    <span class="title" runat="server">
                        <ul class="menu">
                            <li><a tabindex="2" href="Assignments.aspx"><asp:Label ID="lblAssignments" runat="server" Text="Label"></asp:Label></a></li>
                            <%--<li><a tabindex="3" href="UpComingProj.aspx">Upcoming Projects</a></li>--%>
                            <%--<li id="RFSs" runat="server"><a tabindex="4" href="RFS.aspx">RFS</a></li>--%>
                        </ul>
                    </span>
                </ul>
                <!-- END: Menu -->
                <!-- START: content -->

                 <div class="contentPanel" runat="server" id="DivMainContentDashboard" visible="false">

                      <table border="0" cellspacing="5" cellpadding="5" width="100%">
                          <tr>
                              <td  style="float:right">

                                  <asp:LinkButton ID="LinkButtonBack" runat="server" OnClick="LinkButtonBack_Click" ToolTip="Back">
                                        <asp:Image ID="ImageBAck" runat="server" ImageUrl="images/BAck.png" Width="25px"
                                            Height="21px" BorderWidth="0" />
                                    </asp:LinkButton>
                              </td>
                          </tr>
                      </table>
                     
                      <table border="0" cellspacing="5" cellpadding="5" width="100%">
                          <tr>
                              <td>

                                   <br /><br /><br />
                     put ifram here
                              </td>
                          </tr>
                      </table>

                    
                 </div>
                <div class="contentPanel" runat="server" id="DivMainContent">
                    <!-- START: Filer Area -->
                    <div class="filter">
                        <table border="0" cellspacing="5" cellpadding="5" width="100%">
                            <tr>
                                <!-- Resource -->
                                <td>
                                    <telerik:RadComboBox ID="comResource" runat="server" AllowCustomText="True"
                                        Filter="Contains" Height="23px" MaxHeight="150px" MaxLength="50"
                                        NoWrap="True" Sort="Ascending" Width="184px" AutoPostBack="True" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="comResource_SelectedIndexChanged" TabIndex="1">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Selected="True" Text="All"
                                                Value="All" />
                                        </Items>
                                    </telerik:RadComboBox>

                                </td>
                                <!-- Department -->
                                <td>
                                    <telerik:RadComboBox ID="comDepartment" runat="server" Filter="Contains"
                                        Height="25px" MaxHeight="150px" MaxLength="50" Width="177px"
                                        AutoPostBack="True" AppendDataBoundItems="True"
                                        OnSelectedIndexChanged="comDepartment_SelectedIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem runat="server" Selected="True" Text="All"
                                                Value="All" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <!-- Week Starting -->
                                <td>
                                    <telerik:RadDatePicker ID="dpWeekStarting" runat="server" ShowPopupOnFocus="false"
                                        DatePopupButton-Visible="true" Width="160" MinDate="01/01/2012" MaxDate="12/31/2200"
                                        OnSelectedDateChanged="OnSelectedDateChanged" AutoPostBack="true" Visible="true">
                                        <Calendar OnDayRender="Calendar_OnDayRender" DisabledDayStyle-BackColor="Gray">
                                            <ClientEvents OnDayRender="OnDayRender" />
                                            <DisabledDayStyle BackColor="Gray"></DisabledDayStyle>
                                        </Calendar>
                                        <DatePopupButton Visible="true" CssClass="" ImageUrl="" HoverImageUrl="" TabIndex="-1"></DatePopupButton>
                                        <DateInput ID="DateInput1" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd"
                                            LabelWidth="" Width="160px" AutoPostBack="True" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                                <!-- Week Ending -->
                                <td>
                                    <telerik:RadDatePicker ID="dpEnding" runat="server" ShowPopupOnFocus="false" DatePopupButton-Visible="true"
                                        Width="160" MinDate="01/01/1800" MaxDate="12/31/2200" OnSelectedDateChanged="OnSelectedDateChanged"
                                        AutoPostBack="true" Visible="true">
                                        <Calendar OnDayRender="EndCalendar_OnDayRender" DisabledDayStyle-BackColor="Gray">
                                            <ClientEvents OnDayRender="OnDayRender" />
                                            <DisabledDayStyle BackColor="Gray"></DisabledDayStyle>
                                        </Calendar>
                                        <DatePopupButton Visible="true" CssClass="" ImageUrl="" HoverImageUrl="" TabIndex="-1"></DatePopupButton>
                                        <DateInput ID="DateInput2" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd"
                                            LabelWidth="" Width="160px" AutoPostBack="True" runat="server">
                                        </DateInput>
                                    </telerik:RadDatePicker>
                                </td>
                                <!-- ResourceView -->
                                <td width="150" style="font-size: 13px">
                                    <asp:DropDownList ID="ddlResourceView" runat="server" OnSelectedIndexChanged="ddlResourceView_OnSelectedIndexChanged"
                                        AppendDataBoundItems="true" AutoPostBack="True" Width="225px">
                                        <Items>
                                            <asp:ListItem Text="Show Resource Allocation" Value="1" Selected="True" />
                                            <asp:ListItem Text="Show Resource Availability" Value="2" />
                                        </Items>
                                    </asp:DropDownList>
                                </td>
                                <!-- CheckBox -->
                                <td width="250" style="font-size: 13px">
                                    <asp:CheckBox ID="chkavailres" Text="Show only available resource" TextAlign="Right"
                                        AutoPostBack="True" OnCheckedChanged="Check" runat="server" Visible="false" />
                                </td>
                                <!-- Report Type -->
                                <td width="0">
                                    <telerik:RadComboBox ID="cmbReportType" runat="server" CssClass="CMB" AutoPostBack="true"
                                        OnSelectedIndexChanged="cmbReportType_OnSelectedIndexChanged" EmptyMessage="Select Weekly/Daily"
                                        AccessKey="r" Visible="false">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="Weekly" Value="1" Selected="true" />
                                            <telerik:RadComboBoxItem Text="Daily" Value="2" />
                                        </Items>
                                    </telerik:RadComboBox>
                                </td>
                                <td>&nbsp;
                                </td>
                                <td align="right" width="30%">
                                    <asp:ImageButton Text="Email" runat="server" OnClick="Email_Click" Visible="true"
                                        ID="btnEmail" ImageUrl="images/mail-send.png" ToolTip="Email"></asp:ImageButton>
                                </td>
                                <td align="right" width="20">
                                    <asp:ImageButton Text="Export" runat="server" OnClick="Export_Click" Visible="true"
                                        ID="btnExport" ImageUrl="images/excelicon.png" Width="25px" Height="21px" ToolTip="Export"></asp:ImageButton>
                                </td>
                                <td align="right" width="20">
                                    <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="Refresh" ToolTip="Refresh">
                                        <asp:Image ID="imgRefresh" runat="server" ImageUrl="images/icon_referesh.png" Width="25px"
                                            Height="21px" BorderWidth="0" />
                                    </asp:LinkButton>
                                   </td>
                                <td align="right" width="20">
                                    <asp:LinkButton ID="ButtonDashboard" runat="server" OnClick="ButtonDashboard_Click" ToolTip="Dashboard">
                                        <asp:Image ID="ImageDashboard" runat="server" ImageUrl="images/BIIcon.png" Width="25px"
                                            Height="21px" BorderWidth="0" />
                                    </asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <%--<a href="#"><img src="/image/icon_expand.png" alt="Expand" width="23" height="19" border="0"></a>--%>
                                    <asp:ImageButton ID="btnExpandAll" runat="server" Text="Expand All" OnClick="btnExpandAll_Click"
                                        AccessKey="." Font-Size="Smaller" ImageUrl="images/icon_expand.png" Visible="false"></asp:ImageButton>
                                    <asp:ImageButton ID="btnCollapseAll" runat="server" Text="Collapse All" OnClick="btnCollapseAll_Click"
                                        AccessKey="," Font-Size="Smaller" ImageUrl="images/icon_collapse.png" Visible="false"></asp:ImageButton>
                                    <%--<a href="#"><img src="/image/icon_collapse.png" alt="Collapse" width="23" height="19" border="0"></a>--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- END: Filer Area -->
                    <telerik:RadGrid ID="RadGrid_weekly" runat="server" CellSpacing="0" Height="500px"
                        Skin="Office2007" GridLines="Both" AllowSorting="True" OnDetailTableDataBind="RadGrid_weekly_DetailTableDataBind"
                        OnPreRender="RadGrid_weekly_PreRender" Width="99%" AutoGenerateColumns="false"
                        OnUpdateCommand="RadGrid_weekly_UpdateCommand" OnItemCreated="RadGrid_weekly_ItemCreated"
                        OnItemCommand="RadGrid_weekly_ItemCommand" OnItemDataBound="RadGrid_weekly_ItemDataBound" OnDataBinding="RadGrid_weekly_DataBinding"
                        OnNeedDataSource="RadGrid_weekly_NeedDataSource" OnGridExporting="RadGrid_weekly_Export">
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                        <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="false" ClientEvents-OnRowClick="RowClick">
                            <Selecting CellSelectionMode="SingleCell" AllowRowSelect="false"></Selecting>
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="1" />
                            <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true"
                                ExpandDetailTableKey="RightArrow" CollapseDetailTableKey="LeftArrow"></KeyboardNavigationSettings>
                        </ClientSettings>
                        <%--Start of --%>
                        <MasterTableView Name="MasterTable" Width="99%" HeaderStyle-Width="60px" AutoGenerateColumns="true"
                            HeaderStyle-Wrap="false" HeaderStyle-Font-Size="Smaller" TableLayout="Fixed"
                            DataKeyNames="Resource_id" AlternatingItemStyle-CssClass="color1" ClientDataKeyNames="Resource_id">
                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Name" UniqueName="UName" SortExpression="Resource_name" FilterControlAltText="Filter Resource_name column"
                                    DataField="Resource_name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains"
                                    HeaderStyle-Width="360px">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="targetControl2" runat="server" Text='<%# Eval("Resource_name") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <HeaderStyle Width="360px"></HeaderStyle>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <DetailTables>
                                <telerik:GridTableView runat="server" DataKeyNames="Project_id" CommandItemDisplay="None"
                                    EditMode="InPlace" TabIndex="-1" Name="DetailTable" Width="98%" AutoGenerateColumns="true" 
                                    HeaderStyle-Font-Size="Smaller" ShowHeadersWhenNoRecords="false">
                                    <Columns>
                                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                            HeaderStyle-Width="40px">
                                            <HeaderStyle Width="40px"></HeaderStyle>
                                        </telerik:GridEditCommandColumn>
                                        <telerik:GridTemplateColumn HeaderText="Project" UniqueName="UProject" DataField="Project" HeaderStyle-Width="220px">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="LnkProject" runat="server" Text='<%# Eval("Project") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle Width="220px"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn UniqueName="URole" HeaderText="Role" DataField="Role" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:HyperLink runat="server" Text='<%# Eval("Role_Title") %>'></asp:HyperLink>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px"></HeaderStyle>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <HeaderStyle CssClass="color2" />
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
                            <AlternatingItemStyle CssClass="color1"></AlternatingItemStyle>
                            <HeaderStyle Width="60px"></HeaderStyle>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <%--Other Grids--%>
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <table style="width: 15%;" align="right">
                    <tr>
                        <td class="style1">
                            <asp:Label ID="Label1" runat="server" Text="Overloaded" Font-Bold="True"></asp:Label>
                            &nbsp;</td>
                        <td class="style1">
                            <asp:TextBox ID="TextBox1" runat="server" BackColor="#E34234" ReadOnly="True"
                                Width="39px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Partially Loaded" Font-Bold="True"></asp:Label>
                            &nbsp;&nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" BackColor="#F4F41E" ReadOnly="True"
                                Width="39px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Underloaded" Font-Bold="True"></asp:Label>
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server" BackColor="#FFC7CE" ReadOnly="True"
                                Width="39px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Full Loaded" Font-Bold="True"></asp:Label>
                            &nbsp;</td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server" BackColor="#1C881C" ReadOnly="True"
                                Width="39px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                    <br />
                    &nbsp;<asp:Label ID="Label6" runat="server" Text="No. of Employees:"
                        Font-Bold="True" Font-Size="Medium"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label5" runat="server" Text="Label" Font-Bold="True"
                    Font-Size="Medium"></asp:Label>
                    <br />
                    <br />
                    &nbsp;<asp:Label ID="Label8" runat="server" Text="Utilization Percentage (For Selected Period):"
                        Font-Bold="True" Font-Size="Medium"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label7" runat="server" Text="Label" Font-Bold="True"
                    Font-Size="Medium"></asp:Label>
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <br />
                    &nbsp;
                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="Medium"
                    Text="Utilization Percentage (Next 4 Weeks):"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="Medium"
                    Text="Label"></asp:Label>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" OnDetailTableDataBind="RadGrid1_DetailTableDataBind"
                        GridLines="None" AutoGenerateColumns="False" Width="1050px" AllowPaging="True"
                        PageSize="15" OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand"
                        AllowSorting="True" OnUpdateCommand="RadGrid1_UpdateCommand" OnInsertCommand="RadGrid1_InsertCommand"
                        OnDeleteCommand="RadGrid1_OnDeleteCommand" Visible="False" Skin="Office2007">
                        <FilterMenu EnableImageSprites="False">
                        </FilterMenu>
                        <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="false">
                            <Selecting CellSelectionMode="None"></Selecting>
                            <Selecting AllowRowSelect="false" />
                            <ClientEvents OnActiveRowChanged="ActiveRowChanged" />
                            <KeyboardNavigationSettings AllowActiveRowCycle="true" AllowSubmitOnEnter="true"
                                ExpandDetailTableKey="RightArrow" CollapseDetailTableKey="LeftArrow" FocusKey="RightArrow" />
                        </ClientSettings>
                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="Resource_id" Name="MasterTable"
                            CommandItemDisplay="Top">
                            <CommandItemSettings ShowAddNewRecordButton="false" />
                            <DetailTables>
                                <telerik:GridTableView runat="server" DataKeyNames="S_no" CommandItemDisplay="Top"
                                    EditMode="InPlace" Name="DetailTable" TabIndex="-1" AllowPaging="false">
                                    <ParentTableRelation>
                                        <telerik:GridRelationFields DetailKeyField="Resource_id" MasterKeyField="Resource_id" />
                                    </ParentTableRelation>
                                    <HeaderStyle CssClass="color2" />
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </ExpandCollapseColumn>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>
                                    <Columns>
                                        <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn">
                                        </telerik:GridEditCommandColumn>
                                        <telerik:GridBoundColumn DataField="id" DataType="System.Int32" FilterControlAltText="Filter id column"
                                            HeaderText="ID" ReadOnly="True" SortExpression="id" UniqueName="id">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridDateTimeColumn DataField="Week_endings" HeaderText="Week_endingsT" SortExpression="Week_endings"
                                        UniqueName="Week_endings" ReadOnly="false" DataType="System.DateTime">
                                    </telerik:GridDateTimeColumn>--%>
                                        <telerik:GridDropDownColumn DataField="Resource_id" DataSourceID="SqlDSResource"
                                            HeaderText="Resource_name" ListTextField="Resource_name" ListValueField="Resource_id"
                                            UniqueName="Resource_id">
                                        </telerik:GridDropDownColumn>
                                        <telerik:GridDropDownColumn DataField="Project_id" DataSourceID="SqlDSProjects" HeaderText="Project_name"
                                            ListTextField="Project_name" ListValueField="Project_id" UniqueName="Project_id">
                                        </telerik:GridDropDownColumn>
                                        <telerik:GridBoundColumn DataField="sunday" DataType="System.Double" FilterControlAltText="Filter sunday column"
                                            HeaderText="sunday" SortExpression="sunday" UniqueName="sunday">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="monday" DataType="System.Double" FilterControlAltText="Filter monday column"
                                            HeaderText="monday" SortExpression="monday" UniqueName="monday">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="tuesday" DataType="System.Double" FilterControlAltText="Filter tuesday column"
                                            HeaderText="tuesday" SortExpression="tuesday" UniqueName="tuesday">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="wednesday" DataType="System.Double" FilterControlAltText="Filter wednesday column"
                                            HeaderText="wednesday" SortExpression="wednesday" UniqueName="wednesday">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="thursday" DataType="System.Double" FilterControlAltText="Filter thursday column"
                                            HeaderText="thursday" SortExpression="thursday" UniqueName="thursday">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="friday" DataType="System.Double" FilterControlAltText="Filter friday column"
                                            HeaderText="friday" SortExpression="friday" UniqueName="friday">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="saturday" DataType="System.Double" FilterControlAltText="Filter saturday column"
                                            HeaderText="saturday" SortExpression="saturday" UniqueName="saturday">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridButtonColumn ConfirmText="Delete this TimeSheet?" ConfirmDialogType="RadWindow"
                                            CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                            UniqueName="DeleteColumn">
                                            <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                        </telerik:GridButtonColumn>
                                    </Columns>
                                </telerik:GridTableView>
                            </DetailTables>
                            <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="Resource_name" FilterControlAltText="Filter Resource_name column"
                                    HeaderText="Resource_name" SortExpression="Resource_name" UniqueName="Resource_name"
                                    Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" SortExpression="Resource_name" FilterControlAltText="Filter Resource_name column"
                                    DataField="Resource_name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="targetControl1" runat="server" Text='<%# Eval("Resource_name") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Resource_id" FilterControlAltText="Filter Resource_id column"
                                    HeaderText="Resource_id" ReadOnly="True" SortExpression="Resource_id" UniqueName="Resource_id"
                                    Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Week_ending" SortExpression="Week_ending"
                                    UniqueName="Week_endingTemplate" Visible="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbWeek_ending" runat="server" OnClick="lbWeek_ending_OnClick"
                                            Text='<%# Eval("Week_ending") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" Wrap="false" />
                                    <HeaderStyle Width="100px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Monday" SortExpression="Monday" UniqueName="MondayTemplate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbMonday" runat="server" OnClick="lbMonday_OnClick" Text='<%# Eval("Monday") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" Wrap="false" />
                                    <HeaderStyle Width="70px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Monday" FilterControlAltText="Filter Monday column"
                                    HeaderText="Monday" SortExpression="Monday" UniqueName="Monday" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Tuesday" SortExpression="Tuesday" UniqueName="TuesdayTemplate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbTuesday" runat="server" Text='<%# Eval("Tuesday") %>' OnClick="lbTuesday_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" Wrap="false" />
                                    <HeaderStyle Width="70px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Tuesday" FilterControlAltText="Filter Tuesday column"
                                    HeaderText="Tuesday" SortExpression="Tuesday" UniqueName="Tuesday" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Wednesday" SortExpression="Wednesday" UniqueName="WednesdayTemplate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbWednesday" runat="server" OnClick="lbWednesday_OnClick" Text='<%# Eval("Wednesday") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="75px" Wrap="false" />
                                    <HeaderStyle Width="75px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Wednesday" FilterControlAltText="Filter Wednesday column"
                                    HeaderText="Wednesday" SortExpression="Wednesday" UniqueName="Wednesday" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Thursday" SortExpression="Thursday" UniqueName="ThursdayTemplate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbThursday" runat="server" OnClick="lbThursday_OnClick" Text='<%# Eval("Thursday") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" Wrap="false" />
                                    <HeaderStyle Width="70px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Thursday" FilterControlAltText="Filter Thursday column"
                                    HeaderText="Thursday" SortExpression="Thursday" UniqueName="Thursday" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Friday" SortExpression="Friday" UniqueName="FridayTemplate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbFriday" runat="server" OnClick="lbFriday_OnClick" Text='<%# Eval("Friday") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" Wrap="false" />
                                    <HeaderStyle Width="70px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Friday" FilterControlAltText="Filter Friday column"
                                    HeaderText="Friday" SortExpression="Friday" UniqueName="Friday" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Saturday" SortExpression="Saturday" UniqueName="SaturdayTemplate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSaturday" runat="server" OnClick="lbSaturday_OnClick" Text='<%# Eval("Saturday") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" Wrap="false" />
                                    <HeaderStyle Width="70px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Saturday" FilterControlAltText="Filter Saturday column"
                                    HeaderText="Saturday" SortExpression="Saturday" UniqueName="Saturday" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Sunday" SortExpression="Sunday" UniqueName="SundayTemplate">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSunday" runat="server" OnClick="lbSunday_OnClick" Text='<%# Eval("Sunday") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="70px" Wrap="false" />
                                    <HeaderStyle Width="70px" Wrap="false" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="Sunday" FilterControlAltText="Filter Sunday column"
                                    HeaderText="Sunday" SortExpression="Sunday" UniqueName="Sunday" Visible="false">
                                </telerik:GridBoundColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <%--For VS2008 replace RadScriptManager with ScriptManager--%>
                </div>
            </div>
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
        <!-- END: content -->
    </form>
</body>
</html>
