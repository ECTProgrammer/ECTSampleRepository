<%@ Page Title="Setup Job Flow" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupJobFlow.aspx.cs" Inherits="TimeTracker.SetupJobFlow" %>
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
                        <tr><td style="text-align:left"><asp:LinkButton ID="linkBtnAdd" runat="server" CausesValidation="false" Text="Add" CssClass="linkButton" Font-Bold="true" OnClick="linkBtnAdd_Click"></asp:LinkButton></td></tr>
                    </table>
                    <div style="margin:10px 10px 10px 10px">
                        <asp:GridView ID="gridViewJobFlow" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" OnRowCommand="gridViewJobFlow_Command" ShowHeaderWhenEmpty="true">
				            <EmptyDataRowStyle/>
					        <EmptyDataTemplate>
					            No Data Found.
					        </EmptyDataTemplate>
				            <Columns>
					            <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="labelJobFlowId" runat="server" Text='<%#Eval("Id")%>' Visible="false"></asp:Label>
                                    </ItemTemplate>
					            </asp:TemplateField>
					            <asp:BoundField HeaderText="Description" DataField="Description" ReadOnly="true"></asp:BoundField>
					            <asp:BoundField HeaderText="Acronym" DataField="Acronym" ReadOnly="true"></asp:BoundField>
					            <asp:BoundField HeaderText="Position" DataField="Position" ReadOnly="true"></asp:BoundField>
				            </Columns>
			            </asp:GridView>
                    </div>
		        </asp:Panel>
	        </asp:Panel> 
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--Modal--%>
    <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="modalUpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanelModal" DynamicLayout="true">
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="modalImageLoading" runat="server" ImageUrl="Images/loading.gif" AlternateText="Loading..." ToolTip="Loading..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <asp:Panel Id="panelModal" runat="server" style="display:none">
                <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalHeader">Setup Job Flow</asp:Panel>
                <asp:Panel ID="panelModalContent" runat="server" CssClass="modal">
                    <asp:Label ID="modalLabelJobFlowId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="modalLabelError" runat="server" CssClass="validation" Visible="false"></asp:Label>
                    <table style="padding:10px; border-collapse:separate;border-spacing:10px; width:100%">
                        <tr><td>Description</td><td>:</td><td><asp:TextBox ID="modalTxtBoxDescription" runat="server" AutoPostBack="true" ValidationGroup="modal" CssClass="textBox1" OnTextChanged="modalDescription_Changed"/>
                            <asp:RequiredFieldValidator ID="modalReqDescription" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxDescription" Text="*"/></td></tr>
                        <tr><td>Acronym</td><td>:</td><td><asp:TextBox ID="modalTxtBoxAcronym" runat="server" CssClass="textBox1" AutoPostBack="true" ValidationGroup="modal" OnTextChanged="modalAcronym_Changed" MaxLength="15"/>
                            <asp:RequiredFieldValidator ID="modalReqAcronym" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxAcronym" Text="*"/></td></tr>
                        <tr><td>Position</td><td>:</td><td><asp:TextBox ID="modalTxtBoxPosition" runat="server" ValidationGroup="modal" CssClass="textBox1"/>
                            <asp:RequiredFieldValidator ID="modalReqPosition" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxPosition" Text="*"/></td></tr>
                        <tr><td>Job Type</td><td>:</td><td><asp:DropDownList ID="modalDropDownJobType" runat="server" CssClass="dropDownList1" AutoPostBack="true" OnSelectedIndexChanged="modalDropDownJobType_Changed"></asp:DropDownList></td></tr>
                        <tr><td>Department</td><td>:</td><td><asp:DropDownList ID="modalDropDownDepartment" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                        <tr><td colspan="3" style="text-align:left"><asp:Button ID="modalBtnAdd" runat="server" CssClass="buttongreen" Text="Add" OnClick="modalBtnAdd_Click"/></td></tr>
                        <tr><td colspan="3">
                                <asp:Panel ID="modalInnerPanel" runat="server" ScrollBars="Auto" style="max-height:180px">
                                    <asp:GridView ID="gridViewModal" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                       <EmptyDataRowStyle />
                                        <EmptyDataTemplate>
                                            No Data Found.
                                        </EmptyDataTemplate>
                                       <Columns>
                                           <asp:BoundField HeaderText="Job Type" DataField="jobtype"/>
                                           <asp:TemplateField HeaderText="Department">
                                               <ItemTemplate>
                                                   <asp:Label ID="modalLabelDepartment" runat="server"  Text='<%#Eval("department")%>' />
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField HeaderText="Position">
                                               <ItemTemplate>
                                                   <asp:TextBox ID="modalInnerTxtBoxPostion" runat="server" CssClass="textBox1" Text='<%#Eval("Position")%>' />
                                                   <ajaxToolkit:FilteredTextBoxExtender ID="filterInnerPosition" runat="server" TargetControlID="modalInnerTxtBoxPostion" FilterType="Numbers"/>
                                               </ItemTemplate>
                                           </asp:TemplateField>
                                           <asp:TemplateField>
                                               <ItemTemplate>
                                                    <asp:CheckBox ID="modalChkJobType" runat="server" Checked="true" />
                                                    <asp:Label ID="modalLabelJobTypeId" runat="server" Visible="false" Text='<%#Eval("JobTypeId")%>' ToolTip="ToolTip"/>
                                                   <asp:Label ID="modalLabelDepartmentId" runat="server" Visible="false" Text='<%#Eval("DepartmentId")%>' ToolTip="ToolTip"/>
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
                    <ajaxToolkit:FilteredTextBoxExtender ID="filterPosition" runat="server" TargetControlID="modalTxtBoxPosition" FilterType="Numbers"/>
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
