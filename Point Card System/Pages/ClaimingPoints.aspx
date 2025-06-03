<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimingPoints.aspx.cs" Inherits="Point_Card_System.Pages.ClaimingPoints" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/ClaimingPoints.css") %>' />

    <div class="container">
        <h1 class="page-title">Claim Points</h1>

        <div class="content-box">

            <div class="form-group">
                <label for="txtPhoneNo" class="form-label">Phone No</label>
                <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control" Placeholder="Enter Phone No" OnTextChanged="txtPhoneNo_TextChanged"></asp:TextBox>
            </div>

            <div class="form-group">
    <label for="txtCustomerName" class="form-label">Phone No</label>
    <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control text-uppercase" Placeholder="Customer Name" readonly></asp:TextBox>
</div>

            <div class="form-group">
                <label for="txtAvailablePoints" class="form-label">Available Points</label>
                <asp:TextBox ID="txtAvailablePoints" runat="server" CssClass="form-control" Placeholder="Available Points" Enabled="False"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtInvoiceNum" class="form-label">Invoice Number</label>
                <asp:TextBox ID="txtInvoiceNum" runat="server" CssClass="form-control text-uppercase" Placeholder="Enter Invoice Number" OnTextChanged="txtInvoiceNum_TextChanged"></asp:TextBox>
            </div>

            <div class="form-group">
                <label for="txtClaimingPoints" class="form-label">Claiming Points</label>
                <asp:TextBox ID="txtClaimingPoints" runat="server" CssClass="form-control" Placeholder="Claiming Points" OnTextChanged="txtClaimingPoints_TextChanged"></asp:TextBox>
            </div>

            <div class="buttons-container">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-cancel" Text="Cancel" />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-save" Text="Save" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>

</asp:Content>