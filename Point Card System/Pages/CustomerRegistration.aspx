<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerRegistration.aspx.cs" Inherits="Point_Card_System.Pages.CustomerRegistration1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/CustomerRegistration.css") %>' />
    
    <!-- SweetAlert2 CDN -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
       
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
                            <label for="txtCustomerName" class="form-label">Customer Name <span style="color: red;">*</span></label>
                            <asp:TextBox ID="txtCustomerName" runat="server" class="form-control text-uppercase" placeholder="Enter Customer Name" OnTextChanged="txtCustomerName_TextChanged"></asp:TextBox>
                            <div class="error-message" id="customerNameError">Customer Name is required</div>
                        </div>

                        <div class="form-group">
                            <label for="txtNIC" class="form-label">NIC <span style="color: red;">*</span></label>
                            <asp:TextBox ID="txtNIC" runat="server" class="form-control" placeholder="Enter NIC" OnTextChanged="txtNIC_TextChanged"></asp:TextBox>
                            <div class="error-message" id="nicError">NIC is required</div>
                        </div>

                        <div class="form-group">
                            <label for="txtPhoneNo" class="form-label">Phone No <span style="color: red;">*</span></label>
                            <asp:TextBox ID="txtPhoneNo" runat="server" class="form-control" placeholder="Enter Phone No (10 digits)" OnTextChanged="txtPhoneNo_TextChanged"></asp:TextBox>
                            <div class="error-message" id="phoneNoError">Please enter a valid 10-digit phone number</div>
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
                            <asp:Button runat="server" class="btn btn-save" Text="Save" ID="btn_save" OnClick="btn_save_Click" OnClientClick="return validateForm();" />
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
                            <asp:TextBox ID="txtRegisteredDate" runat="server" class="form-control" ReadOnly="true" TextMode="Date" />
                            <%-- <input type="date" id="txtRegisteredDate" class="form-control" value="2024-09-09" readonly/>--%>
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
    
    <script type="text/javascript">
        // SweetAlert2 Functions
        function showSuccessMessage(message) {
            Swal.fire({
                title: 'Success!',
                text: message,
                icon: 'success',
                confirmButtonText: 'OK',
                confirmButtonColor: '#28a745',
                timer: 3000,
                timerProgressBar: true
            });
        }

        function showErrorMessage(message) {
            Swal.fire({
                title: 'Error!',
                text: message,
                icon: 'error',
                confirmButtonText: 'OK',
                confirmButtonColor: '#dc3545'
            });
        }

        function showValidationError(message) {
            Swal.fire({
                title: 'Validation Error!',
                text: message,
                icon: 'warning',
                confirmButtonText: 'OK',
                confirmButtonColor: '#ffc107'
            });
        }

        // Real-time validation functions
        function validateCustomerName() {
            const customerName = document.getElementById('<%= txtCustomerName.ClientID %>');
            const errorDiv = document.getElementById('customerNameError');

            if (customerName.value.trim() === '') {
                customerName.classList.add('error');
                customerName.classList.remove('valid');
                errorDiv.style.display = 'block';
                return false;
            } else {
                customerName.classList.remove('error');
                customerName.classList.add('valid');
                errorDiv.style.display = 'none';
                return true;
            }
        }

        function validateNIC() {
            const nic = document.getElementById('<%= txtNIC.ClientID %>');
            const errorDiv = document.getElementById('nicError');

            if (nic.value.trim() === '') {
                nic.classList.add('error');
                nic.classList.remove('valid');
                errorDiv.style.display = 'block';
                return false;
            } else {
                nic.classList.remove('error');
                nic.classList.add('valid');
                errorDiv.style.display = 'none';
                return true;
            }
        }

        function validatePhoneNumber() {
            const phoneNo = document.getElementById('<%= txtPhoneNo.ClientID %>');
            const errorDiv = document.getElementById('phoneNoError');
            const phonePattern = /^[0-9]{10}$/;
            
            if (phoneNo.value.trim() === '') {
                phoneNo.classList.add('error');
                phoneNo.classList.remove('valid');
                errorDiv.textContent = 'Phone Number is required';
                errorDiv.style.display = 'block';
                return false;
            } else if (!phonePattern.test(phoneNo.value.trim())) {
                phoneNo.classList.add('error');
                phoneNo.classList.remove('valid');
                errorDiv.textContent = 'Please enter a valid 10-digit phone number';
                errorDiv.style.display = 'block';
                return false;
            } else {
                phoneNo.classList.remove('error');
                phoneNo.classList.add('valid');
                errorDiv.style.display = 'none';
                return true;
            }
        }

        // Form validation function called on submit
        function validateForm() {
            let isValid = true;
            
            // Validate Customer Name
            if (!validateCustomerName()) {
                isValid = false;
            }
            
            // Validate NIC
            if (!validateNIC()) {
                isValid = false;
            }
            
            // Validate Phone Number
            if (!validatePhoneNumber()) {
                isValid = false;
            }
            
            if (!isValid) {
                // Show validation error with SweetAlert2
                showValidationError('Please fill in all required fields correctly.');
                
                // Scroll to first error field
                const firstError = document.querySelector('.form-control.error');
                if (firstError) {
                    firstError.scrollIntoView({ behavior: 'smooth', block: 'center' });
                    firstError.focus();
                }
            }
            
            return isValid;
        }

        // Add event listeners when page loads
        document.addEventListener('DOMContentLoaded', function() {
            const customerNameField = document.getElementById('<%= txtCustomerName.ClientID %>');
            const nicField = document.getElementById('<%= txtNIC.ClientID %>');
            const phoneNoField = document.getElementById('<%= txtPhoneNo.ClientID %>');

            // Add real-time validation on blur (when user leaves the field)
            customerNameField.addEventListener('blur', validateCustomerName);
            nicField.addEventListener('blur', validateNIC);
            phoneNoField.addEventListener('blur', validatePhoneNumber);

            // Add real-time validation on input (as user types)
            customerNameField.addEventListener('input', function () {
                if (this.classList.contains('error')) {
                    validateCustomerName();
                }
            });

            nicField.addEventListener('input', function () {
                if (this.classList.contains('error')) {
                    validateNIC();
                }
            });

            phoneNoField.addEventListener('input', function () {
                if (this.classList.contains('error')) {
                    validatePhoneNumber();
                }

                // Allow only numbers
                this.value = this.value.replace(/[^0-9]/g, '');

                // Limit to 10 digits
                if (this.value.length > 10) {
                    this.value = this.value.substring(0, 10);
                }
            });
        });
    </script>
</asp:Content>