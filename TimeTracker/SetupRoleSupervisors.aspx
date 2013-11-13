<%@ Page Title="Role Supervisors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupRoleSupervisors.aspx.cs" Inherits="TimeTracker.SetupRoleSupervisors" %>
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
           <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Role</asp:Panel>
            <asp:Panel ID="panelContent" runat="server" CssClass="modal">
                <asp:Label ID="labelAccessDenied" runat="server" Text="You do not have access rights to this page. Please contact your administrator if you need access. Thank you." Visible="false" CssClass="validation"></asp:Label>
                <asp:Panel ID="panelAccessOK" runat="server">
                    <asp:LinkButton ID="linkBtnAdd" runat="server" CausesValidation="false" Text="Add" CssClass="linkButton" Font-Bold="true" OnCommand="linkBtnAdd_Command"></asp:LinkButton>
                    <div style="margin:10px 10px 10px 10px">
                    <asp:GridView ID="gridViewRoles" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true" OnRowCommand="gridViewRoles_Command">
                        <EmptyDataRowStyle/>
                        <EmptyDataTemplate>
                            No Data Found.
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="labelRoleId" runat="server" Text='<%#Eval("Id") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Role" DataField="Description" ReadOnly="true"></asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>

    <%--Modal--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="modalBtnSubmit" />
        </Triggers>
        <ContentTemplate>
            <asp:UpdateProgress ID="modalUpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="modalImageLoading" runat="server" ImageUrl="Images/loading.gif" AlternateText="Loading..." ToolTip="Loading..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Panel Id="panelModal" runat="server" style="display:none">
                <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalHeader">Setup Supervisor</asp:Panel>
                <asp:Panel ID="panelModalContent" runat="server" CssClass="modal">
                <table style="padding:10px; border-collapse:separate;border-spacing:10px; width:100%">
                    <tr><td>Role</td><td>:</td><td><asp:DropDownList ID="modalDropDownRoles" runat="server"></asp:DropDownList></td></tr>
                    <tr><td colspan="3"><asp:CheckBox ID="modalChkboxAll" runat="server" Text=" Select All" AutoPostBack="true" OnCheckedChanged="modalChkboxAll_Changed" /></td></tr>
                    <tr>
                        <td colspan="3"><asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" style="max-height:350px">
                            <asp:GridView ID="gridViewModal" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:BoundField HeaderText="Role" DataField="Description" />
                                    <asp:TemplateField HeaderText="Supervisor">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkboxSupervisor" runat="server" />
                                            <asp:Label ID="modalLabelRoleId" runat="server" Visible="false" Text='<%#Eval("Id")%>' ToolTip="ToolTip"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align:center">
                            <asp:Button ID="modalBtnSubmit" runat="server" Text="Submit" CssClass="button" OnCommand="modalBtnSubmit_Command"/>
                            <asp:Button ID="modalBtnCancel" runat="server" Text="Cancel" CssClass="button" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            </asp:Panel>

            <asp:Button ID="btnHidden" runat="server" style="display:none"/>
            
            <ajaxToolKit:ModalPopupExtender runat="server" ID="programmaticModalPopup"
                BehaviorID ="programmaticModalPopupBehavior"
                TargetControlID="btnHidden"
                PopupControlID="panelModal"
                BackgroundCssClass="modalBackground"
                CancelControlID="modalBtnCancel"
                DropShadow="false"
                PopupDragHandleControlID="panelModalHeader"
                RepositionMode="RepositionOnWindowResize" >    
            </ajaxToolKit:ModalPopupExtender> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
