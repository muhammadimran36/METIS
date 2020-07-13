<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Assignments.aspx.cs" Inherits="streebo.METIS.UI.Assignments" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head runat="server">
    <title>METIS - Resource Planning System</title>
    <link rel="stylesheet" type="text/css" href="styles/main.css">
</head>
<body>
    <form id="form2" runat="server">
        <!-- START: Main Container -->
        <div class="mainContainer">
            <!-- START: Header -->
            <div class="headerPanel">
                <img class="metis" src="images/metis_logo.png" alt="METIS Logo">
                <div class="action">
                    <span class="title"><%= PrintUserName(Convert.ToString(Session["user"])) %>&nbsp;|&nbsp;</span>
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
                    <li class="selected"><a tabindex="2" href="Assignments.aspx">Assignments</a></li>
                    <%--<li><a tabindex="3" href="UpComingProj.aspx">Upcoming Projects</a></li>--%>
                </ul>
                <!-- END: Menu -->
                <!-- START: content -->
                <div class="contentPanel">
                    <!-- content start -->

                    <div id="divSelection" runat="server">
                        <asp:DropDownList ID="ddlSelection" runat="server" OnSelectedIndexChanged="ddlSelection_SelectedIndexChanged"
                            AutoPostBack="True" TabIndex="3">
                            <asp:ListItem>BulkAssignment</asp:ListItem>
                            <asp:ListItem>Departments</asp:ListItem>
                            <asp:ListItem>Assign Department</asp:ListItem>
                            <asp:ListItem>Assign Resource To Project</asp:ListItem>
                            <asp:ListItem>Mark Leaves</asp:ListItem>
                            <asp:ListItem>Roles</asp:ListItem>
                            <asp:ListItem>Add Project</asp:ListItem>
                            <asp:ListItem>Add Resource</asp:ListItem>
                        </asp:DropDownList>
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
                        </telerik:RadAjaxManager>
                        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" />
                    </div>
                    <%--Needed for JavaScript IntelliSense in VS2010--%><asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true"
                        OnCheckedChanged="CheckBox1_CheckedChanged" Text="Show History" />
                    <div id="divBulkAssignment" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock3">

                            <script type="text/javascript">
                                function rgBulkAssignmentshowFilterItem() {
                                    $find('<%=rgBulkAssignment.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgBulkAssignmenthideFilterItem() {
                                $find('<%=rgBulkAssignment.ClientID %>').get_masterTableView().hideFilterItem();
                            }

                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio7" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgBulkAssignmentshowFilterItem()" /><label for="Radio1">Yes</label>
                            <input id="Radio8" type="radio" runat="server" name="showHideGroup" onclick="rgBulkAssignmenthideFilterItem()" /><label
                                for="Radio2">No</label>&nbsp;&nbsp;&nbsp;&nbsp;
                        </div>
                        <telerik:RadDatePicker ID="RadDatePicker1" Style="display: none;" MinDate="01/01/1900"
                            MaxDate="12/31/2100" runat="server">
                        </telerik:RadDatePicker>
                        <telerik:RadGrid ID="rgBulkAssignment" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgBulkAssignment_NeedDataSource"
                            OnDeleteCommand="rgBulkAssignment_DeleteCommand" OnInsertCommand="rgBulkAssignment_InsertCommand"
                            OnUpdateCommand="rgBulkAssignment_UpdateCommand" EnableAJAX="True" OnItemCreated="rgBulkAssignment_ItemCreated"
                            OnItemDataBound="rgBulkAssignment_ItemDataBound" OnItemCommand="rgBulkAssignment_ItemCommand"
                            AllowFilteringByColumn="true" TabIndex="9" PageSize="15" AllowMultiRowEdit="false">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False"></FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" InitInsertKey="N" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="BulkAssignmentID" EditMode="InPlace"
                                CommandItemDisplay="Top" >
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
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridTemplateColumn UniqueName="WeekEnding" HeaderText="DateOfAssignment"
                                        SortExpression="WeekEnding" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("WeekEnding") %>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderWeekEnding" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput1" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderWeekEnding" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput1" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ResourceName" HeaderText="ResourceName" SortExpression="ResourceName"
                                        DataField="ResourceName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ResourceName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <%--<telerik:RadComboBox ID="comResourceName" runat="server" AppendDataBoundItems="true" CheckBoxes="true"></telerik:RadComboBox>--%>
                                            <asp:DropDownList ID="comResourceName" runat="server" AppendDataBoundItems="true" CheckBoxes="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvcomResourceName" ControlToValidate="comResourceName"
                                                ErrorMessage="Must Select One"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <%--<telerik:RadComboBox ID="comResourceName" runat="server" AppendDataBoundItems="true"></telerik:RadComboBox>--%>
                                            <asp:DropDownList ID="comResourceName" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvcomResourceName" ControlToValidate="comResourceName"
                                                ErrorMessage="Must Select One"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ProjectName" HeaderText="ProjectName" SortExpression="ProjectName"
                                        DataField="ProjectName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ProjectName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboProjectName" Columns="30" runat="Server" AppendDataBoundItems="true"></asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboProjectName" Columns="30" runat="Server" AppendDataBoundItems="true"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="AssignmentTypeName" HeaderText="AssignmentTypeName" SortExpression="ProjectName"
                                        DataField="AssignmentTypeName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("AssignmentTypeName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboAssignmentTypeName" Columns="30" runat="Server" AppendDataBoundItems="true"></asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboAssignmentTypeName" Columns="30" runat="Server" AppendDataBoundItems="true"></asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BulkStartDate" HeaderText="StartDate"
                                        SortExpression="BulkStartDate" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("BulkStartDate")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderStartDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput2" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="calenderStartDate"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderStartDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput3" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="calenderStartDate"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="BulkEndDate" HeaderText="EndDate" SortExpression="BulkEndDate"
                                        DataType="System.DateTime" AllowFiltering="true" ShowFilterIcon="false">
                                        <ItemTemplate>

                                            <%#  Eval ("BulkEndDate")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderEndDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput4" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="calenderEndDate"
                                                ErrorMessage="Enter a End Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderEndDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput5" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="calenderEndDate"
                                                ErrorMessage="Enter a End Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BulkWorkLoad" HeaderText="WorkLoad" SortExpression="BulkWorkLoad"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("BulkWorkLoad")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtWorkLoad" runat="Server" />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvWorlLoad" ControlToValidate="txtWorkLoad"
                                                ErrorMessage="Enter a Work load!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtWorkLoad" runat="Server" />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvWorlLoad" ControlToValidate="txtWorkLoad"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn ConfirmText="Delete this Assignment ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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


                    <div id="divAssignmentHistory" runat="server" style="display: none;">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock6">

                            <script type="text/javascript">
                                function rgAssignmentshowFilterItem() {
                                    $find('<%=rgAssignmentHistory.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgAssignmenthideFilterItem() {
                                $find('<%=rgAssignmentHistory.ClientID %>').get_masterTableView().hideFilterItem();
                            }

                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio13" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgAssignmentshowFilterItem()" /><label for="Radio1">Yes</label>
                            <input id="Radio14" type="radio" runat="server" name="showHideGroup" onclick="rgAssignmenthideFilterItem()" /><label
                                for="Radio2">No</label>&nbsp;&nbsp;&nbsp;&nbsp;
                        </div>

                        <telerik:RadGrid ID="rgAssignmentHistory" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgAssignmentHistory_NeedDataSource"
                            OnDeleteCommand="rgAssignmentHistory_DeleteCommand" OnInsertCommand="rgAssignmentHistory_InsertCommand"
                            OnUpdateCommand="rgAssignmentHistory_UpdateCommand" EnableAJAX="True" OnItemCreated="rgAssignmentHistory_ItemCreated"
                            OnItemDataBound="rgAssignmentHistory_ItemDataBound" OnItemCommand="rgAssignmentHistory_ItemCommand"
                            AllowFilteringByColumn="true" PageSize="50">
                            <GroupingSettings CaseSensitive="false" />

                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" InitInsertKey="N" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="BulkAssignmentID" EditMode="InPlace"
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
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridTemplateColumn UniqueName="WeekEnding" HeaderText="DateOfAssignment"
                                        SortExpression="WeekEnding" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("WeekEnding") %>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderWeekEnding" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput1" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderWeekEnding" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput1" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ResourceName" HeaderText="ResourceName" SortExpression="ResourceName"
                                        DataField="ResourceName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ResourceName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ProjectName" HeaderText="ProjectName" SortExpression="ProjectName"
                                        DataField="ProjectName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ProjectName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboProjectName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboProjectName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BulkStartDate" HeaderText="StartDate"
                                        SortExpression="BulkStartDate" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("BulkStartDate")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderStartDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput2" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="calenderStartDate"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderStartDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput3" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="calenderStartDate"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="BulkEndDate" HeaderText="EndDate" SortExpression="BulkEndDate"
                                        DataType="System.DateTime" AllowFiltering="true" ShowFilterIcon="false">
                                        <ItemTemplate>

                                            <%#  Eval ("BulkEndDate")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderEndDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput4" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="calenderEndDate"
                                                ErrorMessage="Enter a End Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderEndDate" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput5" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="calenderEndDate"
                                                ErrorMessage="Enter a End Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="BulkWorkLoad" HeaderText="WorkLoad" SortExpression="BulkWorkLoad"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("BulkWorkLoad")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtWorkLoad" runat="Server" />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvWorlLoad" ControlToValidate="txtWorkLoad"
                                                ErrorMessage="Enter a Work load!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtWorkLoad" runat="Server" />
                                            <asp:RequiredFieldValidator runat="server" ID="rfvWorlLoad" ControlToValidate="txtWorkLoad"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ConfirmText="Delete this Assignment ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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
                    <div id="divResourceLeaves" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock4">

                            <script type="text/javascript">
                                function rgResourceLeavesshowFilterItem() {
                                    $find('<%=rgResourceLeaves.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgBulkAssignmenthideFilterItem() {
                                $find('<%=rgResourceLeaves.ClientID %>').get_masterTableView().hideFilterItem();
                            }
                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio9" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgResourceLeavesshowFilterItem()" /><label for="Radio9">Yes</label>
                            <input id="Radio10" type="radio" runat="server" name="showHideGroup" onclick="rgResourceLeaveshideFilterItem()" /><label
                                for="Radio10">No</label>
                        </div>
                        <%--For VS2008 replace RadScriptManager with ScriptManager--%>

                        <telerik:RadGrid ID="rgResourceLeaves" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgResourceLeaves_NeedDataSource"
                            OnPreRender="rgResourceLeaves_PreRender"
                            OnDeleteCommand="rgResourceLeaves_DeleteCommand" OnInsertCommand="rgResourceLeaves_InsertCommand"
                            OnUpdateCommand="rgResourceLeaves_UpdateCommand" EnableAJAX="True" OnItemCreated="rgResourceLeaves_ItemCreated"
                            OnItemDataBound="rgResourceLeaves_ItemDataBound" OnItemCommand="rgResourceLeaves_ItemCommand"
                            AllowFilteringByColumn="true" TabIndex="10" PageSize="50">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" InitInsertKey="N" />
                            </ClientSettings>

                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ResourceLeaveHeaderID" EditMode="InPlace"
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
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridTemplateColumn UniqueName="ResourceName" HeaderText="Resource" SortExpression="ResourceName"
                                        DataField="ResourceName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ResourceName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="BulkStartDate" HeaderText="BulkStartDate"
                                        SortExpression="BulkStartDate" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("LeaveStart")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderStartDate" runat="Server" AppendDataBoundItems="true">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="calenderStartDate"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderStartDate" runat="Server" AppendDataBoundItems="true">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvStartDate" ControlToValidate="calenderStartDate"
                                                ErrorMessage="Enter a Start Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="BulkEndDate" HeaderText="BulkEndDate" SortExpression="BulkEndDate"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("LeaveEnd")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="calenderEndDate" runat="Server" AppendDataBoundItems="true">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="calenderEndDate"
                                                ErrorMessage="Enter a End Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="calenderEndDate" runat="Server" AppendDataBoundItems="true">
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEndDate" ControlToValidate="calenderEndDate"
                                                ErrorMessage="Enter a End Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn ConfirmText="Delete this Leave ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
                                        <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                    </telerik:GridButtonColumn>
                                </Columns>

                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <%--<AjaxSettings>
                         <telerik:AjaxSetting AjaxControlID="rgBulkAssignment">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgBulkAssignment" LoadingPanelID="RadAjaxLoadingPanel1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                              <telerik:AjaxSetting AjaxControlID="comboResourceName">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="comboProjectName" LoadingPanelID="RadAjaxLoadingPanel1" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                             <telerik:AjaxSetting AjaxControlID="rgResourceLeaves">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgResourceLeaves" LoadingPanelID="RadAjaxLoadingPanel2" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="rgUpComingProject">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgUpComingProject" LoadingPanelID="RadAjaxLoadingPanel2" />
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>--%>
                    </div>
                    <div id="divDepartment" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock">

                            <script type="text/javascript">
                                function rgDepartmentsshowFilterItem() {
                                    $find('<%=rgDepartments.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgDepartmentshideFilterItem() {
                                $find('<%=rgDepartments.ClientID %>').get_masterTableView().hideFilterItem();
                            }
                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio1" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgDepartmentsshowFilterItem()" /><label for="Radio1">Yes</label>
                            <input id="Radio2" type="radio" runat="server" name="showHideGroup" onclick="rgDepartmentshideFilterItem()" /><label
                                for="Radio2">No</label>
                        </div>
                        <telerik:RadGrid ID="rgDepartments" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgDepartments_NeedDataSource"
                            OnDeleteCommand="rgDepartments_DeleteCommand" OnInsertCommand="rgDepartments_InsertCommand"
                            OnUpdateCommand="rgDepartments_UpdateCommand" EnableAJAX="True" OnItemCreated="rgDepartments_ItemCreated"
                            OnItemDataBound="rgDepartments_ItemDataBound" OnItemCommand="rgDepartments_ItemCommand"
                            AllowFilteringByColumn="True" PageSize="15">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true">
                                <Selecting CellSelectionMode="None" AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="DepartmentID" EditMode="InPlace"
                                CommandItemDisplay="Top" AllowFilteringByColumn="True">
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                        HeaderStyle-Width="50px">
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridTemplateColumn UniqueName="DepartmentID" HeaderText="DepartmentID" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("DepartmentID")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <%--
                                        <telerik:RadButton ID="btnCancelInsert" runat="server" Text="Cancel" CommandName="Cancel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="btnInsert" runat="server" Text="Insert" CommandName="Insert">
                                        </telerik:RadButton>--%>
                                            <asp:TextBox ID="txtDepartmentID" Columns="30" runat="Server" Enabled="false"></asp:TextBox>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDepartmentID" Columns="30" runat="Server" Enabled="false"></asp:TextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DepartmentName" HeaderText="DepartmentName"
                                        DataField="DepartmentName" SortExpression="DepartmentName" AllowFiltering="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("DepartmentName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtDepartmentName" Columns="30" runat="Server"></asp:TextBox>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDepartmentName" Columns="30" runat="Server"></asp:TextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ReportsTo" HeaderText="ReportsTo" SortExpression="ReportsTo"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("ReportsTo")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboReportsTo" Columns="30" runat="Server" AppendDataBoundItems="true">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboReportsTo" Columns="30" runat="Server" AppendDataBoundItems="true">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Active" HeaderText="Status" SortExpression="Active" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("Active")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:CheckBox ID="chkActive" Columns="30" runat="server" Checked="true" AppendDataBoundItems="true"/>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkActive" Columns="30" runat="server" Checked='<%# Eval("Active") %>' AppendDataBoundItems="true"/>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ConfirmText="Delete this Department ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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
                    <div id="divResourceOnProjects" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock2">

                            <script type="text/javascript">
                                function rgResourceOnProjectsshowFilterItem() {
                                    $find('<%=rgResourceOnProjects.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgResourceOnProjectshideFilterItem() {
                                $find('<%=rgResourceOnProjects.ClientID %>').get_masterTableView().hideFilterItem();
                            }
                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio5" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgResourceOnProjectsshowFilterItem()" /><label for="Radio5">Yes</label>
                            <input id="Radio6" type="radio" runat="server" name="showHideGroup" onclick="rgResourceOnProjectshideFilterItem()" /><label
                                for="Radio6">No</label>
                        </div>
                        <telerik:RadGrid ID="rgResourceOnProjects" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgResourceOnProjects_NeedDataSource"
                            OnDeleteCommand="rgResourceOnProjects_DeleteCommand" OnInsertCommand="rgResourceOnProjects_InsertCommand"
                            OnUpdateCommand="rgResourceOnProjects_UpdateCommand" EnableAJAX="True" OnItemCreated="rgResourceOnProjects_ItemCreated"
                            OnItemDataBound="rgResourceOnProjects_ItemDataBound" OnItemCommand="rgResourceOnProjects_ItemCommand"
                            AllowFilteringByColumn="true" PageSize="50">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting CellSelectionMode="None" AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ResourceAssignmentID"
                                EditMode="InPlace" CommandItemDisplay="Top">
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
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridTemplateColumn UniqueName="ResourceName" HeaderText="ResourceName" SortExpression="ResourceName"
                                        DataField="ResourceName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ResourceName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ProjectName" HeaderText="ProjectName" SortExpression="ProjectName"
                                        DataField="ProjectName" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ProjectName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboProjectName" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboProjectName" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="RoleName" HeaderText="RoleName" SortExpression="RoleName"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("RoleName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboRole" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>

                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboRole" Columns="30" runat="Server" AppendDataBoundItems="true" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ConfirmText="Delete this Assignment ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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
                    <div id="divResourceInDepartment" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock1">

                            <script type="text/javascript">
                                function rgResourceInDepartmentshowFilterItem() {
                                    $find('<%=rgResourceInDepartment.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgResourceInDepartmenthideFilterItem() {
                                $find('<%=rgResourceInDepartment.ClientID %>').get_masterTableView().hideFilterItem();
                            }
                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio3" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgResourceInDepartmentshowFilterItem()" /><label for="Radio3">Yes</label>
                            <input id="Radio4" type="radio" runat="server" name="showHideGroup" onclick="rgResourceInDepartmenthideFilterItem()" /><label
                                for="Radio4">No</label>
                        </div>
                        <telerik:RadGrid ID="rgResourceInDepartment" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgResourceInDepartment_NeedDataSource"
                            OnDeleteCommand="rgResourceInDepartment_DeleteCommand" OnInsertCommand="rgResourceInDepartment_InsertCommand"
                            OnUpdateCommand="rgResourceInDepartment_UpdateCommand" EnableAJAX="True" OnItemCreated="rgResourceInDepartment_ItemCreated"
                            OnItemDataBound="rgResourceInDepartment_ItemDataBound" OnItemCommand="rgResourceInDepartment_ItemCommand"
                            AllowFilteringByColumn="true" PageSize="50">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting CellSelectionMode="None" AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="ResourceAssociationID"
                                EditMode="InPlace" CommandItemDisplay="Top" AllowFilteringByColumn="true">
                                <CommandItemSettings ExportToPdfText="Export to PDF" AddNewRecordText="Add new Record" ShowAddNewRecordButton="true"></CommandItemSettings>
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
                                    <telerik:GridTemplateColumn UniqueName="ResourceName" HeaderText="ResourceName" SortExpression="ResourceName"
                                        AllowFiltering="true" DataField="ResourceName" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("ResourceName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DepartmentName" HeaderText="DepartmentName"
                                        SortExpression="DepartmentName" AllowFiltering="true" DataField="DepartmentName"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("DepartmentName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboDepartmentName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboDepartmentName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--                                <telerik:GridButtonColumn ConfirmText="Delete this Association?" ConfirmDialogType="RadWindow"
                                    CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                    UniqueName="DeleteColumn" HeaderStyle-Width="50px">
                                    <ItemStyle HorizontalAlign="Center" CssClass="MyImageButton" />
                                </telerik:GridButtonColumn>--%>
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </div>
                    <div id="divCompResources" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock8">

                            <script type="text/javascript">
                                function rgCompResourcesshowFilterItem() {
                                    $find('<%=rgCompResources.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgCompResourceshideFilterItem() {
                                $find('<%=rgCompResources.ClientID %>').get_masterTableView().hideFilterItem();
                            }
                            </script>

                        </telerik:RadCodeBlock>
                        
                        <telerik:RadGrid ID="rgCompResources" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="True" Width="97%" OnNeedDataSource="rgCompResources_NeedDataSource"
                            OnDeleteCommand="rgCompResources_DeleteCommand" OnInsertCommand="rgCompResources_InsertCommand"
                            OnUpdateCommand="rgCompResources_UpdateCommand" EnableAJAX="True" OnItemCreated="rgCompResources_ItemCreated"
                            OnItemDataBound="rgCompResources_ItemDataBound" OnItemCommand="rgCompResources_ItemCommand"
                            AllowFilteringByColumn="true" PageSize="50">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting CellSelectionMode="None" AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" DataKeyNames="employeeID"
                                EditMode="InPlace" CommandItemDisplay="Top" AllowFilteringByColumn="true">
                                <CommandItemSettings ExportToPdfText="Export to PDF" AddNewRecordText="Add new Record" ShowAddNewRecordButton="true"></CommandItemSettings>
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
                                    <telerik:GridTemplateColumn HeaderText="Resource Name" SortExpression="Resource_name" DataField="Resource_name" ItemStyle-Width="250px"
                                    UniqueName="Resource_name" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                    <ItemTemplate>
                                        <%# Eval("Resource_name")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtResource_name" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtResource_name" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </EditItemTemplate>

                                    <ItemStyle Width="250px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Email" SortExpression="email" DataField="email" ItemStyle-Width="250px"
                                    UniqueName="email" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                    <ItemTemplate>
                                        <%# Eval("email")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtemail" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtemail" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </EditItemTemplate>

                                    <ItemStyle Width="250px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Designation" SortExpression="DESIGNATIONNAME" DataField="DESIGNATIONNAME" ItemStyle-Width="250px"
                                    UniqueName="DESIGNATIONNAME" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                    <ItemTemplate>
                                        <%# Eval("DESIGNATIONNAME")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtDESIGNATIONNAME" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDESIGNATIONNAME" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </EditItemTemplate>

                                    <ItemStyle Width="250px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                    <%--<telerik:GridTemplateColumn UniqueName="Supervisor_name" HeaderText="Supervisor" SortExpression="Supervisor_name"
                                        DataField="Supervisor_name" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("Supervisor_name")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comSupervisor_name" runat="server" AppendDataBoundItems="true" CheckBoxes="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvcomSupervisor_name" ControlToValidate="comSupervisor_name"
                                                ErrorMessage="Must Select One"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comSupervisor_name" runat="server" AppendDataBoundItems="true"></asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvcomSupervisor_name" ControlToValidate="comSupervisor_name"
                                                ErrorMessage="Must Select One"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                    
                                    <telerik:GridButtonColumn ConfirmText="Delete this Resource ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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
                    <div id="divCompProjects" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock9">

                            <script type="text/javascript">
                                function rgCompProjectsshowFilterItem() {
                                    $find('<%=rgCompProjects.ClientID %>').get_masterTableView().showFilterItem();
                            }
                                function rgCompProjectshideFilterItem() {
                                $find('<%=rgCompProjects.ClientID %>').get_masterTableView().hideFilterItem();
                                }
                            </script>

                        </telerik:RadCodeBlock>
                        
                        <telerik:RadGrid ID="rgCompProjects" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="True" Width="97%" OnNeedDataSource="rgCompProjects_NeedDataSource"
                            OnDeleteCommand="rgCompProjects_DeleteCommand" OnInsertCommand="rgCompProjects_InsertCommand"
                            OnUpdateCommand="rgCompProjects_UpdateCommand" EnableAJAX="True" OnItemCreated="rgCompProjects_ItemCreated"
                            OnItemDataBound="rgCompProjects_ItemDataBound" OnItemCommand="rgCompProjects_ItemCommand"
                            AllowFilteringByColumn="true" PageSize="50">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting CellSelectionMode="None" AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" DataKeyNames="PROJECT_ID"
                                EditMode="InPlace" CommandItemDisplay="Top" AllowFilteringByColumn="true">
                                <CommandItemSettings ExportToPdfText="Export to PDF" AddNewRecordText="Add new Record" ShowAddNewRecordButton="true"></CommandItemSettings>
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
                                    <%--<telerik:GridTemplateColumn HeaderText="PROJECT ID" SortExpression="PROJECT_ID" DataField="PROJECT_ID" ItemStyle-Width="250px"
                                    UniqueName="PROJECT_ID">
                                    <ItemTemplate>
                                        <%# Eval("PROJECT_ID")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtPROJECT_ID" name="txtPROJECT_ID" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtPROJECT_ID" name="editPROJECT_ID" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </EditItemTemplate>

                                    <ItemStyle Width="250px"></ItemStyle>
                                </telerik:GridTemplateColumn>--%>
                                    <telerik:GridTemplateColumn HeaderText="Project Name" SortExpression="Project_name" DataField="Project_name" ItemStyle-Width="250px"
                                    UniqueName="Project_name" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                    <ItemTemplate>
                                        <%# Eval("Project_name")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtProject_name" name="txtProject_name" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtProject_name" name="edittxtProject_name" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </EditItemTemplate>

                                    <ItemStyle Width="250px"></ItemStyle>
                                </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn ConfirmText="Delete this Project ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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
                    <div id="divUpComingProjects" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock5">

                            <script type="text/javascript">
                                function rgUpComingProjectsshowFilterItem() {
                                    $find('<%=rgUpComingProject.ClientID %>').get_masterTableView().showFilterItem();
                            }
                            function rgUpComingProjectshideFilterItem() {
                                $find('<%=rgUpComingProject.ClientID %>').get_masterTableView().hideFilterItem();
                            }
                            function Radio13_onclick() {

                            }

                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio11" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgUpComingProjectsshowFilterItem()" /><label for="Radio1">Yes</label>
                            <input id="Radio12" type="radio" runat="server" name="showHideGroup" onclick="rgUpComingProjectshideFilterItem()" /><label
                                for="Radio12">No</label>
                        </div>

                        <telerik:RadDatePicker ID="RadDatePicker3" Style="display: none;" MinDate="01/01/1900"
                            MaxDate="12/31/2100" runat="server">
                            <ClientEvents OnDateSelected="dateSelected" />
                        </telerik:RadDatePicker>

                        <telerik:RadGrid ID="rgUpComingProject" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgUpComingProject_NeedDataSource"
                            OnPreRender="rgUpComingProject_PreRender"
                            OnDeleteCommand="rgUpComingProject_DeleteCommand" OnInsertCommand="rgUpComingProject_InsertCommand"
                            OnUpdateCommand="rgUpComingProject_UpdateCommand" EnableAJAX="True" OnItemCreated="rgUpComingProject_ItemCreated"
                            OnItemDataBound="rgUpComingProject_ItemDataBound" OnItemCommand="rgUpComingProject_ItemCommand"
                            AllowFilteringByColumn="true" TabIndex="10" PageSize="50">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true">
                                <Selecting AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" AllowSubmitOnEnter="true" EnableKeyboardShortcuts="true" InitInsertKey="N" />
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
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridTemplateColumn UniqueName="Project" HeaderText="Project" SortExpression="Project_name"
                                        DataField="Project_name" AllowFiltering="true" CurrentFilterFunction="Contains"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("Project_name")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtProject" Columns="30" runat="Server">
                                            </asp:TextBox>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtProject" Columns="30" runat="Server">
                                            </asp:TextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="DesiredStart" HeaderText="Desired Start Date"
                                        SortExpression="DesiredStart" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("DesiredStart")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="DesiredStart" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput2" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvDesiredStart" ControlToValidate="DesiredStart"
                                                ErrorMessage="Enter a Desired Start Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="DesiredStart" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput3" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvDesiredStart" ControlToValidate="DesiredStart"
                                                ErrorMessage="Enter a Desired Start Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="PlannedStart" HeaderText="PlannedStart" SortExpression="PlannedStart"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("PlannedStart")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <telerik:RadDatePicker ID="PlannedStart" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput5" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvPlannedStart" ControlToValidate="PlannedStart"
                                                ErrorMessage="Enter a Planned Start Date!"></asp:RequiredFieldValidator>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadDatePicker ID="PlannedStart" runat="Server" AppendDataBoundItems="true">
                                                <DateInput ID="DateInput4" DisplayDateFormat="dd-MMM-yyyy" DateFormat="yyyy/MM/dd" LabelWidth="" Width="160px"
                                                    AutoPostBack="True" runat="server">
                                                </DateInput>
                                            </telerik:RadDatePicker>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvPlannedStart" ControlToValidate="PlannedStart"
                                                ErrorMessage="Enter a Planned Start Date!"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Resources" HeaderText="Resources" SortExpression="Resources"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("Resources")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtResource" runat="Server" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtResource" runat="Server" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="Comments" HeaderText="Comments" SortExpression="Comments"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("Comments")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtComment" runat="Server" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtComment" runat="Server" TextMode="MultiLine">
                                            </asp:TextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridButtonColumn ConfirmText="Are you sure you want to delete?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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

                    <!--Roles Start -->
                    <div id="divRoles" runat="server">
                        <telerik:RadCodeBlock runat="server" ID="radCodeBlock7">

                            <script type="text/javascript">
                                function rgRolesshowFilterItem() {
                                    $find('<%=rgRoles.ClientID %>').get_masterTableView().showFilterItem();
                                }
                                function rgRoleshideFilterItem() {
                                    $find('<%=rgRoles.ClientID %>').get_masterTableView().hideFilterItem();
                            }
                            </script>

                        </telerik:RadCodeBlock>
                        <div>
                            Show filtering item
                        <input id="Radio15" type="radio" runat="server" name="showHideGroup" checked="true"
                            onclick="rgRolesshowFilterItem()" /><label for="Radio1">Yes</label>
                            <input id="Radio16" type="radio" runat="server" name="showHideGroup" onclick="rgRoleshideFilterItem()" /><label
                                for="Radio2">No</label>
                        </div>
                        <telerik:RadGrid ID="rgRoles" runat="server" GridLines="None" AllowPaging="True"
                            AllowSorting="True" AutoGenerateColumns="False" Width="97%" OnNeedDataSource="rgRoles_NeedDataSource"
                            OnDeleteCommand="rgRoles_DeleteCommand" OnInsertCommand="rgRoles_InsertCommand"
                            OnUpdateCommand="rgRoles_UpdateCommand" EnableAJAX="True" OnItemCreated="rgDepartments_ItemCreated"
                            OnItemDataBound="rgRoles_ItemDataBound" OnItemCommand="rgRoles_ItemCommand"
                            AllowFilteringByColumn="True" PageSize="15">
                            <GroupingSettings CaseSensitive="false" />
                            <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                            <FilterMenu EnableImageSprites="False">
                            </FilterMenu>
                            <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true">
                                <Selecting CellSelectionMode="None" AllowRowSelect="true"></Selecting>
                                <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="False" DataKeyNames="DepartmentID" EditMode="InPlace"
                                CommandItemDisplay="Top" AllowFilteringByColumn="True">
                                <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                                <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                    <HeaderStyle Width="20px"></HeaderStyle>
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn"
                                        HeaderStyle-Width="50px">
                                    </telerik:GridEditCommandColumn>
                                    <telerik:GridTemplateColumn UniqueName="DepartmentID" HeaderText="DepartmentID" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("DepartmentID")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <%--
                                        <telerik:RadButton ID="btnCancelInsert" runat="server" Text="Cancel" CommandName="Cancel">
                                        </telerik:RadButton>
                                        <telerik:RadButton ID="btnInsert" runat="server" Text="Insert" CommandName="Insert">
                                        </telerik:RadButton>--%>
                                            <asp:TextBox ID="txtDepartmentID" Columns="30" runat="Server" Enabled="false"></asp:TextBox>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDepartmentID" Columns="30" runat="Server" Enabled="false"></asp:TextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="DepartmentName" HeaderText="DepartmentName"
                                        DataField="DepartmentName" SortExpression="DepartmentName" AllowFiltering="true"
                                        CurrentFilterFunction="Contains" ShowFilterIcon="false" AutoPostBackOnFilter="true">
                                        <ItemTemplate>
                                            <%# Eval("DepartmentName")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:TextBox ID="txtDepartmentName" Columns="30" runat="Server"></asp:TextBox>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtDepartmentName" Columns="30" runat="Server"></asp:TextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ReportsTo" HeaderText="ReportsTo" SortExpression="ReportsTo"
                                        AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("ReportsTo")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:DropDownList ID="comboReportsTo" Columns="30" runat="Server" AppendDataBoundItems="true">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="comboReportsTo" Columns="30" runat="Server" AppendDataBoundItems="true">
                                                <asp:ListItem></asp:ListItem>
                                            </asp:DropDownList>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Active" HeaderText="Status" SortExpression="Active" AllowFiltering="false">
                                        <ItemTemplate>
                                            <%# Eval("Active")%>
                                        </ItemTemplate>
                                        <InsertItemTemplate>
                                            <asp:CheckBox ID="chkActive" Columns="30" runat="server" Checked="true" AppendDataBoundItems="true"/>
                                        </InsertItemTemplate>
                                        <EditItemTemplate>
                                            <asp:CheckBox ID="chkActive" Columns="30" runat="server" Checked='<%# Eval("Active") %>' AppendDataBoundItems="true"/>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridButtonColumn ConfirmText="Delete this Department ?" ConfirmDialogType="RadWindow"
                                        CommandName="Delete" ConfirmTitle="Delete" ButtonType="ImageButton" Text="Delete"
                                        UniqueName="DeleteColumn" HeaderStyle-Width="50px">
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
                    <!--Roles End -->
                </div>
            </div>
            <!-- END: content -->
        </div>
        <!-- END: Content -->
        </div>
    <!-- END: Main Container -->
    </form>
</body>
</html>
