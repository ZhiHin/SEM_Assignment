<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SEM_Assignment.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Hero Section -->
    <div class="relative bg-cover bg-center h-96" style="background-image: url('path_to_your_image.jpg');">
        <div class="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
            <h1 class="text-4xl font-bold text-yellow-500">Faculty of Computing and Information Technology</h1>
        </div>
    </div>

    <!-- Information Section: "FOCS at a Glance" -->
    <section class="bg-white py-10">
        <div class="max-w-7xl mx-auto text-center">
            <h2 class="text-2xl font-semibold text-gray-800">FOCS at a Glance</h2>
            <div class="grid grid-cols-2 md:grid-cols-4 gap-10 mt-10">
                <!-- Item 1 -->
                <div>
                    <h3 class="text-4xl font-bold text-gray-900">1972</h3>
                    <p class="text-gray-500">Founded</p>
                </div>
                <!-- Item 2 -->
                <div>
                    <h3 class="text-4xl font-bold text-gray-900">5</h3>
                    <p class="text-gray-500">Departments</p>
                </div>
                <!-- Item 3 -->
                <div>
                    <h3 class="text-4xl font-bold text-gray-900">3500+</h3>
                    <p class="text-gray-500">Active Students</p>
                </div>
                <!-- Item 4 -->
                <div>
                    <h3 class="text-4xl font-bold text-gray-900">16</h3>
                    <p class="text-gray-500">Programmes</p>
                </div>
                <!-- Additional Items (if needed) -->
                <div>
                    <h3 class="text-4xl font-bold text-gray-900">6</h3>
                    <p class="text-gray-500">Research Centres</p>
                </div>
                <div>
                    <h3 class="text-4xl font-bold text-gray-900">2</h3>
                    <p class="text-gray-500">Centres of Excellence</p>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
