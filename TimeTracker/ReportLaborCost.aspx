<%@ Page Title="Labor Cost Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportLaborCost.aspx.cs" Inherits="TimeTracker.ReportLaborCost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelMain" UpdateMode="Conditional" runat="server"> 
        <Triggers><asp:PostBackTrigger ControlID="btnGenerate" /></Triggers> 
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelMain" DynamicLayout="true">
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="mainImageLoading" runat="server" ImageUrl="Images/loading.gif" AlternateText="Loading..." ToolTip="Loading..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Panel ID="panelSetupMain" runat="server" style="width:430px;margin-left:auto;margin-right:auto">
                <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalHeader">Labor Cost Report</asp:Panel>
                <asp:Panel ID="modalContent" runat="server" CssClass="modal" BackColor="#ffffff">
                    <table>
                        <tr><td style="text-align:left">From</td><td style="text-align:left">:</td><td style="text-align:left"><asp:TextBox ID="txtBoxFrom" runat="server" AutoPostBack="true" OnTextChanged="txtBoxFrom_Changed"></asp:TextBox></td>
                            <td style="text-align:right">To</td><td style="text-align:right">:</td><td style="text-align:right"><asp:TextBox ID="txtBoxTo" runat="server" AutoPostBack="true" OnTextChanged="txtBoxTo_Changed"></asp:TextBox></td>
                        </tr>
                        <tr><td colspan="6"><br /></td></tr>
                        <tr><td colspan="6" style="text-align:center"><asp:Button ID="btnGenerate" runat="server" CssClass="button" Text="Generate Report" OnClick="btnGenerate_Click"/></td></tr>
                        <tr><td><br /></td></tr>
                        <tr><td colspan="6" style="text-align:left"><asp:Label ID="lblReadyForDownload" Visible="false" runat="server" ></asp:Label></td></tr>
                        <tr><td colspan="6" style="text-align:center"><asp:Button ID="btnDownload" runat="server" Visible="false" Text="Download Report" CssClass="buttongreen" OnClick="btnDownload_Click" /></td></tr>
                    </table>
                    
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderFrom" runat="server" TargetControlID="txtBoxFrom" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderTo" runat="server" TargetControlID="txtBoxTo" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                </asp:Panel>
            </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
