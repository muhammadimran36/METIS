<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComboInGrid1.aspx.cs" Inherits="ComboInGrid1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .rcbHeader ul,
	.rcbFooter ul,
	.rcbItem ul, .rcbHovered ul, .rcbDisabled ul
	{
    		width: 100%;
    		display: inline-block;
    		margin: 0;
    		padding: 0;
    		list-style-type: none;
	}

	.col1, .col2
	{
    		float: left;
    		margin: 0;
    		padding: 0 5px 0 0;
    		line-height: 14px;
	}
	.col1
	{
    		width: 230px;
	}
	.col2
	{
    		width: 140px;
	}
    </style>
	<telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
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
		//Put your JavaScript code here.
    </script>
 
	   <telerik:RadAjaxManager runat="server" ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadGrid ID="RadGrid1" GridLines="None" AutoGenerateColumns="false" PageSize="10"
        AllowPaging="true" AllowSorting="true" runat="server" OnItemDataBound="OnItemDataBoundHandler"
        DataSourceID="ProductsDataSource" AllowAutomaticUpdates="true" AllowAutomaticInserts="True"
        ShowStatusBar="true">
        <MasterTableView ShowFooter="false" DataKeyNames="ProductID" EditMode="InPlace" CommandItemDisplay="TopAndBottom">
            <Columns>
                <telerik:GridBoundColumn DataField="ProductName" HeaderText="ProductName" HeaderStyle-Width="300px"
                    ItemStyle-Width="300px">
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn UniqueName="Supplier" HeaderText="Supplier" SortExpression="CompanyName"
                    ItemStyle-Width="400px">
                    <FooterTemplate>
                        Template footer</FooterTemplate>
                    <FooterStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "CompanyName")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadComboBox runat="server" ID="RadComboBox1" EnableLoadOnDemand="True" DataTextField="CompanyName"
                            OnItemsRequested="RadComboBox1_ItemsRequested" DataValueField="SupplierID" AutoPostBack="true"
                            HighlightTemplatedItems="true" Height="140px" Width="220px" DropDownWidth="420px"
                            OnSelectedIndexChanged="OnSelectedIndexChangedHandler">
                            <HeaderTemplate>
                                <ul>
                                    <li class="col1">Company</li>
                                    <li class="col2">ContactName</li>
                                </ul>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <ul>
                                    <li class="col1">
                                        <%# DataBinder.Eval(Container, "Text")%>
                                    </li>
                                    <li class="col2">
                                        <%# DataBinder.Eval(Container, "Attributes['ContactName']")%></li>
                                </ul>
                            </ItemTemplate>
                        </telerik:RadComboBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridTemplateColumn HeaderText="Category" ItemStyle-Width="240px">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "CategoryName")%>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <telerik:RadComboBox runat="server" ID="RadComboBox2" DataTextField="CategoryName"
                            DataValueField="CategoryID" DataSourceID="CategoriesDataSource" SelectedValue='<%#Bind("CategoryID") %>'>
                        </telerik:RadComboBox>
                    </EditItemTemplate>
                </telerik:GridTemplateColumn>
                <telerik:GridEditCommandColumn FooterText="EditCommand footer" UniqueName="EditCommandColumn"
                    HeaderText="Edit" HeaderStyle-Width="100px" UpdateText="Update">
                </telerik:GridEditCommandColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <asp:SqlDataSource ID="ProductsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:TelerikVSXConnectionString %>"
        SelectCommand="SELECT products.[ProductID], products.[ProductName], products.[SupplierID], products.[CategoryID],
                              suppliers.[CompanyName], suppliers.[ContactName], 
                              categories.[CategoryName]
                              FROM [Products] AS products
                              INNER JOIN Suppliers AS suppliers
                              ON products.SupplierID = suppliers.SupplierID
                              INNER JOIN Categories AS categories
                              ON products.CategoryID = categories.CategoryID" InsertCommand="INSERT INTO [Products] ([ProductName], [SupplierID], [CategoryID]) VALUES (@ProductName, @SupplierID, @CategoryID)"
        UpdateCommand="UPDATE [Products] SET  [ProductName] = @ProductName, [SupplierID] = @SupplierID, [CategoryID] = @CategoryID WHERE [ProductID] = @ProductID">
        <InsertParameters>
            <asp:Parameter Name="ProductName" Type="String" />
            <asp:SessionParameter SessionField="SupplierID" Name="SupplierID" Type="Int32" />
            <asp:Parameter Name="CategoryID" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="ProductID" Type="Int32" />
            <asp:Parameter Name="ProductName" Type="String" />
            <asp:SessionParameter SessionField="SupplierID" Name="SupplierID" Type="Int32" />
            <asp:Parameter Name="CategoryID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="CategoriesDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:TelerikVSXConnectionString %>"
        SelectCommand="SELECT [CategoryID], [CategoryName] FROM [Categories]"></asp:SqlDataSource>
    </form>
</body>
</html>
