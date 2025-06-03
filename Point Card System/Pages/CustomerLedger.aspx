<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerLedger.aspx.cs" Inherits="Point_Card_System.Pages.CustomerLedger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/CustomerLedger.css") %>' />
    <!-- SweetAlert2 CDN -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert2/11.10.1/sweetalert2.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert2/11.10.1/sweetalert2.min.css" />
    <div class="main-container">
        <!-- Page Title -->
        <h2 class="page-title">Customer Ledger</h2>
        <!-- Customer Info Card -->
        <div class="custom-card">
            <div class="info-section">
                <div class="customer-info">
                    <!-- Phone Number Input Field with Autocomplete -->
                    <div class="form-group">
                        <asp:Label ID="lblPhoneSearch" runat="server" Text="Phone Number:" AssociatedControlID="txtPhoneSearch"></asp:Label>
                        <div style="position: relative;">
                            <asp:TextBox ID="txtPhoneSearch" runat="server" CssClass="form-control" 
                                placeholder="Enter phone number..." onkeyup="filterPhoneNumbers(this.value)"></asp:TextBox>
                            <div id="phoneDropdown" class="phone-dropdown" style="display: none;">
                                <asp:Repeater ID="rptPhoneNumbers" runat="server">
                                    <ItemTemplate>
                                        <div class="phone-item" onclick="selectPhone('<%# Container.DataItem %>')">
                                            <%# Container.DataItem %>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <asp:Label ID="lblName" runat="server" Text="Name:" AssociatedControlID="txtName"></asp:Label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control text-uppercase" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblAddress" runat="server" Text="Address:" AssociatedControlID="txtAddress"></asp:Label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control text-uppercase" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Label ID="lblPoints" runat="server" Text="Points:" AssociatedControlID="txtPoints"></asp:Label>
                        <asp:TextBox ID="txtPoints" runat="server" CssClass="form-control text-uppercase" TextMode="Number" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <!-- Points Analysis -->
                <div style="margin-top: 20px; width: 1000px;">
                    <div class="point-analysis-header">
                        Points Analysis
                    </div>
                    <div class="point-analysis-content">
                        <div class="point-box">
                            <div>Total Points</div>
                            <div>
                                <asp:Label ID="lblTotalPoints" runat="server" Text="0.00"></asp:Label>
                            </div>
                        </div>
                        <div class="point-box">
                            <div>Available Points</div>
                            <div>
                                <asp:Label ID="lblAvailablePoints" runat="server" Text="0.00"></asp:Label>
                            </div>
                        </div>
                        <div class="point-box">
                            <div>Renewable Points</div>
                            <div>
                                <asp:Label ID="lblRenewablePoints" runat="server" Text="0.00"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Table Section -->
        <div class="table-container">
            <asp:GridView ID="gv_Transactions" runat="server" CssClass="custom-table" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="invoice_no" HeaderText="Ref No" />
                    <asp:BoundField DataField="Description" HeaderText="Type" />
                    <asp:BoundField DataField="PointIn" HeaderText="Point In" />
                    <asp:BoundField DataField="PointOut" HeaderText="Point Out" />
                    <asp:BoundField DataField="BalancePoint" HeaderText="Balance Point" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <!-- Hidden field to store all phone numbers for JavaScript filtering -->
    <asp:HiddenField ID="hfAllPhoneNumbers" runat="server" />

    <style>
        .phone-dropdown {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            background: white;
            border: 1px solid #ddd;
            border-top: none;
            max-height: 200px;
            overflow-y: auto;
            z-index: 1000;
            box-shadow: 0 2px 5px rgba(0,0,0,0.2);
        }
        
        .phone-item {
            padding: 8px 12px;
            cursor: pointer;
            border-bottom: 1px solid #f0f0f0;
        }
        
        .phone-item:hover {
            background-color: #f8f9fa;
        }
        
        .phone-item:last-child {
            border-bottom: none;
        }
    </style>

    <script type="text/javascript">
        var allPhoneNumbers = [];

        // Initialize phone numbers array on page load
        window.onload = function () {
            var phoneData = document.getElementById('<%= hfAllPhoneNumbers.ClientID %>').value;
            if (phoneData) {
                allPhoneNumbers = phoneData.split(',');
            }
        };
        
        function filterPhoneNumbers(inputValue) {
            var dropdown = document.getElementById('phoneDropdown');
            
            // Only show dropdown if user has typed at least 4 characters
            if (inputValue.length >= 4) {
                var filteredNumbers = allPhoneNumbers.filter(function(phone) {
                    return phone.toLowerCase().indexOf(inputValue.toLowerCase()) !== -1;
                });
                
                if (filteredNumbers.length > 0) {
                    var html = '';
                    filteredNumbers.forEach(function(phone) {
                        html += '<div class="phone-item" onclick="selectPhone(\'' + phone + '\')">' + phone + '</div>';
                    });
                    dropdown.innerHTML = html;
                    dropdown.style.display = 'block';
                } else {
                    dropdown.style.display = 'none';
                }
            } else {
                dropdown.style.display = 'none';
            }
        }
        
        function selectPhone(phoneNumber) {
            document.getElementById('<%= txtPhoneSearch.ClientID %>').value = phoneNumber;
            document.getElementById('phoneDropdown').style.display = 'none';
            
            // Trigger postback to load customer details
            __doPostBack('<%= txtPhoneSearch.UniqueID %>', '');
        }
        
        // Function to validate phone number on Enter key or blur
        function validatePhoneNumber() {
            var phoneInput = document.getElementById('<%= txtPhoneSearch.ClientID %>');
            var phoneNumber = phoneInput.value.trim();
            
            if (phoneNumber.length > 0) {
                // Check if the phone number exists in our list
                var isValid = allPhoneNumbers.some(function(phone) {
                    return phone === phoneNumber;
                });
                
                if (isValid) {
                    // Trigger postback to load customer details
                    __doPostBack('<%= txtPhoneSearch.UniqueID %>', '');
                } else {
                    // Show SweetAlert2 error for invalid phone number
                    Swal.fire({
                        icon: 'error',
                        title: 'Invalid Phone Number',
                        text: 'Invalid or unregistered customer phone number.',
                        confirmButtonColor: '#d33'
                    });
                    // Clear the invalid input
                    phoneInput.value = '';
                }
            }
        }

        // Add event listeners for phone number validation
        document.addEventListener('DOMContentLoaded', function () {
            var phoneInput = document.getElementById('<%= txtPhoneSearch.ClientID %>');

            // Validate on Enter key press
            phoneInput.addEventListener('keypress', function (e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    validatePhoneNumber();
                }
            });

            // Validate on blur (when user clicks away)
            phoneInput.addEventListener('blur', function () {
                setTimeout(function () {
                    // Small delay to allow dropdown selection to complete
                    if (document.getElementById('phoneDropdown').style.display === 'none') {
                        validatePhoneNumber();
                    }
                }, 200);
            });
        });

        // Hide dropdown when clicking outside
        document.addEventListener('click', function (event) {
            var dropdown = document.getElementById('phoneDropdown');
            var searchBox = document.getElementById('<%= txtPhoneSearch.ClientID %>');

            if (!dropdown.contains(event.target) && event.target !== searchBox) {
                dropdown.style.display = 'none';
            }
        });
    </script>
</asp:Content>