<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCustomerPoints.aspx.cs" Inherits="Point_Card_System.Pages.AddCustomerPoints1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/AddCustomerPoints.css") %>' />

    <div class="container">
        <h1 class="page-title">Add Customer Points</h1>

        <div class="content-box">
            <div class="form-group">
                <label for="txtCustomerPhoneNo" class="form-label">Customer Phone No</label>
                <asp:TextBox ID="txtCustomerPhoneNo" runat="server" CssClass="form-control" Placeholder="Enter Customer Phone No" AutoPostBack="true" OnTextChanged="txtCustomerPhoneNo_TextChanged" />
            </div>

            <div class="form-group">
                <label for="txtCustomerName" class="form-label">Customer Name</label>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control text-uppercase" Placeholder="Customer Name" readonly/>
            </div>

            <div class="form-group">
                <label for="txtInvoiceNo" class="form-label">Invoice No</label>
                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control text-uppercase" Placeholder="Enter Invoice No" />
            </div>

            <div class="form-group">
                <label for="txtInvoiceTotal" class="form-label">Invoice Total</label>
                <asp:TextBox ID="txtInvoiceTotal" runat="server" CssClass="form-control" Placeholder="Enter Invoice Total" AutoPostBack="true" OnTextChanged="txtInvoiceTotal_TextChanged" />
            </div>

            <div class="form-group">
                <label for="txtPoints" class="form-label">Points</label>
                <asp:TextBox ID="txtPoints" runat="server" CssClass="form-control" Placeholder="Points" readonly/>
            </div>

            <div class="form-group">
                <div class="radio-group">
                    <div class="radio-option">
                        <asp:RadioButton ID="rbVatCustomer" runat="server" GroupName="CustomerType" CssClass="form-check-input" />
                        <label for="rbVatCustomer" class="form-check-label">VAT Customer</label>
                    </div>
                    <div class="radio-option">
                        <asp:RadioButton ID="rbNonVatCustomer" runat="server" GroupName="CustomerType" CssClass="form-check-input" />
                        <label for="rbNonVatCustomer" class="form-check-label">Non VAT Customer</label>
                    </div>
                </div>
            </div>

            <div class="buttons-container">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-cancel" />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>
</asp:Content>

