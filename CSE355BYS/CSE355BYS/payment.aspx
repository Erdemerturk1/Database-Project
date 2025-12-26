<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payment.aspx.cs"
    Inherits="CSE355BYS.PaymentPage" MasterPageFile="~/template.Master" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <h2>Payments</h2>

    <h3>Record Customer Payment</h3>

    <p>
        SalesInvID (optional):
        <asp:TextBox ID="txtInvID" runat="server" />
    </p>

    <p>
        Customer (used if SalesInvID empty):
        <asp:DropDownList ID="ddlCustomerPay" runat="server" />
    </p>

    <p>
        Payment Currency:
        <asp:DropDownList ID="ddlPayCurrency" runat="server" />
    </p>

    <p>
        Rate Date (yyyy-MM-dd):
        <asp:TextBox ID="txtRateDate" runat="server" />
    </p>

    <p>
        AmountForeign:
        <asp:TextBox ID="txtAmountForeign" runat="server" />
    </p>

    <asp:Button ID="btnPay" runat="server" Text="Pay" OnClick="btnPay_Click" />
    <br /><br />
    <asp:Label ID="lblPayMsg" runat="server" />

    <hr />

    <h3>Query Invoice Status</h3>
    <p>
        SalesInvID:
        <asp:TextBox ID="txtQueryID" runat="server" />
        <asp:Button ID="btnQuery" runat="server" Text="Query" OnClick="btnQuery_Click" />
    </p>

    <asp:GridView ID="gvStatus" runat="server" AutoGenerateColumns="True" />
</asp:Content>
    