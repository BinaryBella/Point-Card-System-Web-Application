<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddCustomerPoints.aspx.cs" Inherits="Point_Card_System.Pages.AddCustomerPoints1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/AddCustomerPoints.css") %>' />
    
    <!-- SweetAlert2 CSS -->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    
    <div class="container">
        <h1 class="page-title">Add Customer Points</h1>
        <div class="content-box">
            <div class="form-group">
                <label for="txtCustomerPhoneNo" class="form-label">Customer Phone No</label>
                <asp:TextBox ID="txtCustomerPhoneNo" runat="server" CssClass="form-control" Placeholder="Enter Customer Phone No" AutoPostBack="true" OnTextChanged="txtCustomerPhoneNo_TextChanged" />
                <div id="errorCustomerPhoneNo" class="error-message">Customer Phone No is required</div>
            </div>
            <div class="form-group">
                <label for="txtCustomerName" class="form-label">Customer Name</label>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control text-uppercase" Placeholder="Customer Name" readonly/>
                <div id="errorCustomerName" class="error-message">Customer Name is required</div>
            </div>
            <div class="form-group">
                <label for="txtInvoiceNo" class="form-label">Invoice No</label>
                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control text-uppercase" Placeholder="Enter Invoice No" />
                <div id="errorInvoiceNo" class="error-message">Invoice No is required</div>
            </div>
            <div class="form-group">
                <label for="txtInvoiceTotal" class="form-label">Invoice Total</label>
                <asp:TextBox ID="txtInvoiceTotal" runat="server" CssClass="form-control" Placeholder="Enter Invoice Total" AutoPostBack="true" OnTextChanged="txtInvoiceTotal_TextChanged" />
                <div id="errorInvoiceTotal" class="error-message">Invoice Total is required</div>
            </div>
            <div class="form-group">
                <label for="txtPoints" class="form-label">Points</label>
                <asp:TextBox ID="txtPoints" runat="server" CssClass="form-control" Placeholder="Points" readonly/>
                <div id="errorPoints" class="error-message">Points is required</div>
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
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnSave_Click" OnClientClick="return validateForm();" />
            </div>
        </div>
    </div>

    <!-- SweetAlert2 JS -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

    <script type="text/javascript">
        function validateForm() {
            var isValid = true;

            // Clear previous error states
            clearErrorStates();

            // Get form elements
            var customerPhoneNo = document.getElementById('<%= txtCustomerPhoneNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceNo = document.getElementById('<%= txtInvoiceNo.ClientID %>');
            var invoiceTotal = document.getElementById('<%= txtInvoiceTotal.ClientID %>');
            var points = document.getElementById('<%= txtPoints.ClientID %>');
            
            // Validate Customer Phone No
            if (customerPhoneNo.value.trim() === '') {
                showError('errorCustomerPhoneNo', customerPhoneNo);
                isValid = false;
            }
            
            // Validate Customer Name
            if (customerName.value.trim() === '') {
                showError('errorCustomerName', customerName);
                isValid = false;
            }
            
            // Validate Invoice No
            if (invoiceNo.value.trim() === '') {
                showError('errorInvoiceNo', invoiceNo);
                isValid = false;
            }
            
            // Validate Invoice Total
            if (invoiceTotal.value.trim() === '') {
                showError('errorInvoiceTotal', invoiceTotal);
                isValid = false;
            }
            
            // Validate Points
            if (points.value.trim() === '') {
                showError('errorPoints', points);
                isValid = false;
            }
            
            return isValid;
        }
        
        function showError(errorId, inputElement) {
            var errorElement = document.getElementById(errorId);
            if (errorElement) {
                errorElement.style.display = 'block';
            }
            if (inputElement) {
                inputElement.classList.add('error');
            }
        }
        
        function clearErrorStates() {
            // Hide all error messages
            var errorMessages = document.querySelectorAll('.error-message');
            errorMessages.forEach(function(element) {
                element.style.display = 'none';
            });
            
            // Remove error class from all form controls
            var formControls = document.querySelectorAll('.form-control');
            formControls.forEach(function(element) {
                element.classList.remove('error');
            });
        }
        
        // Clear error state when user starts typing in a field
        function clearFieldError(inputElement, errorId) {
            inputElement.classList.remove('error');
            var errorElement = document.getElementById(errorId);
            if (errorElement) {
                errorElement.style.display = 'none';
            }
        }

        // SweetAlert2 Success Message
        function showSuccessAlert(message) {
            Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: message,
                confirmButtonText: 'OK',
                confirmButtonColor: '#28a745'
            });
        }

        // SweetAlert2 Error Message
        function showErrorAlert(message) {
            Swal.fire({
                icon: 'error',
                title: 'Error!',
                text: message,
                confirmButtonText: 'OK',
                confirmButtonColor: '#dc3545'
            });
        }

        // SweetAlert2 Warning Message
        function showWarningAlert(message) {
            Swal.fire({
                icon: 'warning',
                title: 'Warning!',
                text: message,
                confirmButtonText: 'OK',
                confirmButtonColor: '#ffc107'
            });
        }
        
        // Add event listeners to clear errors when user starts typing
        document.addEventListener('DOMContentLoaded', function() {
            var customerPhoneNo = document.getElementById('<%= txtCustomerPhoneNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceNo = document.getElementById('<%= txtInvoiceNo.ClientID %>');
            var invoiceTotal = document.getElementById('<%= txtInvoiceTotal.ClientID %>');
            var points = document.getElementById('<%= txtPoints.ClientID %>');

            if (customerPhoneNo) {
                customerPhoneNo.addEventListener('input', function () {
                    clearFieldError(this, 'errorCustomerPhoneNo');
                });
            }

            if (customerName) {
                customerName.addEventListener('input', function () {
                    clearFieldError(this, 'errorCustomerName');
                });
            }

            if (invoiceNo) {
                invoiceNo.addEventListener('input', function () {
                    clearFieldError(this, 'errorInvoiceNo');
                });
            }

            if (invoiceTotal) {
                invoiceTotal.addEventListener('input', function () {
                    clearFieldError(this, 'errorInvoiceTotal');
                });
            }

            if (points) {
                points.addEventListener('input', function () {
                    clearFieldError(this, 'errorPoints');
                });
            }

            // Set focus to Invoice Total when Enter is pressed in Invoice No field
            if (invoiceNo) {
                invoiceNo.addEventListener('keypress', function (e) {
                    if (e.key === 'Enter' || e.keyCode === 13) {
                        e.preventDefault(); // Prevent form submission
                        if (invoiceTotal) {
                            invoiceTotal.focus();
                        }
                    }
                });
            }
        });
    </script>
</asp:Content>