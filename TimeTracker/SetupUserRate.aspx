<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SetupUserRate.aspx.cs" Inherits="TimeTracker.SetupUserRate" %>
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

            <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Setup User Rate</asp:Panel>
            <asp:Panel ID="panelContent" runat="server" CssClass="modal">
                 <asp:Panel ID="panelAccessOK" runat="server">
                     <table style="width:100%;">
                        <tr><td>Department : <asp:DropDownList ID="dropDownListDepartment" runat="server" CssClass="dropDownList1" AutoPostBack="true" OnSelectedIndexChanged="dropDownDepartment_Changed"></asp:DropDownList></td></tr>
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

    <%--Modal--%>
    <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
        <Triggers></Triggers>
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
                        <tr><td>Employee No</td><td>:</td><td><asp:Label ID="modalLabelEmployeeNo" runat="server"  ValidationGroup="modal"/></td></tr>
                        <tr><td>First Name</td><td>:</td><td><asp:Label ID="modalLabelFirstName" runat="server"  ValidationGroup="modal"/></td></tr>
                        <tr><td>Last Name</td><td>:</td><td><asp:Label ID="modalLableLastName" runat="server"  ValidationGroup="modal"/></td></tr>
                        <tr><td>Start Date</td><td>:</td><td><asp:TextBox ID="modalTxtBoxStartDate" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox></td></tr>
                        <tr><td>Start Time</td><td>:</td><td><asp:TextBox ID="modalTxtBoxStartTime" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqStartTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxStartTime" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="modalRegValStartTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxStartTime" Text="Invalid Value" ValidationExpression="([0-1][0-9]:[0-5][0-9])|(2[0-3]:[0-5][0-9])"></asp:RegularExpressionValidator>
                                                            (00:00-23:59)</td></tr>
                        <tr><td>End Time</td><td>:</td><td><asp:TextBox ID="modalTxtBoxEndTime" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqEndTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxEndTime" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="modalRegValEndTime" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxEndTime" Text="Invalid Value" ValidationExpression="([0-1][0-9]:[0-5][0-9])|(2[0-3]:[0-5][0-9])"></asp:RegularExpressionValidator>
                                                            (00:00-23:59)</td></tr>
                        <tr><td>Monthly Salary</td><td>:</td><td><asp:TextBox ID="modalTxtBoxSalary" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqSalary" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxSalary" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="modalRegValSalary" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxSalary" Text="Invalid Value" ValidationExpression="^\d*\.?\d*$"></asp:RegularExpressionValidator></td></tr>
                        <tr><td>Break Time in Mins</td><td>:</td><td><asp:TextBox ID="modalTxtBoxBreakTimeMin" runat="server" ValidationGroup="modal" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="modalReqBreakTimeMin" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxBreakTimeMin" Text="*"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="modalRegValBreakTimeMin" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxBreakTimeMin" Text="Invalid Value" ValidationExpression="^\d*$"></asp:RegularExpressionValidator></td></tr>
                        
                        <tr><td>Use Pattern For Off Days</td><td>:</td><td><asp:CheckBox ID="modalChckBoxUsePattern" ValidationGroup="modal" runat="server" AutoPostBack="true" OnCheckedChanged="modalChckBoxUsePattern_Changed" /></td></tr>
                        <tr><td colspan="3"><asp:Panel ID="modalPanelNormal" runat="server">
                                <table style="padding:2px; border-collapse:separate;border-spacing:5px;">
                                    <tr><td>Off Day</td><td>:</td><td><asp:DropDownList ID="modalDropDownOffDay" ValidationGroup="modal" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                                    <tr><td>Special Off Day</td><td>:</td><td><asp:DropDownList ID="modalDropDownSpecialOffDay" ValidationGroup="modal" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                                    <tr><td>Optional Off Day 1</td><td>:</td><td><asp:DropDownList ID="modalDropDownOptionalOffDay1" ValidationGroup="modal" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                                    <tr><td>Optional Off Day 2</td><td>:</td><td><asp:DropDownList ID="modalDropDownOptionalOffDay2" ValidationGroup="modal" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                                    <tr><td>Optional Off Day 3</td><td>:</td><td><asp:DropDownList ID="modalDropDownOptionalOffDay3" ValidationGroup="modal" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                                    <tr><td>Optional Off Day 4</td><td>:</td><td><asp:DropDownList ID="modalDropDownOptionalOffDay4" ValidationGroup="modal" runat="server" CssClass="dropDownList1"></asp:DropDownList></td></tr>
                                    <tr><td>No Overtime Pay</td><td>:</td><td><asp:CheckBox ID="modalChckBoxNoOTPay" ValidationGroup="modal" runat="server" /></td></tr>
                                    <tr><td>Is Office Worker</td><td>:</td><td><asp:CheckBox ID="modalChckBoxOfficeWorker" ValidationGroup="modal" runat="server" /></td></tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="modalPanelPattern" runat="server">
                                <table style="padding:2px; border-collapse:separate;border-spacing:5px;">
                                    <tr><td>Pattern Start Date</td><td>:</td><td><asp:TextBox ID="modalTxtBoxPatternStartDate" runat="server" ValidationGroup="modal"  CssClass="textBox"></asp:TextBox></td></tr>
                                    <tr><td>Pattern</td><td>:</td><td><asp:TextBox ID="modalTxtBoxPattern" runat="server" ValidationGroup="modal"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="modalReqPattern" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxPattern" Text="*"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="modalRegValPattern" runat="server" ValidationGroup="modal" CssClass="validation" ControlToValidate="modalTxtBoxPattern" Text="Invalid Value" ValidationExpression="^[0-9]{1,2}(\s*,\s*[0-9]{1,2})*$"></asp:RegularExpressionValidator></td></tr>
                                </table>
                            </asp:Panel>
                        </td></tr>
                        <td colspan="3" style="text-align:center">
                                <asp:Button ID="modalBtnSubmit" runat="server" Text="Submit" ValidationGroup="modal" CausesValidation="true" CssClass="button" OnCommand="modalBtnSubmit_Command"/>
                                <asp:Button ID="modalBtnCancel" runat="server" Text="Cancel" CausesValidation="false" CssClass="button" />
                            </td>
                    </table>
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderStartDate" runat="server" TargetControlID="modalTxtBoxStartDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderPatternStartDate" runat="server" TargetControlID="modalTxtBoxPatternStartDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                    <ajaxToolkit:FilteredTextBoxExtender ID="filterBreakTimeMin" runat="server" TargetControlID="modalTxtBoxBreakTimeMin" FilterType="Numbers"/>
                </asp:Panel>
            </asp:Panel>
            <ajaxToolkit:CollapsiblePanelExtender ID="cpeNormalUse" runat="server" 
                        TargetControlID ="modalPanelNormal"
                        CollapsedSize="0"
                        Collapsed ="True"
                        ExpandControlID="modalChckBoxUsePattern"
                        CollapseControlID="modalChckBoxUsePattern"
                        AutoExpand="false"
                        AutoCollapse="false"
                        ExpandDirection="Vertical"
                        />
            <ajaxToolkit:CollapsiblePanelExtender ID="cpePatternUse" runat="server" 
                        TargetControlID ="modalPanelPattern"
                        CollapsedSize="0"
                        Collapsed ="True"
                        ExpandControlID="modalChckBoxUsePattern"
                        CollapseControlID="modalChckBoxUsePattern"
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
