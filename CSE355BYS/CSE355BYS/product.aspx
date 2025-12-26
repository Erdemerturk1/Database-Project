<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="product.aspx.cs"
    Inherits="CSE355BYS.ProductPage" MasterPageFile="~/template.Master" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <h2>Products</h2>

    <asp:Button ID="btnQuery" runat="server" Text="Query Products" OnClick="btnQuery_Click" />
    <br /><br />

    <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="True" />
    <br />
    <asp:Label ID="lblMsg" runat="server" />

    <hr />
    <h3>Insert Product</h3>

    <p>
        Category:
        <asp:DropDownList ID="ddlCategory" runat="server" />
    </p>

    <p>
        Name:
        <asp:TextBox ID="txtName" runat="server" />
    </p>

    <p>
        Unit:
        <asp:DropDownList ID="ddlUnit" runat="server">
            <asp:ListItem Text="Piece" Value="Piece" />
            <asp:ListItem Text="Kg" Value="Kg" />
            <asp:ListItem Text="Litre" Value="Litre" />
        </asp:DropDownList>
    </p>

    <p>
        Base Currency:
        <asp:DropDownList ID="ddlCurrency" runat="server" />
    </p>

    <p>
        Base Price:
        <asp:TextBox ID="txtBasePrice" runat="server" />
    </p>

    <p>
        VAT Rate:
        <asp:DropDownList ID="ddlVat" runat="server">
            <asp:ListItem Text="0" Value="0" />
            <asp:ListItem Text="1" Value="1" />
            <asp:ListItem Text="8" Value="8" />
            <asp:ListItem Text="10" Value="10" />
            <asp:ListItem Text="18" Value="18" />
            <asp:ListItem Text="20" Value="20" />
        </asp:DropDownList>
    </p>

    <p>
        Initial Stock:
        <asp:TextBox ID="txtStock" runat="server" Text="0" />
    </p>

    <asp:Button ID="btnAdd" runat="server" Text="Insert" OnClick="btnAdd_Click" />
</asp:Content>
