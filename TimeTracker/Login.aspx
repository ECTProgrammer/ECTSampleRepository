<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TimeTracker.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Everett Charles Technologies</title>
    <link href="~/logo.ico" rel="shortcut icon" type="image/x-icon" />
    <link rel="stylesheet" href="~/Content/master.css" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="frame">
            <table style="padding:0; border-spacing:0; width:100%" border="0">
                <tr><td style="width:100px; height:40px;"><img src="Images/ect_logo.gif" /></td><td><h1>Time Tracker</h1></td></tr>
            </table>
            <div style="height:28px;margin:30px 0px 0px 0px;"></div>
        </div>
        <div style="border-top:solid 1px #dedede; padding:0px 0px 0px 0px;"></div>
        <div class="frame">
            <br /><br /><br />
            <asp:Panel runat="server" DefaultButton="btnLogin" HorizontalAlign="Center" >
            <table class="table" style="margin-left:auto; margin-right:auto;" border="0">
                <tr><td colspan="3"  style="text-align:center">
                    <asp:Label ID="labelError" runat="server" Text="" Visible="false" CssClass="validation"></asp:Label>
                    </td></tr>
                <tr>
                    <td>Username</td><td>:</td>
                    <td>
                        <asp:TextBox ID="txtBoxUsername" runat="server" CssClass="textBox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBoxUsername" CssClass="validation" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Password</td><td>:</td>
                    <td>
                        <asp:TextBox ID="txtBoxPassword" runat="server"  TextMode="Password" CssClass="textBox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBoxPassword" CssClass="validation" ErrorMessage="*"></asp:RequiredFieldValidator>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="text-align:center">
                        <asp:Button ID="btnLogin" runat="server" CausesValidation="true"  Text="Log In" CssClass="button" OnClick="BtnLogin_Click" />
                    </td>
               </tr>
                
                
            </table>
                </asp:Panel>
    </div>
    </form>
</body>
</html>
