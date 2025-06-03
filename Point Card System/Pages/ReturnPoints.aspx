<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReturnPoints.aspx.cs" Inherits="Point_Card_System.Pages.ReturnPoints1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/ReturnPoints.css") %>' />
    <div class="container">
        <h1 class="page-title">Return Points</h1>

        <div class="content-box">

            <div class="form-group">
                <label for="txtInvoiceNo" class="form-label">Invoice No</label>
                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" Placeholder="Enter Invoice No" OnTextChanged="txtInvoiceNo_TextChanged"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtCustomerName" class="form-label">Customer Name</label>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" Placeholder="Enter Customer Name"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtInvoiceAmount" class="form-label">Total Invoice Amount</label>
                <asp:TextBox ID="txtInvoiceAmount" runat="server" CssClass="form-control" Placeholder="Enter Invoice Amount"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtReturnAmount" class="form-label">Return Amount</label>
                <asp:TextBox ID="txtReturnAmount" runat="server" CssClass="form-control" Placeholder="Enter Return Amount" OnTextChanged="txtReturnAmount_TextChanged"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtAvailablePoints" class="form-label">Return Points Amount</label>
                <asp:TextBox ID="txtAvailablePoints" runat="server" CssClass="form-control" Placeholder="Return Points Amount"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtReturnInvNum" class="form-label">Return Invoice Number</label>
                <asp:TextBox ID="txtReturnInvNum" runat="server" CssClass="form-control" Placeholder="Return Invoice Number" OnTextChanged="txtReturnInvNum_TextChanged"></asp:TextBox>
            </div>

            <div class="buttons-container">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-cancel" Text="Cancel" />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-save" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
</asp:Content>