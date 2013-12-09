<%@ Page Title="Setup Job Type" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupJobType.aspx.cs" Inherits="TimeTracker.SetupJobType" %>
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

           <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Job Tracker</asp:Panel>
	       <asp:Panel ID="panelContent" runat="server" CssClass="modal">
		        <asp:Label ID="labelAccessDenied" runat="server" Text="You do not have access rights to this page. Please contact your administrator if you need access. Thank you." Visible="false" CssClass="validation"></asp:Label>
		        <asp:Panel ID="panelAccessOK" runat="server">
			        <table style="width:100%;">
                        <tr><td>Department : <asp:DropDownList ID="dropDownListDepartment" runat="server" CssClass="dropDownList1" AutoPostBack="true" OnSelectedIndexChanged="dropDownDepartment_Changed"></asp:DropDownList></td></tr>
				        <tr><td style="text-align:left"><asp:LinkButton ID="linkBtnAdd" runat="server" CausesValidation="false" Text="Add Job Type" CssClass="linkButton" Font-Bold="true" OnClick="linkBtnAdd_Click"></asp:LinkButton></td></tr>
                    </table>
			        <div style="margin:10px 10px 10px 10px">
			        <asp:GridView ID="gridViewJobType" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" OnRowCommand="gridViewJobType_Command" ShowHeaderWhenEmpty="true">
				        <EmptyDataRowStyle/>
					        <EmptyDataTemplate>
					         No Data Found.
					        </EmptyDataTemplate>
				        <Columns>
					        <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="labelJobTypeId" runat="server" Text='<%#Eval("Id")%>' Visible="false"></asp:Label>
                                </ItemTemplate>
					        </asp:TemplateField>
					        <asp:BoundField HeaderText="JobType" DataField="Description" ReadOnly="true"/>
                            <asp:BoundField HeaderText="Acronym" DataField="Acronym" ReadOnly="true" />
                            <asp:BoundField HeaderText="Position" DataField="Position" ReadOnly="true" />
				        </Columns>
			        </asp:GridView>
			        </div>
		        </asp:Panel>
	        </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>

    <%--Modal--%>
    <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
<%--        <Triggers>
            <asp:PostBackTrigger ControlID="modalBtnSubmit" />
        </Triggers>--%>
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
                    <asp:Label ID="modalLabelJobTypeId" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="modalLabelError" runat="server" CssClass="validation" Visible="false"></asp:Label>
                    <table style="padding:10px; border-collapse:separate;border-spacing:10px; width:100%">
                        <tr><td>Description</td><td>:</td><td><asp:TextBox ID="modalTxtBoxDescription" runat="server" ValidationGroup="modal" CssClass="textBox" OnTextChanged="modalDescription_Changed"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqDescription" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxDescription" Text="*"></asp:RequiredFieldValidator></td></tr>
                        <tr><td>Acronym</td><td>:</td><td><asp:TextBox ID="modalTxtBoxAcronym" runat="server" ValidationGroup="modal" CssClass="textBox" OnTextChanged="modalAcronym_Changed" MaxLength="10"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqAcronym" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxDescription" Text="*" /></td></tr>
                        <tr><td>Position</td><td>:</td><td><asp:TextBox ID="modalTxtBoxPosition" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqPosition" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxPosition" Text="*"></asp:RequiredFieldValidator></td></tr>
                        <tr><td>Required Job Id</td><td>:</td><td><asp:CheckBox ID="modalChkBoxRequiredJobId" runat="server" /></td></tr>
                        <tr><td>Compute Time</td><td>:</td><td><asp:CheckBox ID="modalChkBoxComputeTime" runat="server" /></td></tr>
                        <tr><td>Show in Job Overview</td><td>:</td><td><asp:CheckBox ID="modalChkBoxShowJobOverview" runat="server" /></td></tr>
                        <tr><td colspan="3"><asp:CheckBox ID="modalChkboxAll" runat="server" Text=" Select All" AutoPostBack="true" OnCheckedChanged="modalChkboxAll_Changed" /></td></tr>
                        <tr>
                            <td colspan="3"><asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" style="max-height:350px">
                                <asp:GridView ID="modalGridViewDepartment" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:BoundField HeaderText="Department" DataField="Description" />
                                        <asp:TemplateField HeaderText="Position">
                                        <ItemTemplate>
                                            <asp:TextBox ID="modalTxtBoxDeptPosition" runat="server" CssClass="textBox"></asp:TextBox>
                                            <ajaxToolkit:FilteredTextBoxExtender ID="filterDepPosition" runat="server" TargetControlID="modalTxtBoxDeptPosition" FilterType="Numbers"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="modalChkBoxSelect" runat="server" />
                                            <asp:Label ID="modalLabelDepartmentId" runat="server" Visible="false" Text='<%#Eval("Id")%>' ToolTip="ToolTip"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align:center">
                                <asp:Button ID="modalBtnSubmit" runat="server" Text="Submit" ValidationGroup="modal" CausesValidation="true" CssClass="button" OnCommand="modalBtnSubmit_Command"/>
                                <asp:Button ID="modalBtnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="button" />
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
