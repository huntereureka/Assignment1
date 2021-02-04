<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="Assignment1.Homepage" ValidateRequest="false"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page</title>
        <script>
        function startTimer(duration, display) {
            var timer = duration, minutes, seconds;
            setInterval(function () {
                minutes = parseInt(timer / 60, 10);
                seconds = parseInt(timer % 60, 10);

                minutes = minutes < 10 ? "0" + minutes : minutes;
                seconds = seconds < 10 ? "0" + seconds : seconds;

                display.textContent = minutes + ":" + seconds;

                if (--timer < 0) {
                    location.reload();
                }
            }, 1000);
        }

        window.onload = function () {
            var oneMinute = 60 * 1,
                display = document.querySelector('#time');
            startTimer(oneMinute, display);
        };
        </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <fieldset>
            <legend>SITConnect - Home</legend>
                <table style="width: 100%;">
                    <tr>
                        <td><asp:Label ID="lblMessage" runat="server"></asp:Label></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td><div>You will be logged out in <span id="time">01:00</span> minute!</div></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btn_logout" runat="server" Text="Log out" Height="26px" OnClick="btn_logout_Click" /></td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
        </fieldset>
        </div>
    </form>
</body>
</html>
