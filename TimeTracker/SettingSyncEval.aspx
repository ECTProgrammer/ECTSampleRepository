<%@ Page Title="Eval Synchronization" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SettingSyncEval.aspx.cs" Inherits="TimeTracker.SettingSyncEval" %>
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

           <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Eval Synchronization</asp:Panel>
	       <asp:Panel ID="panelContent" runat="server" CssClass="modal">
               <table style="padding:3px;border-spacing:10px; width:100%" border="0">
                   <tr><td><asp:TextBox ID="modalTxtBoxMessage" runat="server" TextMode="MultiLine" Enabled="false" ValidationGroup="modal" Rows="15" Width="100%"></asp:TextBox></td></tr>
                   <tr><td></td></tr>
                   <tr><td style="text-align:center"><asp:Button ID="modalBtnSubmit" runat="server" CssClass="button" ValidationGroup="modal" CausesValidation="true" Text="Sync" OnCommand="modalBtnSync_Command" /></td></tr>
               </table>
           </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
