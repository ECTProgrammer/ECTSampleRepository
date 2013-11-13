<%@ Page Title="Job Tracker" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JobTrack.aspx.cs" Inherits="TimeTracker.JobTrack" EnableEventValidation="true"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

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
            <div style="text-align:right">Select Date : <asp:TextBox ID="txtBoxDate"  runat="server" AutoPostBack="true" OnTextChanged="txtBoxDate_TextChanged"></asp:TextBox></div>
            <table style="width:100%;">
                <tr><td style="text-align:center"><h2><asp:Label ID="labelDay" runat="server"></asp:Label></h2></td></tr>
            </table>
            <ajaxToolKit:CalendarExtender ID="calendarExtenderDate" runat="server" TargetControlID="txtBoxDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
            <table style="width:100%;">
                <tr><td><asp:LinkButton ID="linkBtnAddJobTrack" runat="server" CausesValidation="false" Text="Add Job" CssClass="linkButton" Font-Bold="true" OnClick="linkBtnAddJobTrack_Click"></asp:LinkButton> 
                </td><td style="text-align:right"><asp:Label ID="LabelTotalHours" ForeColor="#ff0000" Font-Size="12px" runat="server" Font-Bold="true"></asp:Label></td></tr></table>   
            <div style="margin:10px 10px 10px 10px">
            <asp:GridView ID="gridJobTrack" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" OnRowCommand="gridViewJobTrack_Command" ShowHeaderWhenEmpty="true">
                <EmptyDataRowStyle/>
                    <EmptyDataTemplate>
                     No Data Found.
                    </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="Id" Visible="false" />
                    <asp:BoundField HeaderText="Start Time" DataField="StartTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                    <asp:BoundField HeaderText="End Time" DataField="EndTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                    <asp:BoundField HeaderText="Description of Work" DataField="jobtype" ReadOnly="true"></asp:BoundField>
                    <asp:BoundField HeaderText="JobId" DataField="JobIdNumber" ReadOnly="true"></asp:BoundField>
                    <asp:BoundField HeaderText="Job Status" DataField="JobStatus" ReadOnly="true"></asp:BoundField>
                    <asp:BoundField HeaderText="Customer" DataField="customer" ReadOnly="false"></asp:BoundField>
                    <asp:BoundField HeaderText="Number of Hours" DataField="totalhours" ReadOnly="false"></asp:BoundField>
                    <asp:BoundField HeaderText="Remarks" DataField="Remarks"></asp:BoundField>
                    <asp:BoundField HeaderText="Status" DataField="Status"></asp:BoundField>
                </Columns>
            </asp:GridView>
            </div>
        </asp:Panel>
      </asp:Panel>

    <%--Modal--%>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="modalBtnSubmit" />
            <asp:PostBackTrigger ControlID="modalBtnDelete" />
        </Triggers>--%>
        <ContentTemplate>
            <asp:UpdateProgress ID="modalUpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                <ProgressTemplate>
                    <div class="loading">
                        <asp:Image ID="modalImageLoading" runat="server" ImageUrl="Images/loading.gif" AlternateText="Loading..." ToolTip="Loading..." style="padding: 10px;position:fixed;top:45%;left:50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

            <asp:Panel Id="panelModal" runat="server" style="display:none">
                <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalHeader">Job Tracker</asp:Panel>
                <asp:Panel ID="panelModalContent" runat="server" CssClass="modal">
                    <asp:Label ID ="modalLabelHWSW" runat="server" Visible="false"></asp:Label>
                    <table style="padding:3px;border-spacing:10px;" border="0">
                        <tr><td colspan="3"><asp:Label ID="modalLabelError" runat="server" Visible="false" CssClass="validation"></asp:Label></td></tr>
                        <tr><td><asp:Label ID="modalLabelSupervisor" runat="server" Text="Supervisor" Visible="false"></asp:Label></td><td><asp:Label ID="modalLabelSupColon" runat="server" Text=":" Visible="false"></asp:Label></td>
                            <td><asp:DropDownList ID="modalDropDownSupervisor" runat="server" Visible="false" CssClass="dropDownList1"></asp:DropDownList></td>
                        </tr>
                        <tr><td>Description of Work</td><td>:</td><td><asp:DropDownList ID="modalDropDownJobType" runat="server" AutoPostBack="true" CssClass="dropDownList1" OnSelectedIndexChanged="modalDropDownJobType_IndexChanged"></asp:DropDownList></td></tr>
                        <tr><td>Job Id</td><td>:</td><td><asp:TextBox ID="modalTxtBoxJobId" runat="server" Enabled="false" AutoPostBack="true" OnTextChanged="modalTxtBoxJobId_TextChanged"></asp:TextBox></td></tr>
                        <tr><td>Job Status</td><td>:</td><td><asp:DropDownList ID="modalDropDownJobStatus" Enabled="false" runat="server">
                            <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
                            <asp:ListItem Text="On Hold" Value="On Hold"></asp:ListItem>
                            <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                            </asp:DropDownList></td></tr>
                        <tr><td>Job Description</td><td>:</td><td><asp:Label ID="modallabelBoxJobDescription" runat="server"></asp:Label></td></tr>
                        <tr><td>Customer</td><td>:</td><td><asp:Label ID="modallabelCustomer" runat="server"></asp:Label></td></tr>
                        <tr><td>Start Time</td><td>:</td><td><asp:DropDownList ID="modalDropDownStartTime" runat="server" AutoPostBack="true" OnSelectedIndexChanged="modalDropDownStartTime_IndexChanged"></asp:DropDownList></td></tr>
                        <tr><td>End Time</td><td>:</td><td><asp:DropDownList ID="modalDropDownEndTime" runat="server"></asp:DropDownList></td></tr>
                        <tr><td>Remarks</td><td>:</td><td></td></tr>
                        <tr><td colspan="3"><asp:TextBox ID="modalTxtBoxRemarks" runat="server" TextMode="MultiLine" Rows="5" Width="100%"></asp:TextBox></td></tr>
                        <tr><td colspan="3" style="text-align:center">
                            <asp:Button ID="modalBtnDelete" runat="server" CssClass="button" OnClientClick="if(!confirm('Are you sure you want to delete this?')) return false;" CausesValidation="true" Text="Delete" OnCommand="modalBtnDelete_Command" />
                            <asp:Button ID="modalBtnSubmit" runat="server" CssClass="button" CausesValidation="true" Text="Submit" OnCommand="modalBtnSubmit_Command" />
                            <asp:Button ID="modalBtnCancel" runat="server" CssClass="button" CausesValidation="false" Text="Cancel" />
                            </td></tr>
                    </table>
                </asp:Panel>

        <%--Alert Modal--%>
                <asp:UpdatePanel ID="UpdatePanelAlert" runat="server" >
                    <ContentTemplate>
                        <asp:Panel ID="panelAlertModal" runat="server" style="display:none">
                            <asp:Panel ID="panelAlertHeader" runat="server" CssClass="modalAlertHeader"><asp:Label ID="labelAlertHeader" runat="server"></asp:Label></asp:Panel>
                            <asp:Panel ID="panelAlertContent" runat="server" CssClass="modalAlert">
                                <table>
                                    <tr><td style="text-wrap:normal"><asp:Label ID="labelAlertMessage" runat="server"></asp:Label></td></tr>
                                    <tr><td style="text-align:center"><asp:Button ID="alertModalBtnOK" runat="server" CssClass="buttonalert" Text="OK" /></td></tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Button ID="btnalertHidden" runat="server" style="display:none" />
                        <ajaxToolKit:ModalPopupExtender runat="server" ID="programmaticAlertModalPopup"
                            BehaviorID ="programmaticModalPopupBehaviorid"
                            TargetControlID="btnalertHidden"
                            PopupControlID="panelAlertModal"
                            BackgroundCssClass="modalAlertBackground"
                            CancelControlID="alertModalBtnOK"
                            DropShadow="false"
                            RepositionMode="RepositionOnWindowResizeAndScroll" >    
                        </ajaxToolKit:ModalPopupExtender> 
                    </ContentTemplate>
                </asp:UpdatePanel>

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

           <asp:UpdatePanel ID="UpdatePanelAlert2" runat="server" >
                    <ContentTemplate>
                        <asp:Panel ID="panelAlertModal2" runat="server" style="display:none">
                            <asp:Panel ID="panelAlertHeader2" runat="server" CssClass="modalAlertHeader"><asp:Label ID="labelAlertHeader2" runat="server"></asp:Label></asp:Panel>
                            <asp:Panel ID="panelAlertContent2" runat="server" CssClass="modalAlert">
                                <table>
                                    <tr><td style="text-wrap:normal"><asp:Label ID="labelAlertMessage2" runat="server"></asp:Label></td></tr>
                                    <tr><td style="text-align:center"><asp:Button ID="alertModalBtnOK2" runat="server" CssClass="buttonalert" Text="OK" /></td></tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Button ID="btnalertHidden2" runat="server" style="display:none" />
                        <ajaxToolKit:ModalPopupExtender runat="server" ID="programmaticAlertModalPopup2"
                            BehaviorID ="programmaticModalPopupBehavior2"
                            TargetControlID="btnalertHidden2"
                            PopupControlID="panelAlertModal2"
                            BackgroundCssClass="modalAlertBackground"
                            CancelControlID="alertModalBtnOK2"
                            DropShadow="false"
                            RepositionMode="RepositionOnWindowResizeAndScroll" >    
                        </ajaxToolKit:ModalPopupExtender> 
                    </ContentTemplate>
            </asp:UpdatePanel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
