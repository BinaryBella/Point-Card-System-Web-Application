<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Point_Card_System._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.0/font/bootstrap-icons.css" rel="stylesheet" />
    
    <style>
        body {
            background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .dashboard-container {
            background: rgba(255, 255, 255, 0.95);
            border-radius: 20px;
            padding: 30px;
            margin: 20px auto;
            backdrop-filter: blur(10px);
        }

        .dashboard-title {
            background: linear-gradient(45deg, #3295FE, #4facfe);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            background-clip: text;
            text-align: center;
            margin-bottom: 40px;
            font-weight: 800;
            font-size: 2.5rem;
        }

        .dashboard-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #FFFFFF;
            border-radius: 20px;
            padding: 30px 20px;
            text-align: center;
            margin-bottom: 30px;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
        }

        .dashboard-card::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: linear-gradient(45deg, rgba(255,255,255,0.1), transparent);
            pointer-events: none;
        }

        .dashboard-card:hover {
            transform: translateY(-10px) scale(1.02);
        }

        .dashboard-card.customers {
            background: linear-gradient(135deg, #3295FE 0%, #4facfe 100%);
        }

        .dashboard-card.points {
            background: linear-gradient(135deg, #fa709a 0%, #fee140 100%);
        }

        .dashboard-card.transactions {
            background: linear-gradient(135deg, #a8edea 0%, #fed6e3 100%);
            color: #333;
        }

        .dashboard-card.revenue {
            background: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%);
            color: #333;
        }

        .dashboard-card.branches {
            background: linear-gradient(135deg, #ff9a9e 0%, #fecfef 100%);
            color: #333;
        }

        .dashboard-card.active-cards {
            background: linear-gradient(135deg, #a18cd1 0%, #fbc2eb 100%);
            color: #333;
        }

        .card-icon {
            font-size: 3rem;
            margin-bottom: 15px;
            opacity: 0.9;
        }

        .card-number {
            font-size: 3.5rem;
            font-weight: 900;
            margin: 15px 0;
            background: rgba(255,255,255,0.2);
            border-radius: 10px;
            padding: 10px;
            backdrop-filter: blur(5px);
        }

        .card-text {
            font-size: 1.1rem;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
            opacity: 0.95;
        }

        .pulse {
            animation: pulse 2s infinite;
        }

        @keyframes pulse {
            0% { transform: scale(1); }
            50% { transform: scale(1.05); }
            100% { transform: scale(1); }
        }

        .fade-in {
            animation: fadeIn 1s ease-in;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(30px); }
            to { opacity: 1; transform: translateY(0); }
        }
    </style>

    <div class="container-fluid">
        <div class="dashboard-container">
            <h1 class="dashboard-title">Point Card System Dashboard</h1>
            
            <!-- First Row of Cards -->
            <div class="row g-4 fade-in">
                <!-- Registered Customers Card -->
                <div class="col-lg-4 col-md-6">
                    <div class="dashboard-card customers pulse">
                        <div class="card-icon">
                            <i class="bi bi-people-fill"></i>
                        </div>
                        <div class="card-number"><%= RegisteredCustomerCount %></div>
                        <div class="card-text">Registered Customers</div>
                    </div>
                </div>
                
                <!-- Total Points Card -->
                <div class="col-lg-4 col-md-6">
                    <div class="dashboard-card points">
                        <div class="card-icon">
                            <i class="bi bi-star-fill"></i>
                        </div>
                        <div class="card-number">15,847</div>
                        <div class="card-text">Total Points Issued</div>
                    </div>
                </div>
                
                <!-- Transactions Card -->
                <div class="col-lg-4 col-md-6">
                    <div class="dashboard-card transactions">
                        <div class="card-icon">
                            <i class="bi bi-receipt"></i>
                        </div>
                        <div class="card-number">1,256</div>
                        <div class="card-text">Total Transactions</div>
                    </div>
                </div>
            </div>
            
            <!-- Second Row of Cards -->
            <div class="row g-4 fade-in">
                <!-- Revenue Card -->
                <div class="col-lg-4 col-md-6">
                    <div class="dashboard-card revenue">
                        <div class="card-icon">
                            <i class="bi bi-currency-dollar"></i>
                        </div>
                        <div class="card-number">$89,450</div>
                        <div class="card-text">Total Revenue</div>
                    </div>
                </div>
                
                <!-- Active Branches Card -->
                <div class="col-lg-4 col-md-6">
                    <div class="dashboard-card branches">
                        <div class="card-icon">
                            <i class="bi bi-building"></i>
                        </div>
                        <div class="card-number">8</div>
                        <div class="card-text">Active Branches</div>
                    </div>
                </div>
                
                <!-- Active Cards Card -->
                <div class="col-lg-4 col-md-6">
                    <div class="dashboard-card active-cards">
                        <div class="card-icon">
                            <i class="bi bi-credit-card"></i>
                        </div>
                        <div class="card-number">342</div>
                        <div class="card-text">Active Point Cards</div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

</asp:Content>