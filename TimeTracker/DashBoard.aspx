<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="TimeTracker.DashBoard" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:70%; width:50%; min-height:300px; float:left;"> 
    <asp:UpdatePanel ID="UpdatePanelLeft" runat="server">   
       <ContentTemplate>
       <asp:Panel ID="panelLeftHeader" runat="server" CssClass="modalHeader" Width="98%">Waiting for Approval</asp:Panel>
        <asp:Panel ID="panelLeftContent" runat="server" CssClass="modalLeft" Width="98%">
            <ajaxToolKit:TabContainer ID="tabContainerLeft" runat="server">
                <ajaxToolKit:TabPanel ID="tabPanelLeft1" runat="server" HeaderText="Waiting for your Approval">
                    <ContentTemplate>
                        <asp:Panel runat="server" ScrollBars="Auto" Height="300px">
                            <asp:GridView ID="gridViewLeftPanel1" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true" OnRowCommand="gridViewLeftPanel1_RowCommand">
                                <EmptyDataRowStyle />
                                <EmptyDataTemplate>
                                    No Data Found.
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:BoundField DataField="Id" Visible="false" />
                                    <asp:BoundField HeaderText="Requested By" DataField="fullname"/>
                                    <asp:BoundField HeaderText="Action Request" DataField="ActionRequest" />
                                    <asp:BoundField HeaderText="Job Date" DataField="ScheduleDate" DataFormatString="{0:dd-MMM-yy}" />
                                    <asp:BoundField HeaderText="Start Time" DataField="StartTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="End Time" DataField="EndTime" DataFormatString="{0:t}" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Description of Work" DataField="jobtype" ReadOnly="true"></asp:BoundField>
                                    <asp:BoundField HeaderText="Customer" DataField="customer" ReadOnly="false"></asp:BoundField>
                                    <asp:BoundField HeaderText="Remarks" DataField="Remarks"></asp:BoundField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <table style="padding:3px 3px 3px 3px;border-style:none"><tr>
                                            <td><asp:Button ID="gridLeftBtnAccept" runat="server" CssClass="buttongreen" Text="Accept" CommandName="AcceptRequest" CommandArgument="<%#((GridViewRow) Container).RowIndex%>" /></td>
                                            <td><asp:Button ID="gridLeftBtnReject" runat="server" CssClass="buttonred" Text="Reject" OnClientClick="if(!confirm('Are you sure you want to reject this request?')) return false;" CommandName="RejectRequest" CommandArgument="<%#((GridViewRow) Container).RowIndex%>" />
                                                </td></tr></table>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </ContentTemplate>
                </ajaxToolKit:TabPanel>
                <ajaxToolKit:TabPanel ID="tabPanelLeft2" runat="server" HeaderText="Pending Request">
                    <ContentTemplate>
                    </ContentTemplate>
                </ajaxToolKit:TabPanel>
                <ajaxToolKit:TabPanel ID="tabPanelLeft3" runat="server" HeaderText="Rejected Request">
                    <ContentTemplate>
                    </ContentTemplate>
                </ajaxToolKit:TabPanel>
            </ajaxToolKit:TabContainer>
        </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div style="height:70%; min-height:300px; width:49.999%; float:right;">
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

        </asp:Panel>
       </ContentTemplate>
    </asp:UpdatePanel>
    </div>
</asp:Content>
