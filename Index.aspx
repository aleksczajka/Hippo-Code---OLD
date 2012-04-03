<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="rad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Aleks is Movin Peeps!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                
                </td>
                <td>
                    <rad:RadRotator runat="server" ScrollDuration="200" ID="Rotator1" 
                        ItemHeight="250" ItemWidth="400"  
                        Width="440px" Height="250px" RotatorType="Buttons">
                    </rad:RadRotator>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
