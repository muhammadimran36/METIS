<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RFS.aspx.cs" Inherits="streebo.METIS.UI.RFS" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <link href="general.css" rel="stylesheet" type="text/css" />
    <link href="styles/style_new.css" rel="stylesheet" type="text/css" />
    <link href="styles/main.css" rel="stylesheet" type="text/css" />

    <title>RFS Summary</title>

    <style type="text/css">
        .color2 {
            background-image: none !important;
            background-color: Lightgray !important;
        }

        .ddlResource {
            align: left;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadStyleSheetManager ID="RadStyleSheetManager2" runat="server">
        </telerik:RadStyleSheetManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadInputManager1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="Label2"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadInputManager1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">


                var hasChanges, inputs, dropdowns, editedRow;
                function RowClick(sender, eventArgs) {
                    if (editedRow && hasChanges) {
                        hasChanges = false;
                        if (confirm("Update changes?")) {
                            $find("<%= RadGrid1.ClientID %>").get_masterTableView().updateItem(editedRow);
                        }
                    }
                }
                function RowDblClick(sender, eventArgs) {
                    editedRow = eventArgs.get_itemIndexHierarchical();

                    $find("<%= RadGrid1.ClientID %>").get_masterTableView().editItem(editedRow);
        }

        function GridCommand(sender, args) {
            if (args.get_commandName() != "Edit") {
                editedRow = null;
            }
        }
        function GridCreated(sender, eventArgs) {
            var gridElement = sender.get_element();
            var elementsToUse = [];
            inputs = gridElement.getElementsByTagName("input");
            for (var i = 0; i < inputs.length; i++) {
                var lowerType = inputs[i].type.toLowerCase();
                if (lowerType == "hidden" || lowerType == "button") {
                    continue;
                }
                if (inputs[i].id.indexOf("PageSizeComboBox") == -1 && inputs[i].type.toLowerCase() != "checkbox") {
                    Array.add(elementsToUse, inputs[i]);
                }
                inputs[i].onchange = TrackChanges;
            }
            dropdowns = gridElement.getElementsByTagName("select");
            for (var i = 0; i < dropdowns.length; i++) {
                dropdowns[i].onchange = TrackChanges;
            }
            setTimeout(function () { if (elementsToUse[0]) elementsToUse[0].focus(); }, 100);
        }
        function TrackChanges(e) {
            hasChanges = true;
        }
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (charCode != 46 && charCode > 31 && charCode != 43 && charCode != 45 && charCode != 42 && charCode != 47
                && charCode != 94 && charCode != 40 && charCode != 41 && charCode != 123 && charCode != 125 && charCode != 91
                && charCode != 93 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
            </script>
        </telerik:RadCodeBlock>

        <!-- START: Main Container -->
        <div class="mainContainer">

            <!-- START: Header -->
            <div class="headerPanel">

                <img class="metis" src="images/metis_logo.png" alt="METIS Logo">

                <div class="action">
                    <asp:PlaceHolder runat="server"><span class="title"><%= PrintUserName(Convert.ToString(Session["user"])) %>&nbsp;|&nbsp;</span> </asp:PlaceHolder>
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
                    <%--<li><a tabindex="3" href="UpComingProj.aspx">Upcoming Projects</a></li>--%>
                    <li class="selected"><a tabindex="4" href="RFS.aspx">RFS</a></li>
                </ul>
                <!-- END: Menu -->

                <!-- START: content -->
                <div id="MainProject" runat="server" class="contentPanel">

                    <!-- START: Filer Area -->
                    <div class="filter">
                        <table border="0" cellspacing="5" cellpadding="5" width="100%">
                            <tr>
                                <td width="150">&nbsp;</td>

                                <!-- Department -->
                                <td class="CMB">&nbsp;</td>

                                <td width="150">&nbsp;</td>

                                <td width="150">&nbsp;</td>

                                <td width="150">&nbsp;</td>

                                <td width="150">

                                    <asp:DropDownList ID="ddlYear" runat="server" AppendDataBoundItems="true" AutoPostBack="True" Width="175px" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                        <Items>
                                            <asp:ListItem Text="All" />
                                        </Items>
                                    </asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                                <%--<td align="right"><a href="#"><img src="./image/icon_referesh.png" width="25" height="21" border="0"></a></td>--%>
                            </tr>
                            <tr>
                                <td colspan="6"><%--<a href="#"><img src="/image/icon_expand.png" alt="Expand" width="23" height="19" border="0"></a>--%>
                                    <%--<a href="#"><img src="/image/icon_collapse.png" alt="Collapse" width="23" height="19" border="0"></a>--%>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- END: Filer Area -->
                    <telerik:RadGrid ID="RadGrid1" runat="server" CssClass="rfsChildGrid"
                        AllowPaging="True" AllowSorting="True" ShowFooter="True" EnableAJAX="True" TabIndex="9"
                        OnNeedDataSource="RadGrid1_NeedDataSource" PageSize="50" ShowStatusBar="True"
                        Skin="Metro" CellSpacing="0" GridLines="None" AutoGenerateColumns="False"
                        OnDeleteCommand="RadGrid1_DeleteCommand" OnInsertCommand="RadGrid1_InsertCommand"
                        OnItemCreated="RadGrid1_ItemCreated" OnItemDataBound="RadGrid1_ItemDataBound"
                        OnUpdateCommand="RadGrid1_UpdateCommand"
                        OnItemCommand="RadGrid1_ItemCommand" ForeColor="White">
                        <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="True" EnableDragToSelectRows="True"></Selecting>
                            <Selecting AllowRowSelect="true"></Selecting>
                            <ClientEvents OnGridCreated="GridCreated" OnRowDblClick="RowDblClick"
                                OnCommand="GridCommand"></ClientEvents>
                            <KeyboardNavigationSettings AllowActiveRowCycle="true" FocusKey="RightArrow" AllowSubmitOnEnter="true"
                                ExpandDetailTableKey="RightArrow" CollapseDetailTableKey="LeftArrow" EnableKeyboardShortcuts="true"
                                InitInsertKey="N" />
                        </ClientSettings>
                        <%--<ItemStyle CssClass="rfsGrid" />--%>
                        <MasterTableView AutoGenerateColumns="false" CommandItemDisplay="Top" DataKeyNames="Target_Actual_PK"
                            EditMode="InPlace" ShowGroupFooter="True">
                            <%--<CommandItemSettings  AddNewRecordText="Add new record" ShowAddNewRecordButton="true"></CommandItemSettings>--%>
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                            </ExpandCollapseColumn>
                            <CommandItemSettings ShowRefreshButton="false" />
                            <RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
                                <HeaderStyle Width="20px"></HeaderStyle>
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Customer Name" SortExpression="CustomerName" ItemStyle-Width="150px"
                                    FooterText="TOTAL" FooterStyle-Font-Bold="true" DataField="Customer Name" UniqueName="CustomerName">
                                    <ItemTemplate>
                                        <%# Eval("[Customer Name]")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <telerik:RadComboBox ID="dropCustomerName" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="dropCustomerName" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="150px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Project Name" SortExpression="ProjectName" ItemStyle-Width="275px"
                                    DataField="Project Name" UniqueName="ProjectName">
                                    <ItemTemplate>
                                        <%# Eval("[Project Name]")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="dropProjectName" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="dropProjectName" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </EditItemTemplate>

                                    <ItemStyle Width="275px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Owner" SortExpression="Consultant" DataField="Consultant" ItemStyle-Width="150px"
                                    UniqueName="Consultant">
                                    <ItemTemplate>
                                        <%# Eval("[Consultant]")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="dropConsultant" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="dropConsultant" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </EditItemTemplate>

                                    <ItemStyle Width="150px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Services" SortExpression="Services" DataField="Services" ItemStyle-Width="75px"
                                    UniqueName="Services">
                                    <ItemTemplate>
                                        <%# Eval("Services")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="dropService" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="dropService" Width="100%" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </EditItemTemplate>

                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Time Year" SortExpression="TimeYear" DataField="TimeYear"
                                    UniqueName="TimeYear">
                                    <ItemTemplate>
                                        <%# Eval("[TimeYear]")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="dropTimeYear" Width="60px" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="dropTimeYear" Width="60px" Columns="30" runat="Server" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q1 Target" SortExpression="Q1Target" DataField="Q1_Target" FooterStyle-Font-Bold="true"
                                    FooterAggregateFormatString=" {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" ItemStyle-Width="75px"
                                    UniqueName="Q1Target">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="TextBox1" runat="server" DbValue='<%# Eval("Q1_Target") %>'
                                            ReadOnly="true" Width="100%" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ1Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ1Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </EditItemTemplate>

                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q1 Actual" SortExpression="Q1ActualFormula" DataField="Q1_Actual_Formula" ItemStyle-Width="75px"
                                    FooterAggregateFormatString=" {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true"
                                    UniqueName="Q1ActualFormula">

                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtActualFormula" runat="server" DbValue='<%# Eval("Q1_Actual_Formula") %>'
                                            ReadOnly="true" Width="100%" Type="Currency" ToolTip='<%# Eval("Q1_Actual_Formula") %>'>
                                        </telerik:RadNumericTextBox>
                                        <asp:Label ID="lblJanF" runat="server" Text='<%# Eval("January_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblFebF" runat="server" Text='<%# Eval("February_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblMarF" runat="server" Text='<%# Eval("March_Formula") %>' Visible="false"></asp:Label>

                                    </ItemTemplate>

                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtJanuaryFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("January") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtFebruaryFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("February") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtMarchFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("March") %>'>
                                        </asp:TextBox>
                                    </InsertItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtJanuaryFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("January") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtFebruaryFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("February") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtMarchFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("March") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>



                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="75px"></ItemStyle>



                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q2 Target" SortExpression="Q2Target" DataField="Q2_Target" ItemStyle-Width="75px"
                                    FooterAggregateFormatString=" {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true"
                                    UniqueName="Q2Target">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="TextBox3" runat="server" DbValue='<%# Eval("Q2_Target") %>'
                                            ReadOnly="true" Width="100%" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ2Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ2Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </EditItemTemplate>

                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q2 Actual" SortExpression="Q2ActualFormula" DataField="Q2_Actual_Formula" ItemStyle-Width="75px"
                                    FooterAggregateFormatString=" {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true"
                                    UniqueName="Q2ActualFormula">

                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtActualFormula2" runat="server" DbValue='<%# Eval("Q2_Actual_Formula") %>'
                                            ReadOnly="true" Width="100%" Type="Currency" ToolTip='<%# Eval("Q2_Actual_Formula") %>'>
                                        </telerik:RadNumericTextBox>
                                        <asp:Label ID="lblAprF" runat="server" Text='<%# Eval("April_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblMayF" runat="server" Text='<%# Eval("May_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblJunF" runat="server" Text='<%# Eval("June_Formula") %>' Visible="false"></asp:Label>

                                    </ItemTemplate>

                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtAprilFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("April") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtMayFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("May") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtJuneFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("June") %>'>
                                        </asp:TextBox>
                                    </InsertItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtAprilFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("April") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtMayFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("May") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtJuneFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("June") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>



                                    <FooterStyle Font-Bold="True"></FooterStyle>



                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q3 Target" SortExpression="Q3Target" DataField="Q3_Target" ItemStyle-Width="75px"
                                    FooterAggregateFormatString="  {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true"
                                    UniqueName="Q3Target">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="TextBox5" runat="server" DbValue='<%# Eval("Q3_Target") %>'
                                            ReadOnly="true" Width="100%" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ3Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ3Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </EditItemTemplate>

                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q3 Actual" SortExpression="Q3ActualFormula" DataField="Q3_Actual_Formula" ItemStyle-Width="75px"
                                    FooterAggregateFormatString=" {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true"
                                    UniqueName="Q3ActualFormula">

                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtActualFormula3" runat="server" DbValue='<%# Eval("Q3_Actual_Formula") %>'
                                            ReadOnly="true" Width="100%" Type="Currency" ToolTip='<%# Eval("Q3_Actual_Formula") %>'>
                                        </telerik:RadNumericTextBox>
                                        <asp:Label ID="lblJulF" runat="server" Text='<%# Eval("July_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblAugF" runat="server" Text='<%# Eval("August_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblSepF" runat="server" Text='<%# Eval("September_Formula") %>' Visible="false"></asp:Label>

                                    </ItemTemplate>

                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtJulyFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("July") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtAugustFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("August") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtSeptemberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("September") %>'>
                                        </asp:TextBox>
                                    </InsertItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtJulyFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("July") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtAugustFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("August") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtSeptemberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("September") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>



                                    <FooterStyle Font-Bold="True"></FooterStyle>



                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q4 Target" SortExpression="Q4_Target" DataField="Q4_Target" ItemStyle-Width="75px"
                                    FooterAggregateFormatString=" {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true"
                                    UniqueName="Q4Target">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="TextBox7" runat="server" DbValue='<%# Eval("Q4_Target") %>'
                                            ReadOnly="true" Width="100%" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ4Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtQ4Target" Width="100%" runat="Server" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </EditItemTemplate>

                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Q4 Actual" SortExpression="Q4ActualFormula" DataField="Q4_Actual_Formula" ItemStyle-Width="75px"
                                    FooterAggregateFormatString=" {0:C2}" GroupByExpression="{0:C2}" Aggregate="Sum" FooterStyle-Font-Bold="true"
                                    UniqueName="Q4ActualFormula">

                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtActualFormula4" runat="server" DbValue='<%# Eval("Q4_Actual_Formula") %>'
                                            ReadOnly="true" Width="100%" Type="Currency" ToolTip='<%# Eval("Q4_Actual_Formula") %>'>
                                        </telerik:RadNumericTextBox>
                                        <asp:Label ID="lblOctF" runat="server" Text='<%# Eval("October_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblNovF" runat="server" Text='<%# Eval("November_Formula") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblDecF" runat="server" Text='<%# Eval("December_Formula") %>' Visible="false"></asp:Label>

                                    </ItemTemplate>

                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtOctoberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("October") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtNovemberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("November") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtDecemberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("December") %>'>
                                        </asp:TextBox>
                                    </InsertItemTemplate>

                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtOctoberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("October") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtNovemberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("November") %>'>
                                        </asp:TextBox>
                                        <asp:TextBox ID="txtDecemberFormula" Width="100%" runat="Server" onkeypress="return isNumberKey(event)" ToolTip='<%# Eval("December") %>'>
                                        </asp:TextBox>
                                    </EditItemTemplate>



                                    <FooterStyle Font-Bold="True"></FooterStyle>



                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Total Target" SortExpression="Total_Target" ItemStyle-Width="75px"
                                    DataField="Total_Target" FooterAggregateFormatString="  {0:C2}" GroupByExpression="{0:C2}" FooterStyle-Font-Bold="true"
                                    Aggregate="Sum" UniqueName="TotalTarget">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="TextBox9" runat="server" DbValue='<%# Eval("Total_Target") %>'
                                            ReadOnly="true" Width="100%" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtTotalTarget" Width="100%" runat="Server" ReadOnly="true"
                                            Visible="false">
                                        </telerik:RadNumericTextBox>
                                    </EditItemTemplate>

                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Total Actual" SortExpression="Total_Actual" ItemStyle-Width="75px"
                                    DataField="Total_Actual" FooterAggregateFormatString="  {0:C2}" GroupByExpression="{0:C2}" FooterStyle-Font-Bold="true"
                                    Aggregate="Sum" UniqueName="Total_Actual">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="TextBox10" runat="server" DbValue='<%# Eval("Total_Actual") %>'
                                            ReadOnly="true" Width="100%" Type="Currency">
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtTotal_Actual" Width="100%" runat="Server" ReadOnly="true"
                                            Visible="false">
                                        </telerik:RadNumericTextBox>
                                    </EditItemTemplate>

                                    <FooterStyle Font-Bold="True"></FooterStyle>

                                    <ItemStyle Width="75px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Comments" SortExpression="Comments" DataField="Comments" ItemStyle-Width="250px"
                                    UniqueName="Comments">
                                    <ItemTemplate>
                                        <%# Eval("Comments")%>
                                    </ItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtComments" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </InsertItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtComments" Width="100%" runat="Server" Visible="true"></asp:TextBox>
                                    </EditItemTemplate>

                                    <ItemStyle Width="250px"></ItemStyle>
                                </telerik:GridTemplateColumn>
                                <telerik:GridButtonColumn CommandName="Delete" ButtonType="ImageButton" ConfirmText="Delete this product?"
                                    ConfirmDialogType="RadWindow" ConfirmTitle="Delete">
                                </telerik:GridButtonColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                </EditColumn>
                            </EditFormSettings>
                            <PagerStyle AlwaysVisible="True" CssClass="rfsGrid" Font-Bold="True"
                                PageButtonCount="25" />
                            <CommandItemTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" Font-Bold="true" ForeColor="Black" CommandName="InitInsert" Visible='<%# !RadGrid1.MasterTableView.IsItemInserted %>'>
                     <img style="border:0px;vertical-align:middle;" alt="" src="Images/addnew.png"/>Add new</asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="btnCancel" Font-Bold="true" runat="server" CommandName="CancelAll" Visible='<%# RadGrid1.EditIndexes.Count > 0 || RadGrid1.MasterTableView.IsItemInserted %>'>
                     <img style="border:0px;vertical-align:middle;" alt="" src="Images/16_close.png"/>Cancel</asp:LinkButton>&nbsp;&nbsp;
                            </CommandItemTemplate>
                        </MasterTableView>

                        <HeaderStyle BackColor="#0099CC" ForeColor="White" />

                        <FilterMenu EnableImageSprites="False"></FilterMenu>
                    </telerik:RadGrid>
                    <%--Other Grids--%>
                </div>
            </div>
        </div>
        <!-- END: content -->
        <br />
        <br />
        <asp:Label ID="lblErr" runat="server" Font-Bold="true" Font-Size="14px" Style="margin: 1em;"></asp:Label>
    </form>
</body>
</html>
