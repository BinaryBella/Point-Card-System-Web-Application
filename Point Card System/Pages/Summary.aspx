
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="Point_Card_System.Pages.Summary1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.7.2/font/bootstrap-icons.css" />

    <link rel="stylesheet" href='<%= ResolveUrl("~/Content/CustomCss/Summary.css") %>' />
    <div class="container">
        <h1 class="page-title">Summary</h1>

        <div class="mb-3 row">
            <div class="col-md-4">
                <label for="txtPhoneNo" class="form-label">Phone No:</label>
                <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="form-control" />
            </div>
            <div class="col-md-4">
                <label for="ddlBranch" class="form-label">Branch:</label>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                    <asp:ListItem Text="-- All Branches --" Value="" />
                </asp:DropDownList>
            </div>
            <div class="col-md-4 d-flex align-items-end">
                <asp:Button ID="btnApplyFilter" runat="server" Text="Apply Filter" CssClass="btn btn-primary me-2" OnClick="btnApplyFilter_Click" />
                <asp:Button ID="btnClearFilter" runat="server" Text="Clear Filter" CssClass="btn btn-secondary" OnClick="btnClearFilter_Click" />
            </div>
        </div>

        <!-- Records per page and info -->
        <div class="mb-3 row">
            <div class="col-md-6">
                <label for="ddlPageSize" class="form-label">Records per page:</label>
                <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-select w-auto d-inline-block" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                    <asp:ListItem Value="10" Text="10" Selected="True" />
                    <asp:ListItem Value="25" Text="25" />
                    <asp:ListItem Value="50" Text="50" />
                    <asp:ListItem Value="100" Text="100" />
                </asp:DropDownList>
            </div>
            <div class="col-md-6 text-end">
                <asp:Label ID="lblRecordCount" runat="server" CssClass="text-muted" />
            </div>
        </div>

        <div class="table-responsive">
            <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false"
                CssClass="table table-bordered customers-table" Width="100%" OnRowCommand="gvCustomers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" />
                    <asp:BoundField DataField="Mobile_Number" HeaderText="Phone No" />
                    <asp:BoundField DataField="Branch_ID" HeaderText="Registered Branch" />
                    <asp:BoundField DataField="Available_Points" HeaderText="Available Points" />
                    <asp:TemplateField HeaderText="STATUS">
                        <ItemTemplate>
                            <span class='<%# Convert.ToBoolean(Eval("Active")) ? "status-badge status-active" : "status-badge status-inactive" %>'>
                                <i class='<%# Convert.ToBoolean(Eval("Active")) ? "bi bi-check-circle-fill" : "bi bi-x-circle-fill" %> me-1'></i>
                                <%# Convert.ToBoolean(Eval("Active")) ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ACTIONS">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-warning btn-sm"
                                CommandName="EditRecord" CommandArgument='<%# Eval("Customer_Id") %>'>
                    <i class="bi bi-pencil-fill"></i>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server"
                                CssClass='<%# Convert.ToBoolean(Eval("Active")) ? "btn btn-danger btn-sm" : "btn btn-success btn-sm" %>'
                                CommandName="ToggleStatus" CommandArgument='<%# Eval("Customer_Id") %>'
                                OnClientClick='<%# "return confirmDelete(" + Eval("Customer_Id") + ");" %>'>
                    <i class='<%# Convert.ToBoolean(Eval("Active")) ? "bi bi-toggle-off" : "bi bi-toggle-on" %>'></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EmptyDataTemplate>
                    <tr>
                        <td colspan="6" class="text-center">No records found.</td>
                    </tr>
                </EmptyDataTemplate>

                <HeaderStyle BackColor="Black" ForeColor="White" />
                <RowStyle BackColor="#e0e0e0" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
        </div>

        <!-- Simple Pagination Controls -->
        <div class="row mt-3">
            <div class="col-md-12">
                <nav aria-label="Page navigation">
                    <div class="d-flex justify-content-between align-items-center">
                        <div>
                            <asp:Label ID="lblPageInfo" runat="server" CssClass="text-muted" />
                        </div>
                        <ul class="pagination mb-0">
                            <li class="page-item">
                                <asp:LinkButton ID="btnFirst" runat="server" CssClass="page-link" OnClick="btnFirst_Click" Text="First" />
                            </li>
                            <li class="page-item">
                                <asp:LinkButton ID="btnPrevious" runat="server" CssClass="page-link" OnClick="btnPrevious_Click" Text="Previous" />
                            </li>
                            <li class="page-item">
                                <asp:LinkButton ID="btnNext" runat="server" CssClass="page-link" OnClick="btnNext_Click" Text="Next" />
                            </li>
                            <li class="page-item">
                                <asp:LinkButton ID="btnLast" runat="server" CssClass="page-link" OnClick="btnLast_Click" Text="Last" />
                            </li>
                        </ul>
                    </div>
                </nav>
            </div>
        </div>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    
    <script type="text/javascript">
        function confirmDelete(customerId) {
            return confirm('Are you sure you want to toggle the status of this customer?');
        }
    </script>
</asp:Content>