<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReturnPoints.aspx.cs" Inherits="Point_Card_System.Pages.ReturnPoints1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/ReturnPoints.css") %>' />
    <!-- SweetAlert2 CSS -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/11.7.32/sweetalert2.min.css" />

    <div class="container">
        <h1 class="page-title">Return Points</h1>
        <div class="content-box">
            <div class="form-group">
                <label for="txtInvoiceNo" class="form-label">Invoice No <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" Placeholder="Enter Invoice No" OnTextChanged="txtInvoiceNo_TextChanged"></asp:TextBox>
                <div id="errorInvoiceNo" class="error-message">Invoice No is required</div>
            </div>
            
            <div class="form-group">
                <label for="txtCustomerName" class="form-label">Customer Name <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtCustomerName" runat="server" CssClass="form-control" Placeholder="Enter Customer Name"></asp:TextBox>
                <div id="errorCustomerName" class="error-message">Customer Name is required</div>
            </div>
            
            <div class="form-group">
                <label for="txtInvoiceAmount" class="form-label">Total Invoice Amount <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtInvoiceAmount" runat="server" CssClass="form-control" Placeholder="Enter Invoice Amount"></asp:TextBox>
                <div id="errorInvoiceAmount" class="error-message">Total Invoice Amount is required</div>
            </div>
            
            <div class="form-group">
                <label for="txtReturnAmount" class="form-label">Return Amount <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtReturnAmount" runat="server" CssClass="form-control" Placeholder="Enter Return Amount" OnTextChanged="txtReturnAmount_TextChanged"></asp:TextBox>
                <div id="errorReturnAmount" class="error-message">Return Amount is required</div>
            </div>
            
            <div class="form-group">
                <label for="txtAvailablePoints" class="form-label">Return Points Amount <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtAvailablePoints" runat="server" CssClass="form-control" Placeholder="Return Points Amount"></asp:TextBox>
                <div id="errorAvailablePoints" class="error-message">Return Points Amount is required</div>
            </div>
            
            <div class="form-group">
                <label for="txtReturnInvNum" class="form-label">Return Invoice Number <span style="color: red;">*</span></label>
                <asp:TextBox ID="txtReturnInvNum" runat="server" CssClass="form-control" Placeholder="Return Invoice Number" OnTextChanged="txtReturnInvNum_TextChanged"></asp:TextBox>
                <div id="errorReturnInvNum" class="error-message">Return Invoice Number is required</div>
            </div>
            
            <div class="buttons-container">
                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-cancel" Text="Cancel" />
                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-save" Text="Save" OnClick="btnSave_Click" OnClientClick="return validateForm();" />
            </div>
        </div>
    </div>

    <!-- SweetAlert2 JavaScript -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/limonte-sweetalert2/11.7.32/sweetalert2.min.js"></script>
    
    <script type="text/javascript">
        function validateForm() {
            var isValid = true;

            // Get all input fields
            var invoiceNo = document.getElementById('<%= txtInvoiceNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceAmount = document.getElementById('<%= txtInvoiceAmount.ClientID %>');
            var returnAmount = document.getElementById('<%= txtReturnAmount.ClientID %>');
            var availablePoints = document.getElementById('<%= txtAvailablePoints.ClientID %>');
            var returnInvNum = document.getElementById('<%= txtReturnInvNum.ClientID %>');
            
            // Validate Invoice No
            if (invoiceNo.value.trim() === '') {
                showError('errorInvoiceNo', invoiceNo);
                isValid = false;
            } else {
                hideError('errorInvoiceNo', invoiceNo);
            }
            
            // Validate Customer Name
            if (customerName.value.trim() === '') {
                showError('errorCustomerName', customerName);
                isValid = false;
            } else {
                hideError('errorCustomerName', customerName);
            }
            
            // Validate Invoice Amount
            if (invoiceAmount.value.trim() === '') {
                showError('errorInvoiceAmount', invoiceAmount);
                isValid = false;
            } else {
                hideError('errorInvoiceAmount', invoiceAmount);
            }
            
            // Validate Return Amount
            if (returnAmount.value.trim() === '') {
                showError('errorReturnAmount', returnAmount);
                isValid = false;
            } else {
                hideError('errorReturnAmount', returnAmount);
            }
            
            // Validate Available Points
            if (availablePoints.value.trim() === '') {
                showError('errorAvailablePoints', availablePoints);
                isValid = false;
            } else {
                hideError('errorAvailablePoints', availablePoints);
            }
            
            // Validate Return Invoice Number
            if (returnInvNum.value.trim() === '') {
                showError('errorReturnInvNum', returnInvNum);
                isValid = false;
            } else {
                hideError('errorReturnInvNum', returnInvNum);
            }
            
            return isValid;
        }
        
        function showError(errorId, inputElement) {
            document.getElementById(errorId).style.display = 'block';
            inputElement.classList.add('error');
        }
        
        function hideError(errorId, inputElement) {
            document.getElementById(errorId).style.display = 'none';
            inputElement.classList.remove('error');
        }
        
        // Function to show SweetAlert2 error message
        function showInvalidInvoiceAlert() {
            Swal.fire({
                icon: 'error',
                title: 'Invalid Invoice',
                text: 'Invalid invoice number or invoice amount is 0.',
                confirmButtonText: 'OK',
                confirmButtonColor: '#dc3545'
            });
        }
        
        // Function to show SweetAlert2 success message
        function showSuccessAlert(message) {
            Swal.fire({
                icon: 'success',
                title: 'Success!',
                text: message,
                confirmButtonText: 'OK',
                confirmButtonColor: '#28a745'
            });
        }
        
        // Function to show SweetAlert2 error message for save failure
        function showErrorAlert(message) {
            Swal.fire({
                icon: 'error',
                title: 'Error!',
                text: message,
                confirmButtonText: 'OK',
                confirmButtonColor: '#dc3545'
            });
        }
        
        // Real-time validation on blur events
        document.addEventListener('DOMContentLoaded', function() {
            var fields = [
                { input: '<%= txtInvoiceNo.ClientID %>', error: 'errorInvoiceNo' },
                { input: '<%= txtCustomerName.ClientID %>', error: 'errorCustomerName' },
                { input: '<%= txtInvoiceAmount.ClientID %>', error: 'errorInvoiceAmount' },
                { input: '<%= txtReturnAmount.ClientID %>', error: 'errorReturnAmount' },
                { input: '<%= txtAvailablePoints.ClientID %>', error: 'errorAvailablePoints' },
                { input: '<%= txtReturnInvNum.ClientID %>', error: 'errorReturnInvNum' }
            ];

            fields.forEach(function (field) {
                var inputElement = document.getElementById(field.input);
                if (inputElement) {
                    inputElement.addEventListener('blur', function () {
                        if (this.value.trim() === '') {
                            showError(field.error, this);
                        } else {
                            hideError(field.error, this);
                        }
                    });

                    // Clear error on focus
                    inputElement.addEventListener('focus', function () {
                        hideError(field.error, this);
                    });
                }
            });
        });
    </script>
</asp:Content>