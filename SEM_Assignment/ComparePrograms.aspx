<%@ Page Title="Compare Programs" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ComparePrograms.aspx.cs" Inherits="SEM_Assignment.ComparePrograms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2 class="text-2xl font-bold mb-4">Compare Programs</h2>

    <div class="flex">
        <!-- Original Program Details -->
        <div class="bg-white p-6 rounded-lg shadow-md w-1/2">
            <h3 class="text-xl font-semibold mb-2">Selected Program</h3>
            <asp:Label ID="lblOriginalProgramName" runat="server" CssClass="font-bold"></asp:Label>
            <p><strong>Department:</strong> <asp:Label ID="lblOriginalDepartment" runat="server"></asp:Label></p>
            <p><strong>Duration:</strong> <asp:Label ID="lblOriginalDuration" runat="server"></asp:Label></p>
            <p><strong>Credits:</strong> <asp:Label ID="lblOriginalCredits" runat="server"></asp:Label></p>
            <p><strong>Career Opportunities:</strong> <asp:Label ID="lblOriginalCareerOpportunities" runat="server"></asp:Label></p>
            <p><strong>Entry Requirements:</strong> <asp:Label ID="lblOriginalEntryRequirements" runat="server"></asp:Label></p>
            <p><strong>Tuition Fees:</strong> RM <asp:Label ID="lblOriginalTuitionFees" runat="server"></asp:Label></p>
            <p><strong>Additional Fees:</strong> RM <asp:Label ID="lblOriginalAdditionalFees" runat="server"></asp:Label></p>
            <p><strong>Total Estimated Cost:</strong> RM <asp:Label ID="lblOriginalTotalEstimatedCost" runat="server"></asp:Label></p>
            <p><strong>Mode of Study:</strong> <asp:Label ID="lblOriginalModeOfStudy" runat="server"></asp:Label></p>
            <p><strong>Accreditation:</strong> <asp:Label ID="lblOriginalAccreditation" runat="server"></asp:Label></p>
        </div>

        <!-- Comparison Program Selection -->
        <div class="bg-white p-6 rounded-lg shadow-md w-1/2 ml-4">
            <h3 class="text-xl font-semibold mb-2">Select Program to Compare</h3>
            <asp:DropDownList ID="ddlComparisonPrograms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlComparisonPrograms_SelectedIndexChanged">
            </asp:DropDownList>

            <div class="bg-white p-4 rounded-lg shadow-md">
                <p><strong>Program Name:</strong> <asp:Label ID="lblComparisonProgramName" runat="server"></asp:Label></p>
                <p><strong>Department:</strong> <asp:Label ID="lblComparisonDepartment" runat="server"></asp:Label></p>
                <p><strong>Duration:</strong> <asp:Label ID="lblComparisonDuration" runat="server"></asp:Label></p>
                <p><strong>Credits:</strong> <asp:Label ID="lblComparisonCredits" runat="server"></asp:Label></p>
                <p><strong>Career Opportunities:</strong> <asp:Label ID="lblComparisonCareerOpportunities" runat="server"></asp:Label></p>
                <p><strong>Entry Requirements:</strong> <asp:Label ID="lblComparisonEntryRequirements" runat="server"></asp:Label></p>
                <p><strong>Tuition Fees:</strong> RM <asp:Label ID="lblComparisonTuitionFees" runat="server"></asp:Label></p>
                <p><strong>Additional Fees:</strong> RM <asp:Label ID="lblComparisonAdditionalFees" runat="server"></asp:Label></p>
                <p><strong>Total Estimated Cost:</strong> RM <asp:Label ID="lblComparisonTotalEstimatedCost" runat="server"></asp:Label></p>
                <p><strong>Mode of Study:</strong> <asp:Label ID="lblComparisonModeOfStudy" runat="server"></asp:Label></p>
                <p><strong>Accreditation:</strong> <asp:Label ID="lblComparisonAccreditation" runat="server"></asp:Label></p>
            </div>
        </div>
    </div>

    <!-- Button to compare selected programs -->
    <div class="mt-6 text-center">
        <asp:Button ID="btnCompareSelected" runat="server" Text="Compare Selected Programs" CssClass="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition duration-300" OnClick="btnCompareSelected_Click" />
    </div>

    <!-- Comparison Results Section -->
    <asp:Panel ID="pnlComparisonResults" runat="server" Visible="false">
        <h3 class="text-lg font-bold mb-2">Comparison Results</h3>
        <div class="bg-white p-4 rounded-lg shadow-md">
            <h4 class="font-bold">Differences</h4>
            <asp:Literal ID="litComparisonResults" runat="server"></asp:Literal>
        </div>
    </asp:Panel>
</asp:Content>
