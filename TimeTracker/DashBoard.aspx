<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="TimeTracker.DashBoard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:70%; width:65%; min-height:300px; float:left;"> 
    <asp:UpdatePanel ID="UpdatePanelLeft" runat="server">   
       <ContentTemplate>
       <asp:Panel ID="panelLeftHeader" runat="server" CssClass="modalHeader" Width="98%">Waiting for Approval</asp:Panel>
        <asp:Panel ID="panelLeftContent" runat="server" CssClass="modalLeft" Width="98%">
            <ajaxToolKit:TabContainer ID="tabContainerLeft" runat="server">
                <ajaxToolKit:TabPanel ID="tabPanelLeft1" runat="server" HeaderText="Waiting for your Approval">
                    <ContentTemplate>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="300px">
                            <table><tr><td style="text-align:left"><asp:CheckBox ID="gridLeftChkBoxSelectAll" runat="server" Text=" Select All" AutoPostBack="true" OnCheckedChanged="gridLeftChkBoxSelectAll_Changed"/></td></tr>
                            <tr><td><asp:GridView ID="gridViewLeftPanel1" runat="server" AutoGenerateColumns="False" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="True" OnRowCommand="gridViewLeftPanel1_RowCommand">
                                <EmptyDataTemplate>
                                    No Data Found.
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField Visible ="false">
                                        <ItemTemplate>
                                            <asp:Label ID="gridLeftlblJobTrackId" runat="server" Text='<%#Eval("Id")%>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="gridLeftChkBoxSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Requested By" DataField="fullname"/>
                                    <asp:BoundField HeaderText="Action Request" DataField="ActionRequest" />
                                    <asp:BoundField HeaderText="Job Date" DataField="ScheduleDate" DataFormatString="{0:dd-MMM-yy}" />
                                    <asp:BoundField HeaderText="Start Time" DataField="StartTime" DataFormatString="{0:t}" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField HeaderText="End Time" DataField="EndTime" DataFormatString="{0:t}" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField HeaderText="Description of Work" DataField="jobtype" ReadOnly="True"></asp:BoundField>
                                    <asp:BoundField HeaderText="Customer" DataField="customer"></asp:BoundField>
                                    <asp:BoundField HeaderText="Remarks" DataField="Remarks"></asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <table style="padding:3px 3px 3px 3px;border-style:none"><tr>
                                            <td><asp:Button ID="gridLeftBtnAccept" runat="server" CssClass="buttongreen" Text="Accept" CommandName="AcceptRequest" CommandArgument="<%#((GridViewRow) Container).RowIndex%>" /></td>
                                            <td><asp:Button ID="gridLeftBtnReject" runat="server" CssClass="buttonred" Text="Reject" OnClientClick="if(!confirm('Are you sure you want to reject this request?')) return false;" CommandName="RejectRequest" CommandArgument="<%#((GridViewRow) Container).RowIndex%>" />
                                                </td></tr></table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView></td></tr>
                                <tr><td style="text-align:center"><asp:Button ID="gridLeftBtnAcceptAll" runat="server" CssClass="buttongreen" Text="Accept" OnClick="gridLeftBtnAcceptAll_Click" />
                                    <asp:Button ID="gridLeftBtnRejectAll" runat="server" CssClass="buttonred" Text="Reject"  OnClientClick="if(!confirm('Are you sure you want to reject all selected request?')) return false;" OnClick="gridLeftBtnRejectAll_Click"/>
                                    </td>

                                </tr>
                                </table>
                        </asp:Panel>
                    </ContentTemplate>
                </ajaxToolKit:TabPanel>
                <ajaxToolKit:TabPanel ID="tabPanelLeft2" runat="server" HeaderText="Pending Request">
                    <ContentTemplate>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="300px">
                            <asp:GridView ID="gridViewLeftPanel2" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                <EmptyDataRowStyle />
                                <EmptyDataTemplate>
                                    No Data Found.
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:BoundField DataField="Id" Visible="false" />
                                    <asp:BoundField HeaderText="Action Request" DataField="ActionRequest" />
                                    <asp:BoundField HeaderText="Job Date" DataField="ScheduleDate" DataFormatString="{0:dd-MMM-yy}" />
                                    <asp:BoundField HeaderText="Start Time" DataField="StartTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="End Time" DataField="EndTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Description of Work" DataField="jobtype" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Customer" DataField="customer" ReadOnly="false"></asp:BoundField>
                                    <asp:BoundField HeaderText="Remarks" DataField="Remarks"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </ajaxToolKit:TabPanel>
                <ajaxToolKit:TabPanel ID="tabPanelLeft3" runat="server" HeaderText="Rejected Request">
                    <ContentTemplate>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="300px">
                            <asp:GridView ID="gridViewLeftPanel3" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                <EmptyDataRowStyle />
                                <EmptyDataTemplate>
                                    No Data Found.
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:BoundField DataField="Id" Visible="false" />
                                    <asp:BoundField HeaderText="Supervisor" DataField="fullname"/>
                                    <asp:BoundField HeaderText="Action Request" DataField="ActionRequest" />
                                    <asp:BoundField HeaderText="Job Date" DataField="ScheduleDate" DataFormatString="{0:dd-MMM-yy}" />
                                    <asp:BoundField HeaderText="Start Time" DataField="StartTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="End Time" DataField="EndTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Description of Work" DataField="jobtype" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Customer" DataField="customer" ReadOnly="false"></asp:BoundField>
                                    <asp:BoundField HeaderText="Supervisor Remarks" DataField="SupervisorRemarks"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </ajaxToolKit:TabPanel>
            </ajaxToolKit:TabContainer>
        </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div style="height:70%; min-height:300px; width:35%; float:right;">
        <asp:UpdatePanel ID="UpdatePanelRight" runat="server">   
       <ContentTemplate>
       <asp:Panel ID="panelRightHeader" runat="server" CssClass="modalAlertHeader" Width="98%">Graph</asp:Panel>
           <asp:Panel ID="panelRightContent" runat="server" CssClass="modalRight" Width="98%">
        </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div style="height:30%; width:100%;min-height:200px; clear:both">
        <asp:UpdatePanel ID="UpdatePanelBottom" runat="server">   
       <ContentTemplate>
       <asp:Panel ID="panelBottomHeader" runat="server" CssClass="modalAlertHeader2" Width="99%">Analysis</asp:Panel>
           <asp:Panel ID="panelBottomContent" runat="server" CssClass="modalBottom" Width="99%">
               <div>
               <table style="width:100%;display:table">
                   <tr><td>Department : <asp:DropDownList ID="ddlBottomDepartment" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBottomDepartment_Changed"></asp:DropDownList></td>
                       <%--<td>Job Type : <asp:DropDownList ID="ddlBottomJobType"   runat="server"></asp:DropDownList></td>--%>
                       <td>Schedule Date : <asp:TextBox ID="txtBoxBottomFromDate" runat="server" AutoPostBack="true" OnTextChanged="txtBoxBottomFromDate_Changed">
                                           </asp:TextBox> to: <asp:TextBox ID="txtBoxBottomToDate" runat="server" AutoPostBack="true" OnTextChanged="txtBoxBottomToDate_Changed"></asp:TextBox></td>
                   </tr>
                   <tr>
                       <td>Personel : <asp:DropDownList ID="ddlBottomPersonel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBottomPersonel_Changed"></asp:DropDownList></td>
                       <td>Job ID : <asp:TextBox ID="txtBoxBottomJobId" runat="server" AutoPostBack="true" OnTextChanged="txtBoxBottomJobId_Changed"></asp:TextBox></td>
                       <td style="text-align:left">Customer : <asp:TextBox ID="txtBoxBottomCustomer" runat="server" AutoPostBack="true" OnTextChanged="txtBoxBottomCustomer_Changed"></asp:TextBox></td>
                   </tr>
                </table>
                   </div>
                <ajaxToolKit:CalendarExtender ID="calExtBottomFromDate" runat="server" TargetControlID="txtBoxBottomFromDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                <ajaxToolKit:CalendarExtender ID="calExtBottomToDate" runat="server" TargetControlID="txtBoxBottomToDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                <ajaxToolKit:TabContainer ID="tabContainerBottom" runat="server">
                    <ajaxToolKit:TabPanel ID="tabPanelBottom1" runat="server" HeaderText="Time Analysis">   
                        <ContentTemplate> 
                            <asp:Panel runat="server">
                               <asp:GridView ID="gridViewBottom" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true" OnRowDataBound="gridViewBottom_RowDataBound">
                                   <EmptyDataRowStyle />
                                    <EmptyDataTemplate>
                                        No Data Found.
                                    </EmptyDataTemplate>
                                   <Columns>
                                       <asp:BoundField HeaderText="Description of Work" DataField="jobtype"/>
                                       <asp:TemplateField HeaderText="Approved Work Time">
                                           <ItemTemplate>
                                               <asp:LinkButton ID="linkBtnBottomApprovedWork" runat="server" Text='<%#Eval("totalworktime")%>' CommandArgument='<%#Eval("jobtypeid")+"|"+Eval("jobtype")+"|Approved"%>' OnCommand="linkButtonBottom_Command"></asp:LinkButton>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Waiting for Approval">
                                           <ItemTemplate>
                                               <asp:LinkButton ID="linkBtnBottomWaitingWork" runat="server" Text='<%#Eval("totalforapproval")%>' CommandArgument='<%#Eval("jobtypeid")+"|"+Eval("jobtype")+"|For Approval"%>' OnCommand="linkButtonBottom_Command"></asp:LinkButton>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <asp:TemplateField HeaderText="Total Job Time">
                                           <ItemTemplate>
                                               <asp:LinkButton ID="linkBtnBottomRejectedWork" runat="server" Text='<%#Eval("totaljobTime")%>' CommandArgument='<%#Eval("jobtypeid")+"|"+Eval("jobtype")+"|Approved|JobTime"%>' OnCommand="linkButtonBottom_Command"></asp:LinkButton>
                                           </ItemTemplate>
                                       </asp:TemplateField>
                                       <%--<asp:BoundField HeaderText="Number of Unclosed Task" DataField="totalunclosedjobs" />--%>
                                   </Columns>
                               </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxToolKit:TabPanel>
                    <ajaxToolKit:TabPanel ID="tabPanelBottom2" runat="server" HeaderText="Task Viewer">
                        <ContentTemplate>
                            <asp:GridView ID="gridViewBottom2" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:BoundField HeaderText="Department" DataField="department" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Personel" DataField="fullname" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Date" DataField="ScheduleDate" DataFormatString="{0:dd MMM yyyy}" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Start Time" DataField="StartTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="End Time" DataField="EndTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Description of Work" DataField="jobtype" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="JobId" DataField="JobIdNumber" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Customer" DataField="customer" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Time Consumed" DataField="totalhours" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Remarks" DataField="Remarks" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Task Status" DataField="JobStatus" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Entry Status" DataField="Status" ReadOnly="true"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </ajaxToolKit:TabPanel> 
                </ajaxToolKit:TabContainer>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
     <%--Modal--%>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
         <ContentTemplate>
            <asp:Panel Id="panelModal" runat="server" style="display:none">
                <asp:Panel ID="panelModalHeader" runat="server" CssClass="modalAlertHeader">Reject Request</asp:Panel>
                <asp:Panel ID="panelModalContent" runat="server" CssClass="modal">
                    <table style="padding:3px;border-spacing:10px;" border="0">
                        <tr><td><asp:Label ID="modalBottomLabelError" runat="server" CssClass="validation" Visible="false"/></td></tr>
                        <tr><td>Remarks : <asp:RequiredFieldValidator ID="modalReqRemarks" runat="server" ValidationGroup="modalLeft" CssClass="validation" ControlToValidate="modalTxtBoxRemarks" Text="Remarks is required."/></td></tr>
                        <tr><td><asp:TextBox ID="modalTxtBoxRemarks" runat="server" TextMode="MultiLine" ValidationGroup="modalLeft" Rows="5" Width="300px"></asp:TextBox></td></tr>
                        <tr><td style="text-align:center">
                            <asp:Button ID="modalBtnConfirm" runat="server" CssClass="button" ValidationGroup="modalLeft" CausesValidation="true" Text="Confirm" OnCommand="modalBtnConfirm_Command" />
                            <asp:Button ID="modalBtnCancel" runat="server" CssClass="button" ValidationGroup="modalLeft" CausesValidation="false" Text="Cancel" />
                            </td></tr>
                    </table>
                </asp:Panel>
            </asp:Panel>

             <asp:Panel ID="panelModalBottom" runat="server" stye="display:none">
                 <asp:Panel ID="panelModalHeaderBottom" runat="server" CssClass="modalHeader"><asp:Label ID="labelmodalBottom" runat="server"></asp:Label></asp:Panel>
                 <asp:Panel ID="panelModalContentBottom" runat="server" CssClass="modal" ScrollBars="Auto" style ="max-height:500px">
                     <table>
                         <tr><td>
                            <asp:GridView ID="gridViewModalBottom" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true">
                                <EmptyDataRowStyle/>
					            <EmptyDataTemplate>
					                No Data Found.
					            </EmptyDataTemplate>
                                <Columns>
                                    <asp:BoundField HeaderText="Department" DataField="department" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Personel" DataField="fullname" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Date" DataField="ScheduleDate" DataFormatString="{0:dd MMM yyyy}" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Start Time" DataField="StartTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="End Time" DataField="EndTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Hours in Words" DataField="totalhours" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="JobId" DataField="JobIdNumber" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Customer" DataField="customer" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Remarks" DataField="Remarks" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Task Status" DataField="JobStatus" ReadOnly="true"></asp:BoundField>
                                </Columns>
                            </asp:GridView>
                        </td></tr>
                         <tr><td style="text-align:center">
                            <asp:Button ID="btnDoneBottom" runat="server" CssClass="button" Text="Done"/>
                            </td></tr>
                    </table>
                 </asp:Panel>
             </asp:Panel>

            <asp:Button ID="btnHidden" runat="server" style="display:none"/>
             <asp:Button ID="btnHiddenBottom" runat="server" style="display:none"/>
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

         <ajaxToolKit:ModalPopupExtender runat="server" ID="programmaticModalPopupBottom"
                BehaviorID ="programmaticModalPopupBehaviorBottom"
                TargetControlID="btnHiddenBottom"
                PopupControlID="panelModalBottom"
                BackgroundCssClass="modalBackground"
                CancelControlID="btnDoneBottom"
                DropShadow="false"
                PopupDragHandleControlID="panelModalBottom"
                RepositionMode="RepositionOnWindowResize" >    
            </ajaxToolKit:ModalPopupExtender> 

         </ContentTemplate>
     </asp:UpdatePanel>
</asp:Content>
