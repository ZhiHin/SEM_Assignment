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

    <!-- IDE Playground -->
    <section class="bg-gray-100 py-12">
        <div class="max-w-7xl mx-auto">
            <h1 class="text-4xl font-semibold text-center text-gray-800 mb-8">Hands-On Learning at TARUMT</h1>
            <p class="text-center text-gray-600 mb-12">
                At the Faculty of Computing and Information Technology, we believe in learning by doing. Experience the basics of web development firsthand through our interactive Web Development IDE.
            </p>

            <div class="containerIde bg-white shadow-md rounded-lg p-8">
                <div class="ide-header mb-6">
                    <h2 class="text-2xl font-bold text-gray-900 mb-4">TARUMT Web Development Playground</h2>
                    <p class="text-gray-700">Edit the HTML, CSS, and JavaScript below to see instant results in the output section.</p>
                </div>

                <div class="editor-container grid grid-cols-1 md:grid-cols-2 gap-8">
                    <!-- Code Editors -->
                    <div class="left space-y-4">
                        <label for="html-code" class="text-lg font-medium text-gray-800">HTML</label>
                        <textarea id="html-code" class="w-full p-3 border border-gray-300 rounded-lg" placeholder="Enter your HTML here"></textarea>

                        <label for="css-code" class="text-lg font-medium text-gray-800">CSS</label>
                        <textarea id="css-code" class="w-full p-3 border border-gray-300 rounded-lg" placeholder="Enter your CSS here"></textarea>

                        <label for="js-code" class="text-lg font-medium text-gray-800">JavaScript</label>
                        <textarea id="js-code" class="w-full p-3 border border-gray-300 rounded-lg" placeholder="Enter your JavaScript here"></textarea>

                        <button type="button" onclick="run()" class="w-full bg-blue-600 text-white font-semibold py-3 rounded-lg hover:bg-blue-700 transition">Run Code</button>
                    </div>

                    <!-- Output Display -->
                    <div class="right">
                        <label class="text-lg font-medium text-gray-800">Output</label>
                        <iframe id="output" class="w-full h-96 border border-gray-300 rounded-lg"></iframe>
                    </div>
                </div>

                <!-- Tutorial Section -->
                <div class="tutorial mt-8">
                    <h2 class="text-xl font-semibold text-gray-900">Quick Start Tutorial</h2>
                    <p class="text-gray-700">1. In the HTML section, try adding: <code>&lt;h1&gt;Hello, World!&lt;/h1&gt;</code></p>
                    <p class="text-gray-700">2. In the CSS section, add: <code>h1 { color: blue; }</code></p>
                    <p class="text-gray-700">3. In the JavaScript section, try: <code>document.querySelector('h1').textContent = "Welcome to web development !";</code></p>
                    <p class="text-gray-700">4. Click "Run Code" to see your changes in action!</p>
                </div>
            </div>
        </div>
    </section>

    <script>
        function run() {
            let htmlCode = document.getElementById("html-code").value;
            let cssCode = document.getElementById("css-code").value;
            let jsCode = document.getElementById("js-code").value;
            let output = document.getElementById("output");

            output.contentDocument.body.innerHTML = htmlCode + "<style>" + cssCode + "</style>";
            output.contentWindow.eval(jsCode);
        }

        // Run the code initially to show the default output
        run();

        // Navbar scroll effect
        window.addEventListener('scroll', function () {
            var navbar = document.getElementById('navbar');
            if (window.scrollY > 50) {
                navbar.classList.add('navbar-scrolled');
            } else {
                navbar.classList.remove('navbar-scrolled');
            }
        });
    </script>
</asp:Content>
