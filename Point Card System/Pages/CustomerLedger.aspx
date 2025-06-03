<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomerLedger.aspx.cs" Inherits="Point_Card_System.Pages.CustomerLedger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/CustomerLedger.css") %>' />
    <div class="main-container">
        <!-- Page Title -->
        <h2 class="page-title">Customer Ledger</h2>
        <!-- Customer Info Card -->
        <div class="custom-card">
            <div class="info-section">
                <div class="customer-info">
                    <div class="form-group">
                        <%--<asp:Label ID="lblPhoneNo" runat="server" Text="Phone No:" AssociatedControlID="txtPhoneNo"></asp:Label>
                        <asp:TextBox ID="TxtPhoneNo" runat="server" CssClass="form-control"></asp:TextBox>--%>
                        <asp:DropDownList ID="txt_SearchPhone" CssClass="form-control text-uppercase" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="txt_SearchPhone_SelectedIndexChanged">
                        </asp:DropDownList>
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
</asp:Content>
