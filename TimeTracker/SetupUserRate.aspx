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

                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
