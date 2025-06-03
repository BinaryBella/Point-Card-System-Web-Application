<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Point_Card_System._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>

        .dashboard-title {
            color: #3295FE;
            text-align: center;
            margin-top: 20px;
            margin-bottom: 30px;
            font-weight: bold;
            font-size: 28px;
        }
        .dashboard-card {
            background-color: #3295FE;
            color: #FFFFFF;
            border-radius: 10px;
            padding: 20px;
            text-align: center;
            margin-bottom: 30px;
        }
        .card-icon {
            font-size: 24px;
            margin-bottom: 10px;
        }
        .card-number {
            font-size: 60px;
            font-weight: bold;
            margin: 10px 0;
        }
        .card-text {
            font-size: 18px;
        }
    </style>

    <div class="container">
        <h1 class="dashboard-title">Dashboard</h1>
        
        <!-- First Row of Cards -->
        <div class="row">
            <!-- Card 1 -->
            <div class="col-md-4">
                <div class="dashboard-card">
                    <div class="card-icon">
                        <i class="bi bi-person-fill">👤</i>
                    </div>
                    <div class="card-number">11</div>
                    <div class="card-text">Registered Customers</div>
                </div>
            </div>
            
            <!-- Card 2 -->
            <div class="col-md-4">
                <div class="dashboard-card">
                    <div class="card-icon">
                        <i class="bi bi-person-fill">👤</i>
                    </div>
                    <div class="card-number">11</div>
                    <div class="card-text">Registered Customers</div>
                </div>
            </div>
            
            <!-- Card 3 -->
            <div class="col-md-4">
                <div class="dashboard-card">
                    <div class="card-icon">
                        <i class="bi bi-person-fill">👤</i>
                    </div>
                    <div class="card-number">11</div>
                    <div class="card-text">Registered Customers</div>
                </div>
            </div>
        </div>
        
        <!-- Second Row of Cards -->
        <div class="row">
            <!-- Card 4 -->
            <div class="col-md-4">
                <div class="dashboard-card">
                    <div class="card-icon">
                        <i class="bi bi-person-fill">👤</i>
                    </div>
                    <div class="card-number">11</div>
                    <div class="card-text">Registered Customers</div>
                </div>
            </div>
            
            <!-- Card 5 -->
            <div class="col-md-4">
                <div class="dashboard-card">
                    <div class="card-icon">
                        <i class="bi bi-person-fill">👤</i>
                    </div>
                    <div class="card-number">11</div>
                    <div class="card-text">Registered Customers</div>
                </div>
            </div>
            
            <!-- Card 6 -->
            <div class="col-md-4">
                <div class="dashboard-card">
                    <div class="card-icon">
                        <i class="bi bi-person-fill">👤</i>
                    </div>
                    <div class="card-number">11</div>
                    <div class="card-text">Registered Customers</div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

</asp:Content>
