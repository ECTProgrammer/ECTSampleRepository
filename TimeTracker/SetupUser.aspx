<%@ Page Title="Setup User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupUser.aspx.cs" Inherits="TimeTracker.SetupUser" %>
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

           <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">User Setup</asp:Panel>
	       <asp:Panel ID="panelContent" runat="server" CssClass="modal">
		        <asp:Label ID="labelAccessDenied" runat="server" Text="You do not have access rights to this page. Please contact your administrator if you need access. Thank you." Visible="false" CssClass="validation"></asp:Label>
		        <asp:Panel ID="panelAccessOK" runat="server">
			        <table style="width:100%;">
                        <tr><td>Department : <asp:DropDownList ID="dropDownListDepartment" runat="server" CssClass="dropDownList1" AutoPostBack="true" OnSelectedIndexChanged="dropDownDepartment_Changed"></asp:DropDownList></td></tr>
                        <tr><td>Status : <asp:RadioButtonList ID="radioBtnListStatus" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Table" OnSelectedIndexChanged="radioBtnStatus_Changed">
                            <asp:ListItem Value="All" Text="All" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="Active" Text="Active"></asp:ListItem>
                            <asp:ListItem Value="Inactive" Text="Inactive"></asp:ListItem></asp:RadioButtonList></td></tr>
				        <tr><td style="text-align:left"><asp:LinkButton ID="linkBtnAddUser" runat="server" CausesValidation="false" Text="Add New User" CssClass="linkButton" Font-Bold="true" OnClick="linkBtnAddUser_Click"></asp:LinkButton></td></tr></table>   
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
                            <asp:BoundField HeaderText="Base Rate" DataField="currentBaseRate" />
                            <asp:BoundField HeaderText="OT Rate" DataField="currentOTRate" />
                            <asp:BoundField HeaderText="Special Rate" DataField="currentSpecialRate" />
				        </Columns>
			        </asp:GridView>
			        </div>
		        </asp:Panel>
	        </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>

    <%--Modal--%>
    <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="modalBtnSubmit" />--%>
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
                        <tr><td>Employee No</td><td>:</td><td><asp:TextBox ID="modalTxtBoxEmployeeNo" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqEmployeeNo" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxEmployeeNo" Text="*"></asp:RequiredFieldValidator></td></tr>
                        <tr><td>Firstname</td><td>:</td><td><asp:TextBox ID="modalTxtBoxFirstname" ValidationGroup="modal" runat="server" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqFirstname" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxFirstname" Text="*"></asp:RequiredFieldValidator></td></tr>
                        <tr><td>Lastname</td><td>:</td><td><asp:TextBox ID="modalTxtBoxLastname" ValidationGroup="modal" runat="server" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqLastname" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxLastname" Text="*"></asp:RequiredFieldValidator></td></tr>
                        <tr><td>Username</td><td>:</td><td><asp:TextBox ID="modalTxtBoxUsername" runat="server" AutoPostBack="true" ValidationGroup="modal" CssClass="textBox" OnTextChanged="modalTxtBoxUser_Changed"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqUsername" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxUsername" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="modalRegValUsername" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxUsername" Text="Minimum 3 characters." ValidationExpression=".{3}.*"></asp:RegularExpressionValidator></td></tr>
                        <tr><td>Password</td><td>:</td><td><asp:TextBox ID="modalTxtBoxPassword" runat="server" ValidationGroup="modal" CssClass="textBox" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqPassword" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxPassword" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="modalRegValPassword" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxPassword" Text="Minimum 3 characters." ValidationExpression=".{3}.*"></asp:RegularExpressionValidator></td></tr>
                        <tr><td>Department</td><td>:</td><td><asp:DropDownList ID="modalDropDownDepartment" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                        <tr><td>Role</td><td>:</td><td><asp:DropDownList ID="modalDropDownRole" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                        <tr><td>Shift</td><td>:</td><td><asp:RadioButtonList ID="modalRadBtnListShift" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="AM" Value="AM"/>
                            <asp:ListItem Text="PM" Value="PM"/>
                                                        </asp:RadioButtonList></td></tr>
                        <tr><td>Email</td><td>:</td><td><asp:TextBox ID="modalTxtBoxEmail" runat="server" ValidationGroup="modal" CssClass="textBox" TextMode="Email"></asp:TextBox></td></tr>
                        <tr><td>Phone</td><td>:</td><td><asp:TextBox ID="modalTxtBoxPhone" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox></td></tr>
                        <tr><td>Mobile</td><td>:</td><td><asp:TextBox ID="modalTxtBoxMobile" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox></td></tr>
                        <tr><td>Fax</td><td>:</td><td><asp:TextBox ID="modalTxtBoxFax" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox></td></tr>
                        <tr><td>Status</td><td>:</td><td><asp:DropDownList ID="modalDropDownStatus" runat="server" ValidationGroup="modal" CssClass="dropDownList1">
                            <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                                         </asp:DropDownList></td></tr>
                        <tr><td colspan="3"><asp:CheckBox ID="modalChkBoxUpdateRate" runat="server" Text="Update Salary Rate" ValidationGroup="modal" AutoPostBack="true" OnCheckedChanged="modalChkBoxUpdateRate_Changed" /></td></tr>
                        <tr><td colspan="3"><asp:Panel ID="modalRatePanel" runat="server">
                            <table style="padding:10px; border-collapse:separate;border-spacing:10px;">
                                            <tr><td>Start Time</td><td>:</td><td><asp:TextBox ID="modalTxtBoxStartTime" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="modalReqStartTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxStartTime" Text="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="modalRegValStartTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxStartTime" Text="Invalid Value" ValidationExpression="([0-1][0-9]:[0-5][0-9])|(2[0-3]:[0-5][0-9])"></asp:RegularExpressionValidator>
                                                                             (00:00-23:59)</td></tr>
                                            <tr><td>End Time</td><td>:</td><td><asp:TextBox ID="modalTxtBoxEndTime" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="modalReqEndTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxEndTime" Text="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="modalRegValEndTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxEndTime" Text="Invalid Value" ValidationExpression="([0-1][0-9]:[0-5][0-9])|(2[0-3]:[0-5][0-9])"></asp:RegularExpressionValidator>
                                                                             (00:00-23:59)</td></tr>
                                            <tr><td>Base Rate</td><td>:</td><td><asp:TextBox ID="modalTxtBoxBaseRate" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="modalReqBaseRate" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxBaseRate" Text="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="modalRegValBaseRate" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxBaseRate" Text="Invalid Value" ValidationExpression="^\d*\.?\d*$"></asp:RegularExpressionValidator></td></tr>
                                            <tr><td>Over Time Rate</td><td>:</td><td><asp:TextBox ID="modalTxtBoxOTRate" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="modalReqOTRate" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxOTRate" Text="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="modalRegValOTRate" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxOTRate" Text="Invalid Value" ValidationExpression="^\d*\.?\d*$"></asp:RegularExpressionValidator></td></tr>
                                            <tr><td>Special Rate</td><td>:</td><td><asp:TextBox ID="modalTxtBoxSpecialRate" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="modalReqSpecialRate" runat="server" CssClass="validation" ValidationGroup="modal" ControlToValidate="modalTxtBoxSpecialRate" Text="*"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="modalRegValSpecialRate" runat="server" ValidationGroup="modalUserlink" CssClass="validation" ControlToValidate="modalTxtBoxSpecialRate" Text="Invalid Value" ValidationExpression="^\d*\.?\d*$"></asp:RegularExpressionValidator></td></tr>
                                        </table>
                                            </asp:Panel></td></tr>
                        <tr>
                            <td colspan="3" style="text-align:center">
                                <asp:Button ID="modalBtnSubmit" runat="server" Text="Submit" ValidationGroup="modal" CausesValidation="true" CssClass="button" OnCommand="modalBtnSubmit_Command"/>
                                <asp:Button ID="modalBtnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                    <ajaxToolkit:FilteredTextBoxExtender ID="filterEmployeeNo" runat="server" TargetControlID="modalTxtBoxEmployeeNo" FilterType="Numbers"/>
                </asp:Panel>
            </asp:Panel>
            <ajaxToolkit:CollapsiblePanelExtender ID="cpeSalaryRate" runat="server" 
                        TargetControlID ="modalRatePanel"
                        CollapsedSize="0"
                        Collapsed ="True"
                        ExpandControlID="modalChkBoxUpdateRate"
                        CollapseControlID="modalChkBoxUpdateRate"
                        AutoExpand="false"
                        AutoCollapse="false"
                        ExpandDirection="Vertical"
                        />
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
