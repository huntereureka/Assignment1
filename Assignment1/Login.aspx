<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Assignment1.Login" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SITConnect - Login</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6LceN0caAAAAAKawSUyFpUmoiBf5IVYpDTrA7A8B">
    </script>
    <style type="text/css">
        .auto-style2 {
            width: 77px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend>SITConnect - Login</legend>
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style2">Email: </td>
                    <td><asp:TextBox ID="tb_userid" runat="server" placeholder="Email"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="auto-style2">Password: </td>
                    <td><asp:TextBox ID="tb_password" runat="server" placeholder="Password"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
                    </td>
                </tr>
                <tr>
                    <td><asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click"/></td>
                    <td><asp:Label ID="lbl_error" runat="server" Text="Label" Visible="false"></asp:Label></td>
                    <td><asp:Label ID="lbl_gScore" runat="server" Text="Label" Visible="false"></asp:Label></td>
                </tr>
            </table>
        </fieldset>
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LceN0caAAAAAKawSUyFpUmoiBf5IVYpDTrA7A8B', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html> 
