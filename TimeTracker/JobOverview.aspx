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
           
            <asp:Panel ID="panelHeader" runat="server" CssClass="modalHeader">Job Tracker</asp:Panel>
            <asp:Panel ID="panelContent" runat="server" CssClass="modal">
                <asp:Label ID="labelAccessDenied" runat="server" Text="You do not have access rights to this page. Please contact your administrator if you need access. Thank you." Visible="false" CssClass="validation"></asp:Label>
                <asp:Panel ID="panelAccessOK" runat="server">
                    <div style="text-align:right">
                        <table style="margin:4px 4px 4px 4px; text-align:right">
                            <tr>
                                <td>Select Date :</td>
                                <td><asp:TextBox ID="txtBoxStartDate"  runat="server" AutoPostBack="true" OnTextChanged="txtBoxStartDate_Changed" ></asp:TextBox></td>
                                <td><asp:TextBox ID="txtBoxEndDate"  runat="server" AutoPostBack="true" OnTextChanged="txtBoxEndDate_Changed"></asp:TextBox></td>
                            </tr>
                        </table>
                    </div>
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderStartDate" runat="server" TargetControlID="txtBoxStartDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender> 
                    <ajaxToolKit:CalendarExtender ID="calendarExtenderEndDate" runat="server" TargetControlID="txtBoxEndDate" Format="dd MMM yyyy"></ajaxToolKit:CalendarExtender>
                    <asp:Panel runat="server" ScrollBars="Auto" Width="100%">
                        <div style="margin:10px 10px 10px 10px">
                            <asp:GridView ID="gridViewMain" runat="server" AutoGenerateColumns="true" CssClass="gridView" ShowHeaderWhenEmpty="true" OnRowCreated="gridViewMain_RowCreated">
                                <EmptyDataRowStyle/>
                                    <EmptyDataTemplate>
                                        No Data Found.
                                    </EmptyDataTemplate>
                                <Columns>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

