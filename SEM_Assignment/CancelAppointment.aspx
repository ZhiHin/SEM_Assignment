<%@ Page Title="Cancel Appointment" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="CancelAppointment.aspx.cs" Inherits="SEM_Assignment.CancelAppoinment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="max-w-lg mx-auto mt-10 p-6 bg-white rounded-lg shadow-lg">
        <h2 class="text-2xl font-bold text-center text-gray-700 mb-6">Cancel Appointment</h2>

        <asp:Label ID="lblMessage" runat="server" CssClass="text-red-500"></asp:Label>

        <div class="mb-4">
            <asp:Label ID="lblName" runat="server" Text="Enter Your Name:" CssClass="block text-sm font-medium text-gray-700"></asp:Label>
            <asp:TextBox ID="txtName" runat="server" CssClass="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter your full name"></asp:TextBox>
        </div>

        <div class="mb-4">
            <asp:Label ID="lblEmail" runat="server" Text="Enter Your Email Address:" CssClass="block text-sm font-medium text-gray-700"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter your email"></asp:TextBox>
        </div>

        <div class="mb-6">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="w-full bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2" OnClick="btnSubmit_Click" Visible="true"/>
        </div>

    <!-- Verification Code Section -->
        <div id="verifyCodeDiv" class="hidden" style="display: none;">
            <div class="mb-4">
                <asp:Label ID="lblVerificationCode" runat="server" Text="Enter the Verification Code sent to your email:" CssClass="block text-sm font-medium text-gray-700"></asp:Label>
                <asp:TextBox ID="txtVerificationCode" runat="server" CssClass="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-blue-500 focus:border-blue-500 sm:text-sm" placeholder="Enter the verification code"></asp:TextBox>
            </div>

            <!-- Add a timer label here -->
             <div id="timer" class="text-red-500 font-bold text-center mb-4"></div>

            <div class="mb-6">
                <asp:Button ID="btnVerify" runat="server" Text="Verify & Cancel Appointment" CssClass="w-full bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2" OnClick="btnVerify_Click" />
            </div>

            <div class="mb-6">
                <asp:Button ID="btnRequestNewCode" runat="server" Text="Request New Code" CssClass="w-full bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md focus:outline-none focus:ring-2 focus:ring-gray-400 focus:ring-offset-2" OnClick="btnRequestNewCode_Click" Visible="false" />
            </div>
        </div>
    </div>

   <script type="text/javascript">
       function startTimer(duration, display, btnRequest) {
           var timer = duration, minutes, seconds;
           var countdown = setInterval(function () {
               minutes = parseInt(timer / 60, 10);
               seconds = parseInt(timer % 60, 10);

               minutes = minutes < 10 ? "0" + minutes : minutes;
               seconds = seconds < 10 ? "0" + seconds : seconds;

               display.textContent = minutes + ":" + seconds;

               if (--timer < 0) {
                   clearInterval(countdown);
                   display.textContent = "00:00";
                   btnRequest.style.display = 'block';  // Show the "Request New Code" button when timer expires
               }
           }, 1000);
       }

       // Call this function when the code is sent (from your server-side code)
       function initiateVerificationCode() {
           var time = 30;  // 30 seconds
           var display = document.getElementById('timer');  // Timer label
           var btnRequest = document.getElementById('<%= btnRequestNewCode.ClientID %>');  // Request new code button
           btnRequest.style.display = 'none';  // Hide "Request New Code" button initially

           startTimer(time, display, btnRequest);
       }
</script>
</asp:Content>
