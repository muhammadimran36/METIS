<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpComingProj.aspx.cs" Inherits="streebo.METIS.UI.UpComingProj" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>METIS - Resource Planning System</title>
    <link rel="stylesheet" type="text/css" href="styles/main.css" />
    <%--<telerik:RadCodeBlock runat="server" ID="radCodeBlock4">
    <script type="text/javascript">
        function Filter(colName, filtertxt) {
           
            var filterTxt = document.getElementById(filtertxt);
            var MasterTable = $find('<%=rgBulkAssignment.ClientID %>').get_masterTableView();
            //var MasterTable = document.getElementById('<%=rgBulkAssignment.ClientID %>');
             
            
            var hidden = document.getElementById('<%=Hidden1.ClientID %>');
            hidden.value = colName;
           
            
            if (filterTxt.value.length > 0) {
                
                MasterTable.filter(colName, filterTxt.value, Telerik.Web.UI.GridFilterFunction.StartsWith);
                alert("Filter");
            }
            else {
                MasterTable.filter(colName, filterTxt.value, Telerik.Web.UI.GridFilterFunction.NoFilter);
                alert("No Filter");
            }
        }
           </script>
     </telerik:RadCodeBlock> 
    <script type="text/javascript">
        function FocusFilter(filter) {
            var input = document.getElementById(filter);
            if (input.createTextRange) {
                var FieldRange = input.createTextRange();
                FieldRange.moveStart('character', input.value.length);
                FieldRange.select();
            }
        }  
        </script>--%>
    <style type="text/css">
        #Checkbox1
        {
            width: 65px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!-- START: Main Container -->
    <div class="mainContainer">
        <!-- START: Header -->
        <div class="headerPanel">
            <img class="metis" src="images/metis_logo.png" alt="METIS Logo">
            <div class="action">
            <asp:placeholder runat="server"><span class="title"><%= PrintUserName(Convert.ToString(Session["user"])) %>&nbsp;|&nbsp;</span></asp:placeholder> 
                <span class="logout">
                    <asp:LinkButton ID="lbLogout" runat="server" OnClick="lbLogout_OnClick" Font-Size="Smaller"
                        Height="15px">Logout</asp:LinkButton></span>
            </div>
        </div>
        <!-- END: Header -->
        <!-- START: Content -->
        <div class="mainContentPanel">
            <!-- START: Menu -->
            <ul class="menu">
                <li><a tabindex="0" href="ResSum.aspx">Resource Summary</a></li>
                <li><a tabindex="1" href="ProjectSum.aspx">Project Summary</a></li>
                <li><a tabindex="2" href="Assignments.aspx">Assignments</a></li>
                <li class="selected"><a tabindex="3" href="UpComingProj.aspx">Upcoming Projects</a></li>
                <%--<li id="RFSs" runat="server" ><a tabindex="4" href="RFS.aspx">RFS</a></li>--%>
            </ul>
            <!-- END: Menu -->
            <!-- START: content -->
            <div class="contentPanel" id="main" runat="server">
                <!-- content start -->
                <input id="Hidden1" runat="server" name="Hidden1" type="hidden" />
                <div>
                    <telerik:RadScriptManager ID="radScriptMgr" runat="server">
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
                            <telerik:AjaxSetting AjaxControlID="rgUpComingProject">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgUpComingProject" LoadingPanelID="RadAjaxLoadingPanel2" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="rgActionItem">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgActionItem" LoadingPanelID="RadAjaxLoadingPanel2" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" />
                    <telerik:RadWindowManager ID="RadWindowConfirmationDialog" runat="server" EnableShadow="true">
                    </telerik:RadWindowManager>
                </div>
                <telerik:RadCodeBlock runat="server" ID="radCodeBlock5">

                    <script type="text/javascript">
                        function rgUpComingProjectsshowFilterItem() {
                            $find('<%=rgUpComingProject.ClientID %>').get_masterTableView().showFilterItem();
                        }
                        function rgUpComingProjectshideFilterItem() {
                            $find('<%=rgUpComingProject.ClientID %>').get_masterTableView().hideFilterItem();
                        }
                    </script>

                </telerik:RadCodeBlock>
                <asp:DropDownList ID="ddlDepartment" runat="server" OnSelectedIndexChanged="ddlDepartment_OnSelectedIndexChanged"
                    AppendDataBoundItems="true" AutoPostBack="True" Width="175px">
                    <Items><asp:ListItem Text="All"/></Items>
                </asp:DropDownList>
                <br />
                <br />
                <b>Upcoming Projects</b>
                <br />
                <br />
                <div style="display: inline-block; width: 97%">
                    <div style="display: inline-block; vertical-align: super;">
                        Show filtering item
                        <input id="Radio1" type="radio" runat="server" name="showHideGroup1" checked="true"
                            onclick="rgUpComingProjectsshowFilterItem()" /><label for="Radio1">Yes</label>
                        <input id="Radio2" type="radio" runat="server" name="showHideGroup1" onclick="rgUpComingProjectshideFilterItem()" /><label
                            for="Radio2">No</label>
                    </div>
                </div>
                <telerik:RadGrid ID="rgUpComingProject" runat="server" GridLines="None" AllowPaging="True"
                    AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgUpComingProject_NeedDataSource"
                    OnPreRender="rgUpComingProject_PreRender" OnDeleteCommand="rgUpComingProject_DeleteCommand"
                    OnInsertCommand="rgUpComingProject_InsertCommand" OnUpdateCommand="rgUpComingProject_UpdateCommand"
                    EnableAJAX="True" OnItemCreated="rgUpComingProject_ItemCreated" OnItemDataBound="rgUpComingProject_ItemDataBound"
                    OnItemCommand="rgUpComingProject_ItemCommand" AllowFilteringByColumn="True" TabIndex="10"
                    PageSize="50" CellSpacing="0">
                    <GroupingSettings CaseSensitive="false" />
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                    <ClientSettings AllowKeyboardNavigation="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                        <KeyboardNavigationSettings AllowActiveRowCycle="true" AllowSubmitOnEnter="true"
                            EnableKeyboardShortcuts="true" InitInsertKey="N" />
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="pkID" EditMode="InPlace"
                        CommandItemDisplay="Top">
                        <CommandItemSettings ExportToPdfText="Export to PDF" AddNewRecordText="Add new record"
                            ShowAddNewRecordButton="true"></CommandItemSettings>
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                HeaderStyle-Width="50px">
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridTemplateColumn UniqueName="Project" HeaderText="Project" SortExpression="Project_name"
                                DataField="Project_name" AllowFiltering="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                <ItemTemplate>
                                    <%# Eval("Project_name")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox MaxLength="100" Width="90%" ID="txtProject" Columns="30" runat="Server">
                                    </asp:TextBox>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox MaxLength="100" Width="90%" ID="txtProject" Columns="30" runat="Server">
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="DesiredStart" HeaderText="Desired Start"
                                SortExpression="DesiredStart" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("DesiredStart")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <div style="margin-top: 31px">
                                        <telerik:RadDatePicker ID="DesiredStart" Width="110px" runat="Server" AppendDataBoundItems="true">
                                            <DateInput Font-Size="XX-Small" ID="DateInput2" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd"
                                                LabelWidth="" AutoPostBack="True" runat="server">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDesiredStart" ControlToValidate="DesiredStart"
                                            ErrorMessage="Enter a Desired Start Date!"></asp:RequiredFieldValidator>
                                    </div>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <div style="margin-top: 31px">
                                        <telerik:RadDatePicker ID="DesiredStart" Width="110px" runat="Server" AppendDataBoundItems="true">
                                            <DateInput ID="DateInput3" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd"
                                                LabelWidth="" AutoPostBack="True" runat="server">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDesiredStart" ControlToValidate="DesiredStart"
                                            ErrorMessage="Enter a Desired Start Date!"></asp:RequiredFieldValidator>
                                    </div>
                                </EditItemTemplate>
                                <HeaderStyle Width="125px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="PlannedStart" HeaderText="Planned Start"
                                SortExpression="PlannedStart" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("PlannedStart")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <div style="margin-top: 31px">
                                        <telerik:RadDatePicker ID="PlannedStart" runat="Server" AppendDataBoundItems="true"
                                            Width="110px">
                                            <DateInput ID="DateInput5" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd"
                                                LabelWidth="" AutoPostBack="True" runat="server">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPlannedStart" ControlToValidate="PlannedStart"
                                            ErrorMessage="Enter a Planned Start Date!"></asp:RequiredFieldValidator>
                                    </div>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <div style="margin-top: 31px">
                                        <telerik:RadDatePicker ID="PlannedStart" runat="Server" AppendDataBoundItems="true"
                                            Width="110px">
                                            <DateInput ID="DateInput4" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd"
                                                LabelWidth="" AutoPostBack="True" runat="server">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvPlannedStart" ControlToValidate="PlannedStart"
                                            ErrorMessage="Enter a Planned Start Date!"></asp:RequiredFieldValidator>
                                    </div>
                                </EditItemTemplate>
                                <HeaderStyle Width="125px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Resources" HeaderText="Resources" SortExpression="Resources"
                                AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("Resources")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="txtResource" runat="Server" TextMode="MultiLine" Width="160px" Height="85px">
                                    </asp:TextBox>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtResource" runat="Server" TextMode="MultiLine" Width="160px" Height="85px">
                                    </asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="200px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Comments" HeaderText="Comments" SortExpression="Comments"
                                AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("Comments")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="txtComment" runat="Server" TextMode="MultiLine" Width="160px" Height="85px">
                                    </asp:TextBox>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtComment" runat="Server" TextMode="MultiLine" Width="160px" Height="85px">
                                    </asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="200px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete?" ConfirmDialogType="Classic"
                                CommandName="Delete" ConfirmTitle="Delete Confirmation" ButtonType="ImageButton" Text="Delete"
                                UniqueName="DeleteColumn" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle">
                                <HeaderStyle Width="50px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                </telerik:RadGrid>
                <telerik:RadCodeBlock runat="server" ID="radCodeBlock6">

                    <script type="text/javascript">
                        function rgActionItemshowFilterItem() {
                            $find('<%=rgActionItem.ClientID %>').get_masterTableView().showFilterItem();
                        }
                        function rgActionItemhideFilterItem() {
                            $find('<%=rgActionItem.ClientID %>').get_masterTableView().hideFilterItem();
                        }
                    </script>

                </telerik:RadCodeBlock>
                <br />
                <br />
                <br />
                <b>Action Items</b>
                <br />
                <br />
                <div style="display: inline-block; width: 97%">
                    <div style="width:50%;display: inline-block; vertical-align: super;">
                        Show filtering item
                        <input id="Radio3" type="radio" runat="server" name="showHideGroup2" checked="true"
                            onclick="rgActionItemshowFilterItem()" /><label for="Radio1">Yes</label>
                        <input id="Radio4" type="radio" runat="server" name="showHideGroup2" onclick="rgActionItemhideFilterItem()" /><label
                            for="Radio2">No</label>
                    </div>
                    <div style="width:49%;display:inline-block;text-align:right">
                         <div style="display:inline-block;vertical-align: super;">
                            <asp:CheckBox ID="chkbArchive" Text="Show Archive" runat="server" TextAlign="Right"
                                AutoPostBack="True" OnCheckedChanged="isArchive_CheckedChanged1" />
                        </div>
                        <div style="display: inline-block;">
                            <asp:ImageButton  Style="" Text="Email"  runat="server" Visible="true" ID="btnEmail"
                                ImageUrl="images/mail-send.png" onclick="btnEmail_Click" ToolTip="Email"></asp:ImageButton>
                        </div>
                    </div>
                </div>
                <telerik:RadGrid ID="rgActionItem" runat="server" GridLines="None" AllowPaging="True"
                    AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgActionItem_NeedDataSource"
                    OnPreRender="rgActionItem_PreRender" OnDeleteCommand="rgActionItem_DeleteCommand"
                    OnInsertCommand="rgActionItem_InsertCommand" OnUpdateCommand="rgActionItem_UpdateCommand"
                    EnableAJAX="True" OnItemCreated="rgActionItem_ItemCreated" OnItemDataBound="rgActionItem_ItemDataBound"
                    OnItemCommand="rgActionItem_ItemCommand" AllowFilteringByColumn="True" TabIndex="10"
                    PageSize="50" CellSpacing="0">
                    <GroupingSettings CaseSensitive="false" />
                    <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                    <ClientSettings AllowKeyboardNavigation="true">
                        <Selecting AllowRowSelect="true"></Selecting>
                        <KeyboardNavigationSettings AllowActiveRowCycle="true" AllowSubmitOnEnter="true"
                            EnableKeyboardShortcuts="true" InitInsertKey="N" />
                    </ClientSettings>
                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="pkID" EditMode="InPlace"
                        CommandItemDisplay="Top">
                        <CommandItemSettings AddNewRecordText="Add new record" ShowAddNewRecordButton="true">
                        </CommandItemSettings>
                        <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                HeaderStyle-Width="50px">
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </telerik:GridEditCommandColumn>
                            <telerik:GridTemplateColumn UniqueName="ActionItem" HeaderText="Action Item" SortExpression="Action_Item"
                                DataField="Action_Item" AllowFiltering="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                <ItemTemplate>
                                    <%# Eval("Action_Item")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="txtActionItem" Width="90%" runat="Server" TextMode="MultiLine" Height="85px">
                                    </asp:TextBox>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtActionItem" Width="90%" runat="Server" TextMode="MultiLine" Height="85px">
                                    </asp:TextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Resource_name" HeaderText="Assigned To" SortExpression="Resource_name"
                                DataField="Resource_name" AllowFiltering="true" CurrentFilterFunction="Contains"
                                ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                <ItemTemplate>
                                    <%# Eval("Resource_name")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <asp:DropDownList Style="width: 180px;" ID="comboResourceName" Columns="30" runat="Server"
                                        AppendDataBoundItems="true" AutoPostBack="true">
                                    </asp:DropDownList>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList Style="width: 180px;" ID="comboResourceName" Columns="30" runat="Server"
                                        AppendDataBoundItems="true" Width="120px">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderStyle Width="200px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Target" HeaderText="Target Date" SortExpression="Target"
                                AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("Target")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <div style="margin-top: 16px">
                                        <telerik:RadDatePicker ID="Target" runat="Server" AppendDataBoundItems="true" Width="110">
                                            <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth=""
                                                AutoPostBack="True" runat="server">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvTarget" ControlToValidate="Target"
                                            ErrorMessage="Enter a Target Date!"></asp:RequiredFieldValidator>
                                    </div>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <div style="margin-top: 16px">
                                        <telerik:RadDatePicker ID="Target" runat="Server" AppendDataBoundItems="true" Width="110">
                                            <DateInput DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth=""
                                                AutoPostBack="True" runat="server">
                                            </DateInput>
                                        </telerik:RadDatePicker>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvTarget" ControlToValidate="Target"
                                            ErrorMessage="Enter a Target Date!"></asp:RequiredFieldValidator>
                                    </div>
                                </EditItemTemplate>
                                <HeaderStyle Width="125px" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Status" HeaderText="Status" SortExpression="Status"
                                DataField="Status" AllowFiltering="false">
                                <ItemTemplate>
                                    <%# Eval("Status")%>
                                </ItemTemplate>
                                <InsertItemTemplate>
                                    <asp:TextBox ID="txtStatus" runat="Server" Width="35">
                                    </asp:TextBox>
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtStatus" runat="Server" Width="35">
                                    </asp:TextBox>
                                </EditItemTemplate>
                                <HeaderStyle Width="50px" />
                            </telerik:GridTemplateColumn>
                             <telerik:GridButtonColumn ButtonType="ImageButton"  ConfirmTitle="Archive" ConfirmText="Are you sure you want to Archive?"
                                Text="Archive" ConfirmDialogType="Classic" CommandName="Archive" ImageUrl="images/hide.png"
                                UniqueName="ArchiveColumn">
                                <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </telerik:GridButtonColumn>
                            <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete?" ConfirmDialogType="Classic"
                                CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                UniqueName="DeleteColumn" HeaderStyle-Width="50px">
                                <HeaderStyle Width="50px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                            </telerik:GridButtonColumn>
                        </Columns>
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                    </MasterTableView>
                </telerik:RadGrid>
          </div>
    <!-- END: content -->
    </div>
    <!-- END: Content -->
    </div>
    <!-- END: Main Container -->
    <br />
    <br />
    <asp:Label ID="lblErr" runat="server" Font-Bold="true" Font-Size="14px" style="margin:1em;"></asp:Label>
    </form>
</body>
</html>
