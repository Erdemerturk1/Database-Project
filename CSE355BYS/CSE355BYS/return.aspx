<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="return.aspx.cs"
    Inherits="CSE355BYS.ReturnPage" MasterPageFile="~/template.Master" %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <h2>Returns</h2>

    <h3>Create Sales Return</h3>
    <p>
        SalesInvID:
        <asp:TextBox ID="txtReturnSalesInvID" runat="server" />
    </p>
    <p>
        Reason:
        <asp:TextBox ID="txtReason" runat="server" Width="400px" />
    </p>

    <asp:Button ID="btnCreateReturn" runat="server" Text="Create Return" OnClick="btnCreateReturn_Click" />
    <br /><br />
    <asp:Label ID="lblReturnMsg" runat="server" />

    <hr />

    <h3>Add Return Item</h3>
    <p>
        SalesReturnID:
        <asp:TextBox ID="txtSalesReturnID" runat="server" />
    </p>
    <p>
        Product:
        <asp:DropDownList ID="ddlReturnProduct" runat="server" />
    </p>
    <p>
        Quantity:
        <asp:TextBox ID="txtReturnQty" runat="server" Text="1" />
    </p>

    <asp:Button ID="btnAddReturnItem" runat="server" Text="Add Return Item" OnClick="btnAddReturnItem_Click" />
    <br /><br />
    <asp:Label ID="lblReturnItemMsg" runat="server" />

    <hr />

    <h3>Query Return</h3>
    <p>
        SalesReturnID:
        <asp:TextBox ID="txtQueryReturnID" runat="server" />
        <asp:Button ID="btnQueryReturn" runat="server" Text="Query" OnClick="btnQueryReturn_Click" />
    </p>

    <h4>Return Header</h4>
    <asp:GridView ID="gvReturnHeader" runat="server" AutoGenerateColumns="True" />

    <h4>Return Items</h4>
    <asp:GridView ID="gvReturnItems" runat="server" AutoGenerateColumns="True" />

    <h4>Inventory Transactions (SALES_RETURN)</h4>
    <asp:GridView ID="gvReturnTx" runat="server" AutoGenerateColumns="True" />
</asp:Content>
