<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PortalPractise.aspx.cs" Inherits="streebo.METIS.UI.PortalPractise" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
    
    <telerik:RadGrid ID="rgResourceOnProjects" runat="server" GridLines="None" 
                        
                        
                        AllowFilteringByColumn="true" PageSize="50" 
            >
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
                                        <asp:DropDownList ID="comboResourceName" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="comboResourceName2" Columns="30" runat="Server" AppendDataBoundItems="true">
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
                                        <asp:DropDownList ID="comboProjectName2" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="RoleName" HeaderText="RoleName" SortExpression="RoleName"
                                    AllowFiltering="false">
                                    <ItemTemplate>
                                        <%# Eval("RoleName")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="comboRole" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                        
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="comboRole2" Columns="30" runat="Server" AppendDataBoundItems="true">
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
    
        <br />
    
    </div>
    </form>
</body>
</html>
