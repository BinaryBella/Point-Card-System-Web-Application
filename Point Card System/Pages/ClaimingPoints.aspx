<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimingPoints.aspx.cs" Inherits="Point_Card_System.Pages.ClaimingPoints" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/ClaimingPoints.css") %>' />
    
    <!-- SweetAlert2 CSS and JS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.all.min.js"></script>
       
    <div class="container">
        <h1 class="page-title">Claim Points</h1>
        <div class="content-box">
            <div class="form-group">
                <label for="txtPhoneNo" class="form-label">Phone No <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control" Placeholder="Enter Phone No" OnTextChanged="txtPhoneNo_TextChanged"></asp:TextBox>
                <span id="phoneError" class="error-message" style="display: none;">Phone number is required.</span>
            </div>
            <div class="form-group">
                <label for="txtCustomerName" class="form-label">Customer Name <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control text-uppercase" Placeholder="Customer Name" readonly></asp:TextBox>
                <span id="customerNameError" class="error-message" style="display: none;">Customer name is required.</span>
            </div>
            <div class="form-group">
                <label for="txtAvailablePoints" class="form-label">Available Points</label>
                <asp:TextBox ID="txtAvailablePoints" runat="server" CssClass="form-control" Placeholder="Available Points" Enabled="False"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtInvoiceNum" class="form-label">Invoice Number <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtInvoiceNum" runat="server" CssClass="form-control text-uppercase" Placeholder="Enter Invoice Number" OnTextChanged="txtInvoiceNum_TextChanged"></asp:TextBox>
                <span id="invoiceError" class="error-message" style="display: none;">Invoice number is required.</span>
            </div>
            <div class="form-group">
                <label for="txtClaimingPoints" class="form-label">Claiming Points <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtClaimingPoints" runat="server" CssClass="form-control" Placeholder="Claiming Points" OnTextChanged="txtClaimingPoints_TextChanged"></asp:TextBox>
                <span id="claimingPointsError" class="error-message" style="display: none;">Claiming points is required.</span>
            </div>
            <div class="buttons-container">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-cancel" Text="Cancel" />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-save" Text="Save" OnClick="btnSave_Click" OnClientClick="return validateForm();" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function validateForm() {
            var isValid = true;

            // Get form elements
            var phoneNo = document.getElementById('<%= txtPhoneNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceNum = document.getElementById('<%= txtInvoiceNum.ClientID %>');
            var claimingPoints = document.getElementById('<%= txtClaimingPoints.ClientID %>');
            
            // Get error message elements
            var phoneError = document.getElementById('phoneError');
            var customerNameError = document.getElementById('customerNameError');
            var invoiceError = document.getElementById('invoiceError');
            var claimingPointsError = document.getElementById('claimingPointsError');
            
            // Reset all error states
            resetErrorState(phoneNo, phoneError);
            resetErrorState(customerName, customerNameError);
            resetErrorState(invoiceNum, invoiceError);
            resetErrorState(claimingPoints, claimingPointsError);
            
            // Validate Phone Number
            if (phoneNo.value.trim() === '') {
                showError(phoneNo, phoneError, 'Phone number is required.');
                isValid = false;
            }
            
            // Validate Customer Name
            if (customerName.value.trim() === '') {
                showError(customerName, customerNameError, 'Customer name is required.');
                isValid = false;
            }
            
            // Validate Invoice Number
            if (invoiceNum.value.trim() === '') {
                showError(invoiceNum, invoiceError, 'Invoice number is required.');
                isValid = false;
            }
            
            // Validate Claiming Points
            if (claimingPoints.value.trim() === '') {
                showError(claimingPoints, claimingPointsError, 'Claiming points is required.');
                isValid = false;
            } else if (isNaN(claimingPoints.value.trim()) || parseFloat(claimingPoints.value.trim()) <= 0) {
                showError(claimingPoints, claimingPointsError, 'Please enter a valid positive number for claiming points.');
                isValid = false;
            }
            
            return isValid;
        }
        
        function showError(inputElement, errorElement, message) {
            inputElement.classList.add('error');
            errorElement.textContent = message;
            errorElement.style.display = 'block';
        }
        
        function resetErrorState(inputElement, errorElement) {
            inputElement.classList.remove('error');
            errorElement.style.display = 'none';
        }
        
        // Function to show SweetAlert2 error message
        function showSweetAlertError(message) {
            Swal.fire({
                icon: 'error',
                title: 'Invalid Mobile Number',
                text: message,
                confirmButtonText: 'OK',
                confirmButtonColor: '#dc3545'
            });
        }
        
        // Real-time validation as user types
        document.addEventListener('DOMContentLoaded', function() {
            var phoneNo = document.getElementById('<%= txtPhoneNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceNum = document.getElementById('<%= txtInvoiceNum.ClientID %>');
            var claimingPoints = document.getElementById('<%= txtClaimingPoints.ClientID %>');

            // Add event listeners for real-time validation
            phoneNo.addEventListener('blur', function () {
                if (this.value.trim() === '') {
                    showError(this, document.getElementById('phoneError'), 'Phone number is required.');
                } else {
                    resetErrorState(this, document.getElementById('phoneError'));
                }
            });

            customerName.addEventListener('blur', function () {
                if (this.value.trim() === '') {
                    showError(this, document.getElementById('customerNameError'), 'Customer name is required.');
                } else {
                    resetErrorState(this, document.getElementById('customerNameError'));
                }
            });

            invoiceNum.addEventListener('blur', function () {
                if (this.value.trim() === '') {
                    showError(this, document.getElementById('invoiceError'), 'Invoice number is required.');
                } else {
                    resetErrorState(this, document.getElementById('invoiceError'));
                }
            });

            claimingPoints.addEventListener('blur', function () {
                if (this.value.trim() === '') {
                    showError(this, document.getElementById('claimingPointsError'), 'Claiming points is required.');
                } else if (isNaN(this.value.trim()) || parseFloat(this.value.trim()) <= 0) {
                    showError(this, document.getElementById('claimingPointsError'), 'Please enter a valid positive number for claiming points.');
                } else {
                    resetErrorState(this, document.getElementById('claimingPointsError'));
                }
            });
        });
    </script>
</asp:Content>