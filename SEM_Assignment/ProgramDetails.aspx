<%@ Page Title="Program Details" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ProgramDetails.aspx.cs" Inherits="SEM_Assignment.ProgramDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="text-2xl font-bold mb-4">Program Details</h2>
    
    <!-- Program details here -->
    <div class="bg-white p-6 rounded-lg shadow-md">
        
        <asp:Label ID="lblProgramName" runat="server"></asp:Label>
        <p><strong>Department:</strong> <asp:Label ID="lblDepartment" runat="server"></asp:Label></p>
        <p><strong>Duration:</strong> <asp:Label ID="lblDuration" runat="server"></asp:Label></p>
        <p><strong>Credits:</strong> <asp:Label ID="lblCredits" runat="server"></asp:Label></p>
        <p><strong>Career Opportunities:</strong> <asp:Label ID="lblCareerOpportunities" runat="server"></asp:Label></p>
        <p><strong>Entry Requirements:</strong> <asp:Label ID="lblEntryRequirements" runat="server"></asp:Label></p>
        <p><strong>Tuition Fees:</strong> RM <asp:Label ID="lblTuitionFees" runat="server"></asp:Label></p>

        <!-- Option to compare this program with others -->
        <div class="mt-6 text-center">
            <asp:Button ID="btnCompare" runat="server" Text="Compare with Other Programs" CssClass="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition duration-300" OnClick="btnCompare_Click" />
            <asp:Button ID="btnCalc" runat="server" Text="Calculate Fees" CssClass="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition duration-300" OnClick="btnCalc_Click" />
        </div>
    </div>
</asp:Content>
