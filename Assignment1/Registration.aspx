<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Assignment1.Registration" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Page</title>
    <style type="text/css">
        .auto-style1 {
            width: 160px;
        }
        .auto-style2 {
            width: 160px;
            height: 26px;
        }
        .auto-style3 {
            height: 26px;
        }
    </style>
    <script type="text/javascript">
        function validatePassword() {
            var password = document.getElementById('<%=tb_password.ClientID%>').value;

            if (password.length < 8) {
                document.getElementById("lbl_password").innerHTML = "Password length must be at least 8 characters!";
                document.getElementById("lbl_password").style.color = "Red";
                return ("too_short");
            }

            else if (password.search(/[0-9]/) == -1) {
                document.getElementById("lbl_password").innerHTML = "Password requires at least 1 number!";
                document.getElementById("lbl_password").style.color = "Red";
                return ("no_number");
            }

            else if (password.search(/[A-Z]/) == -1) {
                document.getElementById("lbl_password").innerHTML = "Password requires at least 1 uppercase letter!";
                document.getElementById("lbl_password").style.color = "Red";
                return ("no_uppercase");
            }

            else if (password.search(/[a-z]/) == -1) {
                document.getElementById("lbl_password").innerHTML = "Password requires at least 1 lowercase letter!";
                document.getElementById("lbl_password").style.color = "Red";
                return ("no_alphabet");
            }

            else if (password.search(/[!@#$%^&*()<>,./?]/) == -1) {
                document.getElementById("lbl_password").innerHTML = "Password requires at least 1 special character!";
                document.getElementById("lbl_password").style.color = "Red";
                return ("no_special");
            }

            document.getElementById("lbl_password").innerHTML = "Excellent!";
            document.getElementById("lbl_password").style.color = "Blue";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
                <legend>SITConnect - Registration</legend>
                <table style="width: 100%;">
                    <tr>
                        <td class="auto-style1">First name:</td>
                        <td><asp:TextBox ID="tb_firstname" runat="server" placeholder="First name"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Last name:</td>
                        <td><asp:TextBox ID="tb_lastname" runat="server" placeholder="Last name"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="auto-style2">Credit card information:</td>
                        <td class="auto-style3"><asp:TextBox ID="tb_creditcard" runat="server" placeholder="Credit card number"></asp:TextBox></td>
                        <td class="auto-style3"><asp:Label ID="error_creditcard" runat="server" Text="Label" Visible="false"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Email address:</td>
                        <td><asp:TextBox ID="tb_email" runat="server" placeholder="Email" TextMode="Email"></asp:TextBox></td>
                        <td><asp:Label ID="error_email" runat="server" Text="Label" Visible="false"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Password:</td>
                        <td><asp:TextBox ID="tb_password" runat="server" placeholder="Password" onkeyup="javascript:validatePassword()" TextMode="Password"></asp:TextBox></td>
                        <td><asp:Label ID="lbl_password" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Date of birth:</td>
                        <td><asp:TextBox ID="tb_dob" runat="server" placeholder="DDMMYYYY"></asp:TextBox></td>
                        <td><asp:Label ID="error_dob" runat="server" Text="Label" Visible="false"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click"/>
                            <br />
                            <asp:Label ID="lbl_pwdchecker" runat="server" Text="Label" Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="checkpwd" runat="server" Text="Check Password" OnClick="checkpwd_Click" MaintainScrollPositionOnPostBack="true"/>
                        </td>
                        <td>
                            <asp:Label ID="lbl_error" runat="server" Text="Label" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </form>
</body>
</html>
