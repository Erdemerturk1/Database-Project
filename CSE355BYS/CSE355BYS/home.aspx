<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs"
    Inherits="CSE355.HomePage" MasterPageFile="~/template.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <h2>FurnicolorDB - Home</h2>
    <p>Use the buttons below.</p>

    <asp:Button ID="btnProducts" runat="server" Text="Products" PostBackUrl="~/product.aspx" />
    &nbsp;
    <asp:Button ID="btnSales" runat="server" Text="Sales" PostBackUrl="~/sales.aspx" />
    &nbsp;
    <asp:Button ID="btnPayments" runat="server" Text="Payments" PostBackUrl="~/payment.aspx" />
    &nbsp;
    <asp:Button ID="btnReturns" runat="server" Text="Returns" PostBackUrl="~/return.aspx" />
</asp:Content>
