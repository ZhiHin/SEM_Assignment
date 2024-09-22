using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEM_Assignment
{
    public partial class CancelAppoinment : System.Web.UI.Page
    {

        static string[] Scopes = { CalendarService.Scope.Calendar };
        static string ApplicationName = "SEM_Assignment";
        private static string verificationCode;  // Store the generated verification code
        private static DateTime verificationCodeGeneratedTime; // Store the time when the code was generated

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Hide the verification input field and the resend button initially
                //verifyCodeDiv.Visible = false;
                btnRequestNewCode.Visible = false;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT AppointmentID FROM Appointments WHERE BookedByName = @Name AND BookedByEmail = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);

                    string appointmentID = cmd.ExecuteScalar() as string;

                    if (appointmentID != null)
                    {
                        // Generate and send verification code
                        verificationCode = GenerateVerificationCode();
                        verificationCodeGeneratedTime = DateTime.Now; // Store the current time
                        SendVerificationCode(email, verificationCode);

                        lblMessage.Text = "A verification code has been sent to your email.";
                        lblMessage.CssClass = "text-green-500"; // Change to green for success
                        lblMessage.Visible = true;

                        // Disable the submit button
                        btnSubmit.Visible = false;

                        // Show verification input field and resend button
                        //verifyCodeDiv.Visible = true;
                        btnRequestNewCode.Visible = true; // Initially hidden

                        // Show verification input field
                        ClientScript.RegisterStartupScript(this.GetType(), "ShowVerifyCodeDiv", "document.getElementById('verifyCodeDiv').style.display = 'block'; initiateVerificationCode();", true);

                    }
                    else
                    {
                        lblMessage.Text = "No appointment found for the provided name and email.";
                        lblMessage.Visible = true;
                    }
                }
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string enteredCode = txtVerificationCode.Text.Trim();
            TimeSpan timeElapsed = DateTime.Now - verificationCodeGeneratedTime; // Calculate time elapsed since generation


            if (enteredCode == verificationCode && timeElapsed.TotalSeconds < 30)
            {
                string name = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string query = "SELECT AppointmentID FROM Appointments WHERE BookedByName = @Name AND BookedByEmail = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Email", email);

                        string appointmentID = cmd.ExecuteScalar() as string;

                        if (appointmentID != null)
                        {
                            CancelGoogleCalendarEvent(appointmentID);
                            CancelAppointmentInDatabase(appointmentID);

                            lblMessage.Text = "Your appointment has been successfully canceled.";
                            lblMessage.CssClass = "text-green-500"; // Change to green for success
                            lblMessage.Visible = true;

                            // Clear input fields
                            txtName.Text = "";
                            txtEmail.Text = "";
                            txtVerificationCode.Text = "";

                            btnSubmit.Visible = true;
                        }
                        else
                        {
                            lblMessage.Text = "No appointment found.";
                            lblMessage.CssClass = "text-red-500"; // Red for error
                            lblMessage.Visible = true;
                        }
                    }
                }
            }
            else
            {
                lblMessage.Text = timeElapsed.TotalSeconds >= 30
                    ? "The verification code has expired. Please request a new one."
                    : "Invalid verification code. Please try again.";
                lblMessage.CssClass = "text-red-500"; // Red for error
                lblMessage.Visible = true;

                // Show the request new code button if expired
                if (timeElapsed.TotalSeconds >= 30)
                {
                    btnRequestNewCode.Visible = true; // Show the button to request a new code
                }

                // Clear input fields
                txtName.Text = "";
                txtEmail.Text = "";
                txtVerificationCode.Text = "";

                // Show the input fields and submit button again
                txtName.Visible = true;
                txtEmail.Visible = true;
                btnSubmit.Visible = true; 
            }
        }

        protected void btnRequestNewCode_Click(object sender, EventArgs e)
        {
            // Call the same method to generate and send a new verification code
            btnSubmit_Click(sender, e);
        }

        private void SendVerificationCode(string email, string code)
        {
            // Send the code via email
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("foongzh-wm21@student.tarc.edu.my");
            mail.Subject = "Appointment Cancellation Code";
            mail.Body = "Your verification code is: " + code;

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",  // Your SMTP server
                Port = 587,
                Credentials = new System.Net.NetworkCredential("foongzh-wm21@student.tarc.edu.my", "damd zvvf qlzb csyq"),
                EnableSsl = true
            };

            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Failed to send email: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        private string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();  // Generate a 6-digit code
        }

        private void CancelAppointmentInDatabase(string appointmentID)
        {
            string slotID;

            // First, retrieve the SlotID from the appointment
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                string selectQuery = "SELECT SlotID FROM Appointments WHERE AppointmentID = @AppointmentID";

                using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                    slotID = cmd.ExecuteScalar() as string; // Retrieve the SlotID
                }
            }

            // Now delete the appointment
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                    cmd.ExecuteNonQuery();
                }
            }

            // Finally, update the AvailableSlots table to set IsBooked = 0
            if (!string.IsNullOrEmpty(slotID))
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE AvailableSlots SET IsBooked = 0 WHERE SlotID = @SlotID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@SlotID", slotID);
                        cmd.ExecuteNonQuery(); // Update the slot to be available
                    }
                }
            }
        }

        private void CancelGoogleCalendarEvent(string appointmentID)
        {
            string eventId;
            System.Diagnostics.Debug.WriteLine($"Retrieved appointmentID: {appointmentID}");

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
            {
                conn.Open();
                string query = "SELECT GoogleEventID FROM Appointments WHERE AppointmentID = @AppointmentID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppointmentID", appointmentID);
                    eventId = cmd.ExecuteScalar() as string;
                }
            }

            // Log to see if eventId was retrieved
            System.Diagnostics.Debug.WriteLine($"Retrieved eventId: {eventId}");

            if (string.IsNullOrEmpty(eventId))
            {
                lblMessage.Text = "No Google Calendar event associated with this appointment.";
                lblMessage.Visible = true;
                return;
            }

            // Load client secrets
            UserCredential credential;
            string credentialsPath = Server.MapPath("~/Json/credentials.json");
            string tokenPath = Server.MapPath("~/Json/token.json");

            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(tokenPath)).Result;
            }

            // Create Google Calendar API service
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            try
            {
                // Log eventId before deletion
                System.Diagnostics.Debug.WriteLine($"Attempting to delete event with ID: {eventId}");

                // Delete the event from Google Calendar
                service.Events.Delete("primary", eventId).Execute();

                // Success message
                lblMessage.Text = "Event successfully deleted from Google Calendar.";
                lblMessage.Visible = true;
            }
            catch (Google.GoogleApiException ex)
            {
                // Fail message with specific error details
                lblMessage.Text = $"Failed to delete event from Google Calendar: {ex.Message}";
                lblMessage.Visible = true;
                // Log the exception details for further investigation
                System.Diagnostics.Debug.WriteLine($"Error deleting event: {ex.Message}");
            }
            catch (Exception ex)
            {
                // General fail message
                lblMessage.Text = $"An error occurred: {ex.Message}";
                lblMessage.Visible = true;
                // Log the general exception details
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}