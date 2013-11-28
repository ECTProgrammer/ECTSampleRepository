<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DatabaseSetting.aspx.cs" Inherits="TimeTracker.DatabaseSetting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanelMain" runat="server">   
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelMain" DynamicLayout="true">
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="mainImageLoading" runat="server" ImageUrl="Images/loading.gif" AlternateText="Loading..." ToolTip="Loading..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
            <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Database Setting</asp:Panel>
            <asp:Panel ID="panelContent" runat="server" CssClass="modal">
                <table style="padding:10px; border-collapse:separate;border-spacing:10px; width:100%;text-align:center" >
                    <tr><td><asp:Panel runat="server" BorderStyle="Solid" BorderColor="#c8ccd1" Width="100%">
                            <table style="width:98%"><tr><td colspan="3" style="text-align:left">Time Tracker</td></tr>
                                <tr><td colspan="3"></td></tr>
                                <tr><td  style="text-align:right">Server</td><td  style="text-align:left">:</td><td  style="text-align:left"><asp:TextBox ID="ttTxtBoxServer" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorTTServer" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="ttTxtBoxServer" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Database</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="ttTxtBoxDatabase" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorTTDatabase" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="ttTxtBoxDatabase" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Username</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="ttTxtBoxUsername" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorTTUsername" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="ttTxtBoxUsername" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Password</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="ttTxtBoxPassword" runat="server" CssClass="textBox1" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorTTPassword" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="ttTxtBoxPassword" Text="*"></asp:RequiredFieldValidator></td></tr>
                            </table>
                            </asp:Panel></td>
                    <td><asp:Panel ID="Panel1" runat="server" BorderStyle="Solid" BorderColor="#c8ccd1" Width="100%">
                            <table style="width:98%"><tr><td colspan="3"  style="text-align:left">Hardware</td></tr>
                                <tr><td colspan="3"></td></tr>
                                <tr><td style="text-align:right">Server</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="hwTxtBoxServer" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorHWServer" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="hwTxtBoxServer" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Database</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="hwTxtBoxDatabase" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorHWDatabase" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="hwTxtBoxDatabase" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Username</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="hwTxtBoxUsername" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorHWUsername" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="hwTxtBoxUsername" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Password</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="hwTxtBoxPassword" runat="server" CssClass="textBox1" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorHWPassword" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="hwTxtBoxPassword" Text="*"></asp:RequiredFieldValidator></td></tr>
                            </table>
                            </asp:Panel></td>
                    <td><asp:Panel ID="Panel2" runat="server" BorderStyle="Solid" BorderColor="#c8ccd1" Width="100%">
                            <table style="width:98%"><tr><td colspan="3"  style="text-align:left">Software</td></tr>
                                <tr><td colspan="3"></td></tr>
                                <tr><td style="text-align:right">Server</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="swTxtBoxServer" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorSWServer" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="swTxtBoxServer" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Database</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="swTxtBoxDatabase" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorSWDatabase" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="swTxtBoxDatabase" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Username</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="swTxtBoxUsername" runat="server" CssClass="textBox1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorSWUsername" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="swTxtBoxUsername" Text="*"></asp:RequiredFieldValidator></td></tr>
                                <tr><td style="text-align:right">Password</td><td style="text-align:right">:</td><td style="text-align:left"><asp:TextBox ID="swTxtBoxPassword" runat="server" CssClass="textBox1" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="validatorSWPassword" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="swTxtBoxPassword" Text="*"></asp:RequiredFieldValidator></td></tr>
                            </table>
                            </asp:Panel></td></tr>
                    <tr><td><br /></td></tr>
                    <tr><td colspan="3" style="text-align:center">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" CausesValidation="true" OnClientClick="if(!confirm('Are you sure you want to change the database settings?')) return false;" OnClick="btnSubmit_Click"/>
                            </td></tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
