<%@ Page Title="Setup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Setup.aspx.cs" Inherits="TimeTracker.Setup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="panelSetupMain" runat="server" style="width:400px; display:none">
        <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalHeader">Select Option</asp:Panel>
        <asp:Panel ID="modalContent" runat="server" CssClass="modal" BackColor="#ffffff">
            <div style="margin-left:auto; margin-right:auto; display:table;">
                <ul>
                    <asp:Repeater ID="repeaterSetup" runat="server" Visible="true">
                        <ItemTemplate>
                            <li class="li">
                                <asp:LinkButton ID="lnkBtnSetup" runat="server" Text='<%#Eval("Description") %>' CommandArgument='<%#Eval("Filename")%>' OnCommand="lnkBtnSetup_Command">
                                </asp:LinkButton>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Button ID="btnHidden" runat="server" style="display:none" />
    <ajaxToolKit:ModalPopupExtender runat="server" ID="programmaticModalPopup"
                BehaviorID ="programmaticModalPopupBehavior"
                TargetControlID="btnHidden"
                PopupControlID="panelSetupMain"
                
                DropShadow="false"
                PopupDragHandleControlID="modalContent"
                RepositionMode="RepositionOnWindowResize" >    
            </ajaxToolKit:ModalPopupExtender> 
</asp:Content>
