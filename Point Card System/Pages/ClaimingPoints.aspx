<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClaimingPoints.aspx.cs" Inherits="Point_Card_System.Pages.ClaimingPoints" %>
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
                <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control" Placeholder="Enter Phone No"  OnTextChanged="txtPhoneNo_TextChanged"></asp:TextBox>
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
        var phoneValidationTimeout;

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

        // Function to validate phone number and fetch customer data
        function validatePhoneAndFetchData(phoneNumber) {
            if (phoneNumber.trim() === '') {
                return;
            }

            // Show loading indicator (optional)
            document.getElementById('<%= txtCustomerName.ClientID %>').value = 'Loading...';
            document.getElementById('<%= txtAvailablePoints.ClientID %>').value = 'Loading...';

            // Trigger server-side validation
            __doPostBack('<%= txtPhoneNo.UniqueID %>', '');
        }
        
        // Real-time validation as user types
        document.addEventListener('DOMContentLoaded', function() {
            var phoneNo = document.getElementById('<%= txtPhoneNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceNum = document.getElementById('<%= txtInvoiceNum.ClientID %>');
            var claimingPoints = document.getElementById('<%= txtClaimingPoints.ClientID %>');

            // Add event listener for phone number input with debouncing
            phoneNo.addEventListener('input', function () {
                clearTimeout(phoneValidationTimeout);
                var phoneValue = this.value.trim();
                
                // Clear previous data if phone is empty
                if (phoneValue === '') {
                    document.getElementById('<%= txtCustomerName.ClientID %>').value = '';
                    document.getElementById('<%= txtAvailablePoints.ClientID %>').value = '';
                    return;
                }
                
                // Set timeout to avoid too many server calls
                phoneValidationTimeout = setTimeout(function() {
                    if (phoneValue.length >= 10) { // Adjust minimum length as needed
                        validatePhoneAndFetchData(phoneValue);
                    }
                }, 800); // Wait 800ms after user stops typing
            });

            // Add blur event for immediate validation when user leaves the field
            phoneNo.addEventListener('blur', function () {
                clearTimeout(phoneValidationTimeout);
                var phoneValue = this.value.trim();
                
                if (phoneValue === '') {
                    showError(this, document.getElementById('phoneError'), 'Phone number is required.');
                } else if (phoneValue.length >= 10) {
                    validatePhoneAndFetchData(phoneValue);
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
        
        // Function to be called from server-side to focus on invoice field
        function focusInvoiceField() {
            setTimeout(function() {
                document.getElementById('<%= txtInvoiceNum.ClientID %>').focus();
            }, 100);
        }
    </script>
</asp:Content>--%>

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
                <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control" Placeholder="Enter Phone No" OnTextChanged="txtPhoneNo_TextChanged" AutoPostBack="true"></asp:TextBox>
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
        var phoneValidationTimeout;

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

        // Function to validate phone number and fetch customer data
        function validatePhoneAndFetchData(phoneNumber) {
            if (phoneNumber.trim() === '') {
                return;
            }

            // Show loading indicator (optional)
            document.getElementById('<%= txtCustomerName.ClientID %>').value = 'Loading...';
            document.getElementById('<%= txtAvailablePoints.ClientID %>').value = 'Loading...';

            // Trigger server-side validation
            __doPostBack('<%= txtPhoneNo.UniqueID %>', '');
        }

        // Real-time validation as user types
        document.addEventListener('DOMContentLoaded', function () {
            var phoneNo = document.getElementById('<%= txtPhoneNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceNum = document.getElementById('<%= txtInvoiceNum.ClientID %>');
            var claimingPoints = document.getElementById('<%= txtClaimingPoints.ClientID %>');

            // Add event listener for phone number input with debouncing
            phoneNo.addEventListener('input', function () {
                clearTimeout(phoneValidationTimeout);
                var phoneValue = this.value.trim();
                
                // Clear previous data if phone is empty
                if (phoneValue === '') {
                    document.getElementById('<%= txtCustomerName.ClientID %>').value = '';
                    document.getElementById('<%= txtAvailablePoints.ClientID %>').value = '';
                    checkFormCompletion(); // Check form completion
                    return;
                }
                
                // Set timeout to avoid too many server calls
                phoneValidationTimeout = setTimeout(function() {
                    if (phoneValue.length >= 10) { // Adjust minimum length as needed
                        validatePhoneAndFetchData(phoneValue);
                    }
                }, 800); // Wait 800ms after user stops typing
                
                // Check form completion after input
                setTimeout(checkFormCompletion, 900);
            });

            // Add blur event for immediate validation when user leaves the field
            phoneNo.addEventListener('blur', function () {
                clearTimeout(phoneValidationTimeout);
                var phoneValue = this.value.trim();
                
                if (phoneValue === '') {
                    showError(this, document.getElementById('phoneError'), 'Phone number is required.');
                } else if (phoneValue.length >= 10) {
                    validatePhoneAndFetchData(phoneValue);
                    resetErrorState(this, document.getElementById('phoneError'));
                }
                checkFormCompletion(); // Check form completion
            });

            customerName.addEventListener('blur', function () {
                if (this.value.trim() === '') {
                    showError(this, document.getElementById('customerNameError'), 'Customer name is required.');
                } else {
                    resetErrorState(this, document.getElementById('customerNameError'));
                }
                checkFormCompletion(); // Check form completion
            });

            invoiceNum.addEventListener('blur', function () {
                if (this.value.trim() === '') {
                    showError(this, document.getElementById('invoiceError'), 'Invoice number is required.');
                } else {
                    resetErrorState(this, document.getElementById('invoiceError'));
                }
                checkFormCompletion(); // Check form completion
            });
            
            // Add input event listener for invoice number
            invoiceNum.addEventListener('input', function () {
                checkFormCompletion(); // Check form completion on input
            });

            claimingPoints.addEventListener('blur', function () {
                if (this.value.trim() === '') {
                    showError(this, document.getElementById('claimingPointsError'), 'Claiming points is required.');
                } else if (isNaN(this.value.trim()) || parseFloat(this.value.trim()) <= 0) {
                    showError(this, document.getElementById('claimingPointsError'), 'Please enter a valid positive number for claiming points.');
                } else {
                    resetErrorState(this, document.getElementById('claimingPointsError'));
                }
                checkFormCompletion(); // Check form completion
            });
            
            // Add input event listener for claiming points
            claimingPoints.addEventListener('input', function () {
                checkFormCompletion(); // Check form completion on input
            });
            
            // Initial form completion check
            checkFormCompletion();
        });
        
        // Function to be called from server-side to focus on invoice field
        function focusInvoiceField() {
            setTimeout(function() {
                document.getElementById('<%= txtInvoiceNum.ClientID %>').focus();
            }, 100);
        }
        
        // Function to check if all required fields are filled and enable/disable save button
        function checkFormCompletion() {
            var phoneNo = document.getElementById('<%= txtPhoneNo.ClientID %>');
            var customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            var invoiceNum = document.getElementById('<%= txtInvoiceNum.ClientID %>');
            var claimingPoints = document.getElementById('<%= txtClaimingPoints.ClientID %>');
            var availablePoints = document.getElementById('<%= txtAvailablePoints.ClientID %>');
            var saveButton = document.getElementById('<%= btnSave.ClientID %>');

            // Check if all required fields are filled
            var phoneValid = phoneNo.value.trim() !== '';
            var customerValid = customerName.value.trim() !== '';
            var invoiceValid = invoiceNum.value.trim() !== '';
            var claimingValid = claimingPoints.value.trim() !== '' &&
                !isNaN(claimingPoints.value.trim()) &&
                parseFloat(claimingPoints.value.trim()) > 0;
            var availableValid = availablePoints.value.trim() !== '' &&
                !isNaN(availablePoints.value.trim()) &&
                parseFloat(availablePoints.value.trim()) >= 1000;
            var pointsCheckValid = claimingValid && availableValid &&
                parseFloat(claimingPoints.value.trim()) <= parseFloat(availablePoints.value.trim());

            // Enable save button only if all conditions are met
            if (phoneValid && customerValid && invoiceValid && pointsCheckValid) {
                saveButton.disabled = false;
                saveButton.classList.remove('disabled');
            } else {
                saveButton.disabled = true;
                saveButton.classList.add('disabled');
            }
        }
    </script>
</asp:Content>