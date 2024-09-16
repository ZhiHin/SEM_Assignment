<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Programmes.aspx.cs" Inherits="SEM_Assignment.Programmes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Add styles in the head section -->
    <style>
        .program-card {
            border: 2px solid #FFD700; /* Gold color border */
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            background-color: #FFF9E5; /* Light yellow background */
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1); /* Subtle shadow */
        }
        .program-card-header {
            background-color: #FFD700;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            padding: 10px;
            color: #000;
            font-weight: bold;
            text-align: center;
        }
        .program-card-body {
            padding: 10px;
        }
        .program-card-body p {
            margin: 5px 0;
        }
        .program-card a {
            display: inline-block;
            margin-top: 10px;
            color: #007bff;
            text-decoration: none;
            font-weight: bold;
        }
        .program-card a:hover {
            text-decoration: underline;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="text-2xl font-bold mb-4">Bachelor Degree</h2>

    <p>FOCS offers the following Bachelor Degree programmes:</p>

    <div class="programs-container">
        <asp:Repeater ID="ProgramsRepeater" runat="server">
            <ItemTemplate>
                <!-- Program card structure -->
                <div class="program-card">
                    <div class="program-card-header">
                        <%# Eval("ProgramName") %>
                    </div>
                    <div class="program-card-body">
                        <p><strong>Department:</strong> <%# Eval("Department") %></p>
                        <p><strong>Duration:</strong> <%# Eval("Duration") %></p>
                        <!-- Link to view entire program details -->
                        <a href='ProgramDetails.aspx?id=<%# Eval("ProgramId") %>'>View Program Details</a>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
