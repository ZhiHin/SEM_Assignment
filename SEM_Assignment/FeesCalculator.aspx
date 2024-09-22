<%@ Page Title="Fees Calculator" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="FeesCalculator.aspx.cs" Inherits="SEM_Assignment.FeesCalculator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://cdn.jsdelivr.net/npm/tailwindcss@2.2.19/dist/tailwind.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="text-2xl font-bold text-center mt-10 mb-6">Fees Calculator with Financial Aid Options</h2>
    <div class="container mx-auto p-6 bg-gray-100 rounded-lg shadow-lg max-w-xl">
        <!-- Tuition and Program Cost Details -->
        <h3 class="text-lg font-semibold mb-4">Program Cost Details</h3>
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Course Selected:</label>
            <asp:TextBox ID="txtCourse" runat="server" CssClass="block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Tuition Fee (RM):</label>
            <asp:TextBox ID="txtFee" runat="server" CssClass="block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Select Student Type: </label>
            <asp:DropDownList ID="ddlStudType" runat="server" AutoPostBack="True" CssClass="block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm mb-2" OnSelectedIndexChanged="ddlStudType_SelectedIndexChanged">
                <asp:ListItem Text="Select Student Type" Value="" />
                <asp:ListItem Text="Local" Value="Local" />
                <asp:ListItem Text="International" Value="International" />
            </asp:DropDownList>
        </div>
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Select Financial Status: </label>
            <asp:DropDownList ID="ddlFinancialStatus" runat="server" AutoPostBack="True" CssClass="block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm mb-2">
                <asp:ListItem Text="Select Financial Status" Value="" />
                <asp:ListItem Text="Poor" Value="Poor" />
                <asp:ListItem Text="Middle" Value="Middle" />
            </asp:DropDownList>
        </div>
        <div class="mb-4">
            <label class="block text-sm font-medium text-gray-700 mb-2">Select Grade Type:</label>
            <asp:DropDownList ID="ddlGradeType" runat="server" AutoPostBack="True" CssClass="block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm mb-2">
                <asp:ListItem Text="Select Grade Type" Value="" />
                <asp:ListItem Text="Diploma/Foundation" Value="CGPA" />
                <asp:ListItem Text="A-Levels" Value="A-Level" />
            </asp:DropDownList>
            <div class="flex items-center space-x-2 mb-4">
                <asp:TextBox ID="txtGrade" runat="server" CssClass="flex-grow block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm" placeholder="Enter current grades"></asp:TextBox>
                <asp:Button ID="btnCheck" runat="server" CssClass="bg-blue-500 text-white px-4 py-2 rounded-lg shadow hover:bg-blue-600" Text="Check Eligibility" OnClick="btnCheckEligibility_Click" />
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="bg-gray-100 p-4 rounded-lg"></asp:Label>
            <div class="mb-6">
                <asp:Label ID="lblAim" runat="server" CssClass="block text-sm font-medium text-gray-700 mb-2">Goal CGPA:</asp:Label>
                <asp:TextBox ID="txtAim" runat="server" CssClass="flex-grow block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm"></asp:TextBox>
            </div>
            <div class="flex items-center space-x-2 mb-4">
                <asp:CheckBox ID="chkDisability" runat="server" CssClass="py-2 mr-2" Text="Include Disability" AutoPostBack="True" />
            </div>
            <h3 class="text-lg font-semibold mb-4">Recommended Scholarship</h3>
            <asp:Panel ID="pScholarship" runat="server" CssClass="mb-4">
            </asp:Panel>
        </div>
        <div class="mb-6">
            <asp:Button ID="btnCalc" runat="server" CssClass="w-full bg-blue-500 text-white py-2 rounded-lg shadow hover:bg-blue-600" Text="Calculate" OnClick="btnCalculate_Click" />
        </div>

        <div class="mb-6">
            <asp:Label ID="lblFinalFee" runat="server" CssClass="block text-sm font-medium text-gray-700 mb-2">Final Tuition Fee (RM):</asp:Label>
            <asp:TextBox ID="txtFinalFee" runat="server" CssClass="flex-grow block w-full px-4 py-2 border rounded-lg text-gray-700 bg-white shadow-sm" ReadOnly="true"></asp:TextBox>
            <asp:Label ID="lblAvgFee" runat="server" CssClass="block text-sm font-medium text-gray-700 mb-2"></asp:Label>
            <asp:Label ID="lblFuture" runat="server" CssClass="block text-sm font-medium text-gray-700 mb-2"></asp:Label>
        </div>
    </div>


</asp:Content>
