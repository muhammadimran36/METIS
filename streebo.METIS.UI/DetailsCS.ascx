<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetailsCS.ascx.cs" Inherits="streebo.METIS.UI.DetailsCS" %>
<link href="General.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .style1 {
        width: 127px;
    }

    .style2 {
        width: 220px;
    }
</style>
<table runat="server" style="width: 100%; height: 175px;" id="ResourceWrapper"
    border="0" cellpadding="2"
    cellspacing="0">
    <tr>
        <td style="text-align: center;" valign="middle" class="style6">
            <asp:FormView ID="ResourceView"
                runat="server" OnDataBound="ResourceView_DataBound"
                OnPageIndexChanging="ResourceView_PageIndexChanging" Height="48px"
                Width="358px" Style="height: 175px">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td valign="middle" class="style1">
                                <asp:Image ID="imgGenderIcon" runat="server" Height="139px" Width="125px" />
                            </td>
                            <td valign="top" class="style2">
                                <br />
                                <asp:Label ID="ResourceName" runat="server"
                                    Style="font-weight: 700; color: #666666; font-size: medium;"><%# Eval("Resource_name")%></asp:Label>
                                <br />
                                <asp:Label ID="Label3" runat="server"
                                    Style="color: #666666; font-size: small;">Designation: </asp:Label>
                                &nbsp;<asp:Label ID="Designation" runat="server" Font-Size="Small"
                                    Style="color: #666666; text-decoration: underline;"><%# Eval("DESIGNATIONNAME")%></asp:Label>
                                <br />
                                <asp:Label ID="Label1" runat="server" Font-Size="Small" Style="color: #666666">Streebo exp: </asp:Label>
                                <asp:Label ID="TotExpStreebo" runat="server" Font-Size="Small"
                                    Style="color: #666666; text-decoration: underline;"><%# Eval("TotalStreeboExp")%></asp:Label>
                                <br />
                                <asp:Label ID="Label2" runat="server" Font-Size="Small" Style="color: #666666">Total exp: </asp:Label>
                                &nbsp;&nbsp;&nbsp;
                                <asp:Label ID="TotExpIT" runat="server" Font-Size="Small"
                                    Style="color: #666666; text-decoration: underline;"><%# Eval("TotalITExp")%></asp:Label>
                                <br />
                                <asp:Label ID="Label4" runat="server"
                                    Style="color: #666666; font-size: small;">Resource Manager: </asp:Label>
                                &nbsp;<asp:Label ID="Label5" runat="server" Font-Size="Small"
                                    Style="color: #666666; text-decoration: underline;"><%# Eval("Supervisor_name")%></asp:Label>
                                <br />

                                <asp:Label ID="lblgender" runat="server" Text='<%# Eval("gender")%>' Font-Size="Small"
                                    Style="display: none; text-decoration: underline;"></asp:Label>

                                <asp:Label ID="lblCV_link" runat="server" Style="color: #666666; font-size: small;">CV: </asp:Label>
                                <asp:HyperLink ID="HL_Cv_link" runat="server" NavigateUrl='<%# Eval("Cv_link")%>' Target="_blank" Text='Click here' ></asp:HyperLink>

                                <br />
                                <asp:Label ID="lblProfile" runat="server" Style="color: #666666; font-size: small;">Profile: </asp:Label>
                                <asp:HyperLink ID="HL_Profile" runat="server" NavigateUrl='<%# Eval("Profile")%>' Target="_blank" Text='Click here' ></asp:HyperLink>

                            </td>
                        </tr>
                    </table>

                </ItemTemplate>
            </asp:FormView>
        </td>
    </tr>
</table>