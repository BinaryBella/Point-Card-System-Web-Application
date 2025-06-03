<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerRegistration.aspx.cs" Inherits="Point_Card_System.Pages.CustomerRegistration1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/CustomerRegistration.css") %>' />
    <div class="container">
        <h1 class="registration-title">Customer Registration</h1>

            <div class="row">
                <!-- Customer Details Section -->
                <div class="col-md-6">
                    <h2 class="section-title">Customer Details</h2>
                    <div class="content-box">
                         <asp:HiddenField ID="hfCustomerId" runat="server" />
                        <div class="form-group">
                            <label for="txtCustomerID" class="form-label">Customer ID</label>
                            <asp:TextBox ID="txtCustomerID" runat="server" class="form-control" placeholder="Enter Customer ID"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="txtCustomerName" class="form-label">Customer Name</label>
                            <asp:TextBox ID="txtCustomerName" runat="server" class="form-control text-uppercase" placeholder="Enter Customer Name" OnTextChanged="txtCustomerName_TextChanged"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="txtNIC" class="form-label">NIC</label>
                            <asp:TextBox ID="txtNIC" runat="server" class="form-control" placeholder="Enter NIC" OnTextChanged="txtNIC_TextChanged"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="txtPhoneNo" class="form-label">Phone No</label>
                            <asp:TextBox ID="txtPhoneNo" runat="server" class="form-control" placeholder="Enter Phone No" OnTextChanged="txtPhoneNo_TextChanged"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="txtAddress" class="form-label">Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" class="form-control text-uppercase" placeholder="Enter Address" OnTextChanged="txtAddress_TextChanged"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <div class="radio-group">
                                <div class="radio-option">
                                    <asp:RadioButton ID="rbVatCustomer" runat="server" class="form-control" name="CustomerType" ></asp:RadioButton>
                                    <label for="rbVatCustomer" class="form-check-label">VAT Customer</label>
                                </div>
                                <div class="radio-option">
                                    <asp:RadioButton ID="rbNonVatCustomer" runat="server" class="form-control" name="CustomerType" ></asp:RadioButton>
                                    <label for="rbNonVatCustomer" class="form-check-label">Non VAT Customer</label>
                                </div>
                            </div>
                        </div>

                        <div class="buttons-container">
                            <asp:Button runat="server" class="btn btn-cancel" Text="Cancel" ID="btn_cancel" />
                            <asp:Button runat="server" class="btn btn-save" Text="Save" ID="btn_save" OnClick="btn_save_Click" />
                        </div>
                    </div>
                </div>

                <!-- Customer Points Details Section -->
                <div class="col-md-6">
                    <h2 class="section-title">Customer Loyality Card</h2>
                    <div class="content-box">
                        <div class="form-group">
                            <label for="txtTotalPoints" class="form-label">Card Number</label>
                            <asp:TextBox ID="txtCardNum" runat="server" class="form-control" value="" ></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="txtClammingPoints" class="form-label">Clamming Points</label>
                            <asp:TextBox ID="txtClammingPoints" runat="server" class="form-control" value="" readonly ></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="txtRegisteredDate" class="form-label">Customer Registered Date</label>
                            <input type="date" id="txtRegisteredDate" class="form-control" value="2024-09-09" readonly/>
                        </div>

                        <div class="form-group">
                            <label for="txtRegisteredBranch" class="form-label">Customer Registered Branch</label>
                            <asp:TextBox ID="txtRegisteredBranch" runat="server" class="form-control" value="" readonly ></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</asp:Content>