<%@ Page Title="Appointment" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Appointment.aspx.cs" Inherits="SEM_Assignment.Appointment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="max-w-7xl mx-auto p-6 bg-white rounded-lg shadow-md">
        <h1 class="text-2xl font-bold mb-4">Book an Appointment</h1>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-red-500"></asp:Label>

        <div class="mb-4">
            <label for="txtName" class="block text-sm font-medium text-gray-700">Your Name</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="border-gray-300 rounded-md"></asp:TextBox>
        </div>

        <div class="mb-4">
            <label for="txtEmail" class="block text-sm font-medium text-gray-700">Your Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="border-gray-300 rounded-md"></asp:TextBox>
        </div>

        <div class="mb-4">
            <label for="ddlFaculty" class="block text-sm font-medium text-gray-700">Select Faculty</label>
            <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="border-gray-300 rounded-md" AutoPostBack="True" OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged">
                <asp:ListItem Text="--Select Faculty--" Value=""></asp:ListItem>
                
            </asp:DropDownList>
        </div>

        <div class="mb-4">
            <label for="ddlAdvisor" class="block text-sm font-medium text-gray-700">Select Advisor</label>
            <asp:DropDownList ID="ddlAdvisor" runat="server" CssClass="border-gray-300 rounded-md" AutoPostBack="True" OnSelectedIndexChanged="ddlAdvisor_SelectedIndexChanged">
                <asp:ListItem Text="--Select Advisor--" Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="mb-4">
            <label for="ddlTimeSlots" class="block text-sm font-medium text-gray-700">Available Time Slots</label>
            <asp:DropDownList ID="ddlTimeSlots" runat="server" CssClass="border-gray-300 rounded-md">
                <asp:ListItem Text="--Select Time Slot--" Value=""></asp:ListItem>
                
            </asp:DropDownList>
        </div>

        <asp:Button ID="btnBook" runat="server" Text="Book Appointment" CssClass="bg-blue-500 text-white px-4 py-2 rounded-md" OnClick="btnBook_Click" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel Appointment" CssClass="bg-blue-500 text-white px-4 py-2 rounded-md" OnClick="btnCancel_Click" />
    </div>

    <script type="text/javascript">
    window.addEventListener("popstate", function (event) {
        console.log("Current URL: " + window.location.href);
    });

    // Log the URL when the page is loaded
    console.log("Initial URL: " + window.location.href);
</script>
</asp:Content>
