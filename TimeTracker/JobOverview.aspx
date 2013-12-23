<%@ Page Title="Job Overview" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="JobOverview.aspx.cs" Inherits="TimeTracker.JobOverview" %>
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
           
            <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Job Overview</asp:Panel>
            <asp:Panel ID="panelContent" runat="server" CssClass="modal">
                <asp:Label ID="labelAccessDenied" runat="server" Text="You do not have access rights to this page. Please contact your administrator if you need access. Thank you." Visible="false" CssClass="validation"></asp:Label>
                <asp:Panel ID="panelAccessOK" runat="server">
                    <div style="text-align:right">
                        <table style="margin:4px 4px 4px 4px; text-align:right">
                            <tr><td>
                                <table><tr>
                                <td>Select Date :</td>
                                <td><asp:TextBox ID="txtBoxStartDate"  runat="server" AutoPostBack="true" OnTextChanged="txtBoxStartDate_Changed" ></asp:TextBox></td>
                                <td><asp:TextBox ID="txtBoxEndDate"  runat="server" AutoPostBack="true" OnTextChanged="txtBoxEndDate_Changed"></asp:TextBox></td>
                                </tr></table>
                                </td>
                                <td>
                                    <table><tr>
                                <td>Job ID:</td>
                                <td><asp:TextBox ID="txtBoxJobId"  runat="server" AutoPostBack="true" OnTextChanged="txtBoxJobId_Changed" ></asp:TextBox></td>
                                </tr></table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderStartDate" runat="server" TargetControlID="txtBoxStartDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender> 
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderEndDate" runat="server" TargetControlID="txtBoxEndDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                    <ajaxToolKit:TabContainer ID="tabContainerJobOverview" runat="server">
                        <ajaxToolKit:TabPanel ID="tabPanelJobSummary" runat="server" HeaderText="Job Overview Summary">
                            <ContentTemplate>
                                    <asp:Panel runat="server" ScrollBars="Auto" Width="100%" style="max-height:350px">
                                        <div style="margin:10px 10px 10px 10px">
                                            <asp:GridView ID="gridViewSummary" runat="server" AutoGenerateColumns="false" CssClass="gridView" ShowHeaderWhenEmpty="true"  OnRowDataBound="gridViewSummary_RowDataBound">
                                                <EmptyDataRowStyle/>
                                                    <EmptyDataTemplate>
                                                        No Data Found.
                                                    </EmptyDataTemplate>
                                                <Columns>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                            </ContentTemplate>
                        </ajaxToolKit:TabPanel>
                        <ajaxToolKit:TabPanel ID="tabPanelJobDetails" runat="server" HeaderText="Job Overview Details">
                            <ContentTemplate>
                                    <asp:Panel runat="server" ScrollBars="Auto" Width="100%" style="max-height:350px">
                                        <div style="margin:10px 10px 10px 10px">
                                            <asp:GridView ID="gridViewDetail" runat="server" AutoGenerateColumns="false" CssClass="gridView" ShowHeaderWhenEmpty="true"  OnRowCreated="gridViewDetail_RowCreated" OnRowDataBound="gridViewDetail_RowDataBound">
                                                <EmptyDataRowStyle/>
                                                    <EmptyDataTemplate>
                                                        No Data Found.
                                                    </EmptyDataTemplate>
                                                <Columns>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                            </ContentTemplate>
                        </ajaxToolKit:TabPanel>
                        <ajaxToolKit:TabPanel ID="tabPanelJobFlow" runat="server" HeaderText="Process Flow">
                            <ContentTemplate>
                                    <asp:Panel  runat="server" ScrollBars="Auto" Width="100%" style="max-height:500px">
                                        <div style="margin:10px 10px 10px 10px">
                                            <asp:GridView ID="gridViewJobFlow" runat="server" AutoGenerateColumns="false" CssClass="gridView" ShowHeaderWhenEmpty="true"  OnRowCreated="gridViewJobFlow_RowCreated" OnRowDataBound="gridViewJobFlow_RowDataBound">
                                                <EmptyDataRowStyle/>
                                                    <EmptyDataTemplate>
                                                        No Data Found.
                                                    </EmptyDataTemplate>
                                                <Columns>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </asp:Panel>
                            </ContentTemplate>
                        </ajaxToolKit:TabPanel>
                    </ajaxToolKit:TabContainer>
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

            <asp:Panel Id="panelModalSummary" runat="server" style="display:none">
                <asp:Panel ID="panelModalHeaderSummary" runat="server" CssClass="modalHeader">Details</asp:Panel>
                <asp:Panel ID="panelModalContentSummary" runat="server" CssClass="modal">
                    <table style="padding:10px; border-collapse:separate;border-spacing:10px; width:100%">
                        <asp:Repeater ID ="modalSummaryRepeater" runat="server">
                            <ItemTemplate>
                                <tr><td><asp:Label ID="modalSummaryLabelTitle" runat="server" Text='<%#Eval("Key") %>' /></td><td>:</td>
                                    <td><asp:Label ID="modalSummaryLabelValue" runat="server" Text='<%#Eval("Value") %>'></asp:Label></td></tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr>
                            <td colspan="3" style="text-align:center">
                                <asp:Button ID="modalSummaryBtnCancel" runat="server" Text="Close" CausesValidation="false" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>

            <asp:Panel Id="panelModalDetail" runat="server" style="display:none">
                <asp:Panel ID="panelModalHeaderDetail" runat="server" CssClass="modalHeader">Info</asp:Panel>
                <asp:Panel ID="panelModalContentDetail" runat="server" CssClass="modal">
                    <table style="padding:10px; border-collapse:separate;border-spacing:10px;width:450px">
                        <tr><td>Name</td><td>:</td><td><asp:Label ID="modalDetailLabelName" runat="server"></asp:Label></td></tr>
                        <tr><td>Description of Work</td><td>:</td><td><asp:Label ID="modalDetailLabelJobType" runat="server" ValidationGroup="modal"></asp:Label></td></tr>
                        <tr><td>Job Status</td><td>:</td><td><asp:Label ID="modalDetailLabelJobStatus" runat="server" ValidationGroup="modal"></asp:Label></td></tr>
                        <tr><td>Date</td><td>:</td><td><asp:Label ID="modalDetailLabelDate" runat="server"></asp:Label></td></tr>
                        <tr><td>Start Time</td><td>:</td><td><asp:Label ID="modalDetailLabelStartTime" runat="server" ValidationGroup="modal"></asp:Label></td></tr>
                        <tr><td>End Time</td><td>:</td><td><asp:Label ID="modalDetailLabelEndTime" runat="server" ValidationGroup="modal"></asp:Label></td></tr>
                        <tr><td>Remarks</td><td>:</td><td></td></tr>
                        <tr><td colspan="3"><asp:TextBox ID="modalDetailTxtBoxRemarks" runat="server" TextMode="MultiLine" ValidationGroup="modal" ReadOnly="true" Rows="5" Width="100%"></asp:TextBox></td></tr>
                        <tr>
                            <td colspan="3" style="text-align:center" >
                                <asp:Button ID="modalDetailBtnCancel" runat="server" Text="Close" CausesValidation="false" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>

            <asp:Button ID="btnHiddenDetail" runat="server" style="display:none"/>
            <asp:Button ID="btnHiddenSummary" runat="server" style="display:none"/>
            <ajaxToolKit:ModalPopupExtender runat="server" ID="programmaticModalPopupDetail"
                BehaviorID ="programmaticModalPopupBehavior"
                TargetControlID="btnHiddenDetail"
                PopupControlID="panelModalDetail"
                BackgroundCssClass="modalBackground"
                CancelControlID="modalDetailBtnCancel"
                DropShadow="false"
                PopupDragHandleControlID="panelModalHeaderDetail"
                RepositionMode="RepositionOnWindowResize" >    
            </ajaxToolKit:ModalPopupExtender> 

            <ajaxToolKit:ModalPopupExtender runat="server" ID="programmaticModalPopupSummary"
                BehaviorID ="programmaticModalPopupBehaviorSummary"
                TargetControlID="btnHiddenSummary"
                PopupControlID="panelModalSummary"
                BackgroundCssClass="modalBackground"
                CancelControlID="modalSummaryBtnCancel"
                DropShadow="false"
                PopupDragHandleControlID="panelModalHeaderSummary"
                RepositionMode="RepositionOnWindowResize" >    
            </ajaxToolKit:ModalPopupExtender> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

