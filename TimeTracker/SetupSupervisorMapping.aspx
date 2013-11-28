<%@ Page Title="Setup Supervisor Mapping" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupSupervisorMapping.aspx.cs" Inherits="TimeTracker.SetupSupervisorMapping" %>
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
            <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Supervisor Mapping</asp:Panel>
	        <asp:Panel ID="panelContent" runat="server" CssClass="modal">
                <asp:Label ID="labelAccessDenied" runat="server" Text="You do not have access rights to this page. Please contact your administrator if you need access. Thank you." Visible="false" CssClass="validation"></asp:Label>
		        <asp:Panel ID="panelAccessOK" runat="server">
                    <table style="width:100%;">
                        <tr><td>Department : <asp:DropDownList ID="dropDownListDepartment" runat="server" CssClass="dropDownList1" AutoPostBack="true" OnSelectedIndexChanged="dropDownDepartment_Changed"></asp:DropDownList></td></tr>
                        <tr><td style="text-align:left"><asp:LinkButton ID="linkBtnAdd" runat="server" CausesValidation="false" Text="Add" CssClass="linkButton" Font-Bold="true" OnClick="linkBtnAdd_Click"></asp:LinkButton></td></tr>
                    </table>
                    <div style="margin:10px 10px 10px 10px">
                        <asp:GridView ID="gridViewUser" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" OnRowCommand="gridViewUser_Command" ShowHeaderWhenEmpty="true">
				            <EmptyDataRowStyle/>
					        <EmptyDataTemplate>
					            No Data Found.
					        </EmptyDataTemplate>
				            <Columns>
					            <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="labelUserId" runat="server" Text='<%#Eval("Id")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
					            </asp:TemplateField>
					            <asp:BoundField HeaderText="Employee No" DataField="EmployeeNumber" ReadOnly="true"></asp:BoundField>
					            <asp:BoundField HeaderText="First Name" DataField="Firstname" ReadOnly="true"></asp:BoundField>
					            <asp:BoundField HeaderText="Last Name" DataField="Lastname" ReadOnly="true"></asp:BoundField>
					            <asp:BoundField HeaderText="Department" DataField="department" ReadOnly="true"></asp:BoundField>
					            <asp:BoundField HeaderText="Role" DataField="role" ReadOnly="true"></asp:BoundField>
					            <asp:BoundField HeaderText="Email" DataField="Email" ReadOnly="false"></asp:BoundField>
					            <asp:BoundField HeaderText="Phone" DataField="Phone" ReadOnly="false"></asp:BoundField>
					            <asp:BoundField HeaderText="Mobile" DataField="Mobile"></asp:BoundField>
					            <asp:BoundField HeaderText="Fax" DataField="Fax"></asp:BoundField>
				            </Columns>
			            </asp:GridView>
                    </div>
		        </asp:Panel>
	        </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="modalBtnSubmit" />
        </Triggers>
        <ContentTemplate>
            <asp:UpdateProgress ID="modalUpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelModal" DynamicLayout="true">
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="modalImageLoading" runat="server" ImageUrl="Images/loading.gif" AlternateText="Loading..." ToolTip="Loading..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Panel Id="panelModal" runat="server" style="display:none">
                <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalHeader">Setup User</asp:Panel>
                <asp:Panel ID="panelModalContent" runat="server" CssClass="modal">
                    <asp:Label ID="modalLabelUserId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="modalLabelError" runat="server" CssClass="validation" Visible="false"></asp:Label>
                    <table style="padding:10px; border-collapse:separate;border-spacing:10px; width:100%">
                        <tr><td>User</td><td>:</td><td><asp:DropDownList ID="modalDropDownUsers" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                        <tr><td>Department</td><td>:</td><td><asp:DropDownList ID="modalDropDownDepartment" runat="server" CssClass="dropDownList1" AutoPostBack="true" OnSelectedIndexChanged="modalDropDownDepartment_Changed"></asp:DropDownList></td></tr>
                        <tr><td>Supervisor</td><td>:</td><td><asp:DropDownList ID="modalDropDownSupervisor" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                        <tr><td colspan="3" style="text-align:left"><asp:Button ID="modalBtnAdd" runat="server" CssClass="buttongreen" Text="Add" OnClick="modalBtnAdd_Click"/></td></tr>
                        <tr><td colspan="3">
                                <asp:Panel ID="modalInnerPanel" runat="server" ScrollBars="Auto" style="max-height:180px">
                                    <asp:GridView ID="gridViewModal" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                       <EmptyDataRowStyle />
                                        <EmptyDataTemplate>
                                            No Data Found.
                                        </EmptyDataTemplate>
                                       <Columns>
                                           <asp:BoundField HeaderText="Name" DataField="supervisorname"/>
                                           <asp:BoundField HeaderText="Department" DataField="supdepartment" />
                                           <asp:TemplateField>
                                               <ItemTemplate>
                                                    <asp:CheckBox ID="modalChkSupervisor" runat="server" Checked="true" />
                                                    <asp:Label ID="modalLabelSupId" runat="server" Visible="false" Text='<%#Eval("SupervisorId")%>' ToolTip="ToolTip"></asp:Label>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                       </Columns>
                                   </asp:GridView>
                                </asp:Panel>
                            </td></tr>
                        <tr>
                            <td colspan="3" style="text-align:center">
                                <asp:Button ID="modalBtnSubmit" runat="server" Text="Submit" ValidationGroup="modal" CausesValidation="true" CssClass="button" OnCommand="modalBtnSubmit_Command"/>
                                <asp:Button ID="modalBtnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="button" OnClick="modalBtnCancel_Click" />
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
