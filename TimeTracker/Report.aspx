<%@ Page Title="Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="TimeTracker.Report" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panelReportMain" runat="server" style="width:400px;margin-left:auto;margin-right:auto">
        <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalHeader">Select Option</asp:Panel>
        <asp:Panel ID="modalContent" runat="server" CssClass="modal" BackColor="#ffffff">
            <div style="margin-left:auto; margin-right:auto; display:table;">
                <ul>
                    <asp:Repeater ID="repeaterReport" runat="server" Visible="true">
                        <ItemTemplate>
                            <li class="li">
                                <asp:LinkButton ID="lnkBtnReport" runat="server" Text='<%#Eval("Description") %>' CommandArgument='<%#Eval("Filename")%>' OnCommand="lnkBtnReport_Command">
                                </asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
