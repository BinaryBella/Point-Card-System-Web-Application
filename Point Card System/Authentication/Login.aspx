<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Authentication_Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <!-- Bootstrap CSS and Font Awesome for the eye icon -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        .login-container {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }

        .login-image-container {
            padding: 20px;
            max-width: 500px;
            margin-right: 70px;    
            margin-left: 70px;
        }

        .login-form-container {
            background-color: #E5F2FF;
            border-radius: 8px;
            padding: 60px;
            max-width: 550px;
            width: 100%;
            height: auto;
            min-height: 550px;
        }

        .login-title {
            color: #007CFF;
            font-size: 28px;
            font-weight: bold;
            margin-top: 40px;
            margin-bottom: 30px;
            text-align: center;
        }

        .login-button {
            background-color: #007CFF;
            border-color: #007CFF;
            width: 100%;
            padding: 10px;
            font-size: 16px;
            margin-top: 20px;
            color: white;
        }

        .login-button:hover {
            background-color: #005fc3;
            border-color: #005fc3;
            width: 100%;
            padding: 10px;
            font-size: 16px;
            margin-top: 20px;
            color: white;
        }

        .form-control {
            padding: 10px;
            margin-bottom: 5px;
        }

        .form-label {
            font-weight: 500;
            margin-bottom: 10px;
        }
        
        .password-container {
            position: relative;
        }

        .password-toggle {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            border: none;
            background: transparent;
            cursor: pointer;
            color: #6c757d;
        }

        .password-toggle:hover {
            color: #007CFF;
        }

        .error-message {
            color: #dc3545;
            font-size: 0.875em;
            margin-top: 5px;
            margin-bottom: 10px;
        }

        .login-error {
            color: #dc3545;
            font-size: 0.9em;
            text-align: center;
            margin-top: 10px;
            font-weight: 500;
        }

        .form-control.is-invalid {
            border-color: #dc3545;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }

        .form-control.is-valid {
            border-color: #28a745;
            box-shadow: 0 0 0 0.2rem rgba(40, 167, 69, 0.25);
        }
    </style>
    </head>
    <body>

    <div class="login-container">
        <div class="row w-100">
            <div class="col-md-6 login-image-container">
                <asp:Image ID="imgLogin" runat="server" ImageUrl="~/Content/src/login.jpg" CssClass="img-fluid" AlternateText="Login" />
            </div>
            <div class="col-md-6 d-flex align-items-center justify-content-center">
                <div class="login-form-container">
                    <form id="form1" runat="server">
                        <div class="login-title">Login</div>

                        <div class="mb-3">
                            <asp:Label ID="Label1" for="TextBox_username" class="form-label" runat="server" Text="Username"></asp:Label>
                            <asp:TextBox ID="TextBox_username" runat="server" CssClass="form-control" placeholder="Enter User Name" MaxLength="50"></asp:TextBox>
                            
                            <!-- No validation for username -->
                        </div>

                        <div class="mb-3">
                            <asp:Label ID="Label2" for="txtPassword" class="form-label" runat="server" Text="Password"></asp:Label>
                            <div class="password-container">
                                <asp:TextBox id="txtPassword" runat="server" class="form-control" placeholder="Enter Password" TextMode="Password" MaxLength="100"></asp:TextBox>
                                <button type="button" id="togglePassword" class="password-toggle">
                                    <i class="fa fa-eye-slash" id="toggleIcon"></i>
                                </button>
                            </div>
                            
                            <!-- Server-side validation for password - Required only -->
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                ControlToValidate="txtPassword" 
                                ErrorMessage="Password is required" 
                                CssClass="error-message" 
                                Display="Dynamic" 
                                SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                        </div>

                        <!-- Login error message -->
                        <asp:Label ID="lblLoginError" runat="server" CssClass="login-error" Visible="false"></asp:Label>

                        <asp:Button ID="Button1" runat="server" Text="Login" class="btn login-button" OnClick="Button1_Click" OnClientClick="return validateForm();" />
                    </form>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
    <!-- Client-side validation and password toggle script -->
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const togglePassword = document.getElementById('togglePassword');
            const password = document.getElementById('txtPassword');
            const toggleIcon = document.getElementById('toggleIcon');

            // Password toggle functionality
            if (togglePassword && password && toggleIcon) {
                togglePassword.addEventListener('click', function () {
                    if (password.type === 'password') {
                        password.type = 'text';
                        toggleIcon.classList.remove('fa-eye-slash');
                        toggleIcon.classList.add('fa-eye');
                    } else {
                        password.type = 'password';
                        toggleIcon.classList.remove('fa-eye');
                        toggleIcon.classList.add('fa-eye-slash');
                    }
                });
            }

            // Real-time validation - only for required fields
            const usernameField = document.getElementById('TextBox_username');
            const passwordField = document.getElementById('txtPassword');

            if (passwordField) {
                passwordField.addEventListener('blur', function () {
                    validatePassword();
                });

                passwordField.addEventListener('input', function () {
                    if (this.value.length > 0) {
                        validatePassword();
                    } else {
                        this.classList.remove('is-valid', 'is-invalid');
                    }
                });
            }
        });

        function validatePassword() {
            const password = document.getElementById('txtPassword');

            if (password.value.trim() === '') {
                password.classList.remove('is-valid');
                password.classList.add('is-invalid');
                return false;
            } else {
                password.classList.remove('is-invalid');
                password.classList.add('is-valid');
                return true;
            }
        }

        function validateForm() {
            const isPasswordValid = validatePassword();

            if (!isPasswordValid) {
                return false; // Prevent form submission
            }

            return true; // Allow form submission
        }
    </script>
   </body>
    </html>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Point_Card_System.Authentication.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <!-- Bootstrap CSS and Font Awesome for the eye icon -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <style>
        .login-container {
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
        }

        .login-image-container {
            padding: 20px;
            max-width: 500px;
            margin-right: 70px;    
            margin-left: 70px;
        }

        .login-form-container {
            background-color: #E5F2FF;
            border-radius: 8px;
            padding: 60px;
            max-width: 550px;
            width: 100%;
            height: auto;
            min-height: 550px;
        }

        .login-title {
            color: #007CFF;
            font-size: 28px;
            font-weight: bold;
            margin-top: 40px;
            margin-bottom: 30px;
            text-align: center;
        }

        .login-button {
            background-color: #007CFF;
            border-color: #007CFF;
            width: 100%;
            padding: 10px;
            font-size: 16px;
            margin-top: 20px;
            color: white;
        }

        .login-button:hover {
            background-color: #005fc3;
            border-color: #005fc3;
            width: 100%;
            padding: 10px;
            font-size: 16px;
            margin-top: 20px;
            color: white;
        }

        .form-control {
            padding: 10px;
            margin-bottom: 5px;
        }

        .form-label {
            font-weight: 500;
            margin-bottom: 10px;
        }
        
        .password-container {
            position: relative;
        }

        .password-toggle {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            border: none;
            background: transparent;
            cursor: pointer;
            color: #6c757d;
        }

        .password-toggle:hover {
            color: #007CFF;
        }

        .error-message {
            color: #dc3545;
            font-size: 0.875em;
            margin-top: 5px;
            margin-bottom: 10px;
        }

        .login-error {
            color: #dc3545;
            font-size: 0.9em;
            text-align: center;
            margin-top: 10px;
            font-weight: 500;
        }

        .form-control.is-invalid {
            border-color: #dc3545;
            box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25);
        }

        .form-control.is-valid {
            border-color: #28a745;
            box-shadow: 0 0 0 0.2rem rgba(40, 167, 69, 0.25);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="row w-100">
                <div class="col-md-6 login-image-container">
                    <asp:Image ID="imgLogin" runat="server" ImageUrl="~/Content/src/login.jpg" CssClass="img-fluid" AlternateText="Login" />
                </div>
                <div class="col-md-6 d-flex align-items-center justify-content-center">
                    <div class="login-form-container">
                        <div class="login-title">Login</div>

                        <div class="mb-3">
                            <asp:Label ID="lblUsername" for="txtUsername" class="form-label" runat="server" Text="Username"></asp:Label>
                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter User Name" MaxLength="50"></asp:TextBox>
                            
                            <!-- Username validation -->
                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" 
                                ControlToValidate="txtUsername" 
                                ErrorMessage="Username is required" 
                                CssClass="error-message" 
                                Display="Dynamic" 
                                SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="mb-3">
                            <asp:Label ID="lblPassword" for="txtPassword" class="form-label" runat="server" Text="Password"></asp:Label>
                            <div class="password-container">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Enter Password" TextMode="Password" MaxLength="100"></asp:TextBox>
                                <button type="button" id="togglePassword" class="password-toggle">
                                    <i class="fa fa-eye-slash" id="toggleIcon"></i>
                                </button>
                            </div>
                            
                            <!-- Server-side validation for password -->
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" 
                                ControlToValidate="txtPassword" 
                                ErrorMessage="Password is required" 
                                CssClass="error-message" 
                                Display="Dynamic" 
                                SetFocusOnError="true">
                            </asp:RequiredFieldValidator>
                        </div>

                        <!-- Login error message -->
                        <asp:Label ID="lblLoginError" runat="server" CssClass="login-error" Visible="false"></asp:Label>

                        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn login-button" OnClick="btnLogin_Click" OnClientClick="return validateForm();" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
    <!-- Client-side validation and password toggle script -->
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            const togglePassword = document.getElementById('togglePassword');
            const password = document.getElementById('<%= txtPassword.ClientID %>');
            const toggleIcon = document.getElementById('toggleIcon');

            // Password toggle functionality
            if (togglePassword && password && toggleIcon) {
                togglePassword.addEventListener('click', function () {
                    if (password.type === 'password') {
                        password.type = 'text';
                        toggleIcon.classList.remove('fa-eye-slash');
                        toggleIcon.classList.add('fa-eye');
                    } else {
                        password.type = 'password';
                        toggleIcon.classList.remove('fa-eye');
                        toggleIcon.classList.add('fa-eye-slash');
                    }
                });
            }

            // Real-time validation
            const usernameField = document.getElementById('<%= txtUsername.ClientID %>');
            const passwordField = document.getElementById('<%= txtPassword.ClientID %>');

            if (usernameField) {
                usernameField.addEventListener('blur', function () {
                    validateUsername();
                });

                usernameField.addEventListener('input', function () {
                    if (this.value.length > 0) {
                        validateUsername();
                    } else {
                        this.classList.remove('is-valid', 'is-invalid');
                    }
                });
            }

            if (passwordField) {
                passwordField.addEventListener('blur', function () {
                    validatePassword();
                });

                passwordField.addEventListener('input', function () {
                    if (this.value.length > 0) {
                        validatePassword();
                    } else {
                        this.classList.remove('is-valid', 'is-invalid');
                    }
                });
            }
        });

        function validateUsername() {
            const username = document.getElementById('<%= txtUsername.ClientID %>');

            if (username.value.trim() === '') {
                username.classList.remove('is-valid');
                username.classList.add('is-invalid');
                return false;
            } else {
                username.classList.remove('is-invalid');
                username.classList.add('is-valid');
                return true;
            }
        }

        function validatePassword() {
            const password = document.getElementById('<%= txtPassword.ClientID %>');

            if (password.value.trim() === '') {
                password.classList.remove('is-valid');
                password.classList.add('is-invalid');
                return false;
            } else {
                password.classList.remove('is-invalid');
                password.classList.add('is-valid');
                return true;
            }
        }

        function validateForm() {
            const isUsernameValid = validateUsername();
            const isPasswordValid = validatePassword();

            if (!isUsernameValid || !isPasswordValid) {
                return false; // Prevent form submission
            }

            return true; // Allow form submission
        }
    </script>
</body>
</html>