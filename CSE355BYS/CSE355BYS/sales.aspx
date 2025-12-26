<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sales.aspx.cs"
    Inherits="CSE355BYS.SalesPage" MasterPageFile="~/template.Master" %>



<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <h2>Sales</h2>

    <h3>Create Sales Invoice</h3>
    <p>
        Customer:
        <asp:DropDownList ID="ddlCustomer" runat="server" />
    </p>
    <p>
        Currency:
        <asp:DropDownList ID="ddlSalesCurrency" runat="server" />
    </p>
    <p>
        Invoice Date (yyyy-MM-dd HH:mm):
        <asp:TextBox ID="txtInvDate" runat="server" />
    </p>
    <p>
        Credit Sale?
        <asp:CheckBox ID="chkCredit" runat="server" />
    </p>
    <p>
        Due Date (optional, yyyy-MM-dd HH:mm):
        <asp:TextBox ID="txtDueDate" runat="server" />
    </p>

    <asp:Button ID="btnCreateInvoice" runat="server" Text="Create Invoice" OnClick="btnCreateInvoice_Click" />
    <br /><br />
    <asp:Label ID="lblCreateMsg" runat="server" />

    <hr />

    <h3>Add Item</h3>
    <p>
        SalesInvID:
        <asp:TextBox ID="txtSalesInvID" runat="server" />
    </p>
    <p>
        Product:
        <asp:DropDownList ID="ddlProduct" runat="server" />
    </p>
    <p>
        Quantity:
        <asp:TextBox ID="txtQty" runat="server" Text="1" />
    </p>
    <p>
        UnitPriceForeign:
        <asp:TextBox ID="txtUnitPriceForeign" runat="server" />
    </p>

    <asp:Button ID="btnAddItem" runat="server" Text="Add Item" OnClick="btnAddItem_Click" />
    <br /><br />
    <asp:Label ID="lblItemMsg" runat="server" />

    <hr />

    <h3>Query Invoice</h3>
    <p>
        SalesInvID:
        <asp:TextBox ID="txtQueryInvID" runat="server" />
        <asp:Button ID="btnQueryInv" runat="server" Text="Query" OnClick="btnQueryInv_Click" />
    </p>

    <h4>Invoice Header</h4>
    <asp:GridView ID="gvHeader" runat="server" AutoGenerateColumns="True" />

    <h4>Invoice Items</h4>
    <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="True" />

    <h4>Inventory Transactions (SALE)</h4>
    <asp:GridView ID="gvInvTx" runat="server" AutoGenerateColumns="True" />

    <br />
    <asp:Label ID="lblQueryMsg" runat="server" />
</asp:Content>
