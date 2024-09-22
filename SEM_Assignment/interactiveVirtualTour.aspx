<%@ Page Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" CodeBehind="interactiveVirtualTour.aspx.cs" Inherits="SEM_Assignment.interactiveVirtualTour" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Hero Section -->
    <div class="relative bg-cover bg-center h-96" style="background-image: url('path_to_your_image.jpg');">
        <div class="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
            <h1 class="text-4xl font-bold text-yellow-500">Explore Our FOCS Facilities</h1>
        </div>
    </div>

    <!-- Information Section: Virtual Tour -->
    <section class="bg-white py-10">
        <div class="max-w-7xl mx-auto text-center">
            <h2 class="text-2xl font-semibold text-gray-800">Virtual Tour of Our FOCS Facilities</h2>
            <p class="mt-5 text-gray-600">Experience an interactive virtual tour that showcases the state-of-the-art facilities of the Faculty of Computing and Information Technology (FOCS). Navigate through our advanced computer labs, innovative research spaces, and collaborative study areas.</p>
            
            <!-- Virtual Tour Embed -->
            <iframe id="tour-embeded" name="TARUMT CITC" src="https://tour.panoee.net/iframe/66efa413d9f161567654f9be" frameBorder="0" scrolling="no" allowvr="yes" allow="vr; xr; accelerometer; gyroscope; autoplay;" allowFullScreen="false" style="width: 100%; height: 500px;"></iframe>
            <script>
                var pano_iframe_name = "tour-embeded";
                window.addEventListener("devicemotion", function (e) {
                    var iframe = document.getElementById(pano_iframe_name);
                    if (iframe) iframe.contentWindow.postMessage({ type: "devicemotion", deviceMotionEvent: { acceleration: { x: e.acceleration.x, y: e.acceleration.y, z: e.acceleration.z }, accelerationIncludingGravity: { x: e.accelerationIncludingGravity.x, y: e.accelerationIncludingGravity.y, z: e.accelerationIncludingGravity.z }, rotationRate: { alpha: e.rotationRate.alpha, beta: e.rotationRate.beta, gamma: e.rotationRate.gamma }, interval: e.interval, timeStamp: e.timeStamp } }, "*");
                });
            </script>
        </div>
    </section>
    
    <!-- Google Street View Section -->
    <section class="bg-gray-100 py-10">
        <div class="max-w-7xl mx-auto text-center">
            <h2 class="text-2xl font-semibold text-gray-800">Explore Our Surroundings with Google Street View</h2>
            <p class="mt-5 text-gray-600">Take a look at the surroundings of our faculty and see the vibrant campus life and facilities nearby through Google Street View.</p>
          <iframe src="https://www.google.com/maps/embed?pb=!4v1726987459836!6m8!1m7!1st6NRpu_H2m3BW4rBp65ruA!2m2!1d3.213950482397468!2d101.7270496355752!3f295.99828842778317!4f8.214169310242184!5f0.7820865974627469" style="width: 100%; height: 500px; border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>
        </div>
    </section>
</asp:Content>
