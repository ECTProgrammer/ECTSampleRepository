<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Job Overview.aspx.cs" Inherits="TimeTracker.Job_Overview" %>
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
                    <div style="text-align:right">Select Date : <asp:TextBox ID="txtBoxDate"  runat="server" AutoPostBack="true" ></asp:TextBox></div>
                    <table style="width:100%;">
                        <tr><td style="text-align:center"><h2><asp:Label ID="labelDay" runat="server"></asp:Label></h2></td></tr>
                    </table>
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderDate" runat="server" TargetControlID="txtBoxDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                    <table style="width:100%;">
                        <tr><td><asp:LinkButton ID="linkBtnAddJobTrack" runat="server" CausesValidation="false" Text="Add Job" CssClass="linkButton" Font-Bold="true"></asp:LinkButton> 
                        </td><td style="text-align:right"><asp:Label ID="LabelTotalHours" ForeColor="#ff0000" Font-Size="12px" runat="server" Font-Bold="true"></asp:Label></td></tr></table>   
                    <div style="margin:10px 10px 10px 10px">
                        <asp:GridView ID="gridJobTrack" runat="server" AutoGenerateColumns="false" CssClass="gridView" GridLines="None" ShowHeaderWhenEmpty="true" OnRowCreated="gridViewJobTrack_RowCreated">
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
                                <asp:BoundField HeaderText="Customer" DataField="customer" ReadOnly="false"></asp:BoundField>
                                <asp:BoundField HeaderText="Number of Hours" DataField="totalhours" ReadOnly="false"></asp:BoundField>
                                <asp:BoundField HeaderText="Remarks" DataField="Remarks"></asp:BoundField>
                                <asp:BoundField HeaderText="Status" DataField="Status"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
