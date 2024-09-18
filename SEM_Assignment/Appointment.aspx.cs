using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Data;
using Google.Apis.Auth.OAuth2.Flows;

namespace SEM_Assignment
{
    public partial class Appointment : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadFaculty();
                // Handle the callback if there's a code in the query string
                if (!string.IsNullOrEmpty(Request.QueryString["code"]))
                {
                    HandleGoogleCallback();
                }  
            }
        }

        private void LoadFaculty()
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT FacultyID, FacultyName FROM Faculty", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                ddlFaculty.DataSource = reader;
                ddlFaculty.DataTextField = "FacultyName";
                ddlFaculty.DataValueField = "FacultyID";
                ddlFaculty.DataBind();
                ddlFaculty.Items.Insert(0, new ListItem("--Select Faculty--", ""));
            }
        }

        protected void ddlFaculty_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlFaculty.SelectedValue))
            {
                LoadAdvisors(ddlFaculty.SelectedValue);
            }
            else
            {
                ddlAdvisor.Items.Clear();
                ddlAdvisor.Items.Insert(0, new ListItem("--Select Advisor--", ""));
                ddlTimeSlots.Items.Clear();
                ddlTimeSlots.Items.Insert(0, new ListItem("--Select Time Slot--", ""));
            }
        }

        private void LoadAdvisors(string facultyId)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT AdvisorID, AdvisorName FROM Advisor WHERE FacultyID = @FacultyID", conn);
                cmd.Parameters.AddWithValue("@FacultyID", facultyId);
                SqlDataReader reader = cmd.ExecuteReader();
                ddlAdvisor.DataSource = reader;
                ddlAdvisor.DataTextField = "AdvisorName";
                ddlAdvisor.DataValueField = "AdvisorID";
                ddlAdvisor.DataBind();
                ddlAdvisor.Items.Insert(0, new ListItem("--Select Advisor--", ""));
            }
        }

        protected void ddlAdvisor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlAdvisor.SelectedValue))
            {
                LoadAvailableSlots(ddlFaculty.SelectedValue, ddlAdvisor.SelectedValue);
            }
            else
            {
                ddlTimeSlots.Items.Clear();
                ddlTimeSlots.Items.Insert(0, new ListItem("--Select Time Slot--", ""));
            }
        }

        private void LoadAvailableSlots(string facultyId, string advisorId)
        {
            string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                // Log to verify the inputs
                System.Diagnostics.Debug.WriteLine($"Loading slots for FacultyID: {facultyId}, AdvisorID: {advisorId}");

                SqlCommand cmd = new SqlCommand("SELECT SlotID, StartDateTime, EndDateTime FROM AvailableSlots WHERE FacultyID = @FacultyID AND AdvisorID = @AdvisorID AND IsBooked = 0", conn);
                cmd.Parameters.AddWithValue("@FacultyID", facultyId);
                cmd.Parameters.AddWithValue("@AdvisorID", advisorId);

                SqlDataReader reader = cmd.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Load(reader);

                // Create a new column to hold the formatted time range
                dt.Columns.Add("TimeRange", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    DateTime startDateTime = Convert.ToDateTime(row["StartDateTime"]);
                    DateTime endDateTime = Convert.ToDateTime(row["EndDateTime"]);
                    row["TimeRange"] = $"{startDateTime:yyyy-MM-dd HH:mm} - {endDateTime:HH:mm}";
                }

                ddlTimeSlots.DataSource = dt;
                ddlTimeSlots.DataTextField = "TimeRange"; // Use the new column for display
                ddlTimeSlots.DataValueField = "SlotID";
                ddlTimeSlots.DataBind();

                ddlTimeSlots.Items.Insert(0, new ListItem("--Select Time Slot--", ""));
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlTimeSlots.SelectedValue))
            {
                lblMessage.Text = "Please select a time slot.";
                return;
            }

            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtEmail.Text))
            {
                lblMessage.Text = "Please enter your name and email.";
                return;
            }

            // Store the user input in session
            Session["UserName"] = txtName.Text;
            Session["UserEmail"] = txtEmail.Text;
            Session["SelectedAdvisor"] = ddlAdvisor.SelectedValue;
            Session["SelectedSlot"] = ddlTimeSlots.SelectedValue;

            // Check if user is authenticated
            if (Session["UserCredential"] == null)
            {
                // Redirect to Google sign-in and return after the code is exchanged
                var authUrl = GetAuthorizationUrl();
                Response.Redirect(authUrl);
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                string appointmentId = GenerateAppointmentId();
                SqlCommand cmd = new SqlCommand("INSERT INTO Appointments (AppointmentID, AdvisorID, BookedByName, BookedByEmail, SlotID) VALUES (@AppointmentID, @AdvisorID, @BookedByName, @BookedByEmail, @SlotID)", conn);
                cmd.Parameters.AddWithValue("@AppointmentID", appointmentId);
                cmd.Parameters.AddWithValue("@AdvisorID", ddlAdvisor.SelectedValue);
                cmd.Parameters.AddWithValue("@BookedByName", txtName.Text);
                cmd.Parameters.AddWithValue("@BookedByEmail", txtEmail.Text);
                cmd.Parameters.AddWithValue("@SlotID", ddlTimeSlots.SelectedValue);
                cmd.ExecuteNonQuery();

                // Update slot as booked
                SqlCommand updateCmd = new SqlCommand("UPDATE AvailableSlots SET IsBooked = 1 WHERE SlotID = @SlotID", conn);
                updateCmd.Parameters.AddWithValue("@SlotID", ddlTimeSlots.SelectedValue);
                updateCmd.ExecuteNonQuery();

                // Create Google Calendar Event
                System.Diagnostics.Debug.WriteLine("Create Event.");
                CreateGoogleCalendarEvent(appointmentId, ddlTimeSlots.SelectedValue);

                lblMessage.Text = "Appointment booked successfully!";

                // Clear session values after booking
                Session.Remove("UserName");
                Session.Remove("UserEmail");
                Session.Remove("SelectedAdvisor");
                Session.Remove("SelectedSlot");
            }
        }

        private void CreateGoogleCalendarEvent(string appointmentId, string slotId)
        {
            try
            {
                string[] Scopes = { CalendarService.Scope.Calendar };
                string applicationName = "SEM_Assignment"; // Replace with your application name

                UserCredential credential;

                // Use Server.MapPath to get the correct path
                string credentialsPath = Server.MapPath("~/Json/credentials.json");
                string tokenPath = Server.MapPath("~/Json/token.json");

                using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(tokenPath, true)).Result;
                }

                // Create Google Calendar API service
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = applicationName,
                });

                // Fetch the time slot details for the event
                DateTime startDateTime, endDateTime;
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT StartDateTime, EndDateTime FROM AvailableSlots WHERE SlotID = @SlotID", conn);
                    cmd.Parameters.AddWithValue("@SlotID", slotId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            startDateTime = Convert.ToDateTime(reader["StartDateTime"]);
                            endDateTime = Convert.ToDateTime(reader["EndDateTime"]);
                        }
                        else
                        {
                            throw new Exception("Slot not found.");
                        }
                    }
                }

                // Create event details
                Event newEvent = new Event()
                {
                    Summary = $"Appointment with Advisor {ddlAdvisor.SelectedItem.Text}",
                    Location = "University Office",
                    Description = $"Appointment ID: {appointmentId}\nBooked by: {txtName.Text}\nEmail: {txtEmail.Text}",
                    Start = new EventDateTime()
                    {
                        DateTimeDateTimeOffset = startDateTime,
                        TimeZone = "Asia/Kuala_Lumpur",
                    },
                    End = new EventDateTime()
                    {
                        DateTimeDateTimeOffset = endDateTime,
                        TimeZone = "Asia/Kuala_Lumpur",
                    },
                    Reminders = new Event.RemindersData()
                    {
                        UseDefault = false,
                        Overrides = new EventReminder[] {
                    new EventReminder() { Method = "email", Minutes = 10 },
                    new EventReminder() { Method = "popup", Minutes = 10 },
                }
                    }
                };

                // Insert the event into the calendar
                String calendarId = "primary";
                EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
                System.Diagnostics.Debug.WriteLine("Attempting to create calendar event.");
                request.Execute();
                System.Diagnostics.Debug.WriteLine("Event created successfully.");
            }
            catch (Exception ex)
            {
                // Log the error (you can use a logging framework here)
                System.Diagnostics.Debug.WriteLine($"Error creating Google Calendar event: {ex.Message}");
                lblMessage.Text = "An error occurred while creating the appointment in Google Calendar.";
            }
        }

        private void HandleGoogleCallback()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                string code = Request.QueryString["code"];
                var credential = ExchangeAuthorizationCodeForToken(code);

                // Restore user inputs from session (if they are cleared)
                if (Session["UserName"] != null)
                {
                    txtName.Text = Session["UserName"].ToString();
                }

                if (Session["UserEmail"] != null)
                {
                    txtEmail.Text = Session["UserEmail"].ToString();
                }

                if (Session["SelectedAdvisor"] != null)
                {
                    ddlAdvisor.SelectedValue = Session["SelectedAdvisor"].ToString();
                    LoadAvailableSlots(ddlFaculty.SelectedValue, ddlAdvisor.SelectedValue); // Load time slots
                }

                if (Session["SelectedSlot"] != null)
                {
                    ddlTimeSlots.SelectedValue = Session["SelectedSlot"].ToString();
                }

                // Store the credential in session for later use
                Session["UserCredential"] = credential;
            }
        }

        private UserCredential ExchangeAuthorizationCodeForToken(string code)
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = "803652464494-4drca0n2olkik9tq1pfv119ervqa17kg.apps.googleusercontent.com",
                ClientSecret = "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",
            };

            var authorizationCodeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] { CalendarService.Scope.Calendar },
                DataStore = new FileDataStore(Server.MapPath("~/Json/token.json"), true)
            });

            // Ensure the redirect URI matches what you set in Google Cloud Console
            var redirectUri = "https://localhost:44316/Appointment.aspx/"; // Update if necessary

            // Exchange code for access token
            var tokenResponse = authorizationCodeFlow.ExchangeCodeForTokenAsync(
                "user", code, redirectUri, CancellationToken.None).Result;

            return new UserCredential(authorizationCodeFlow, "user", tokenResponse);
        }

        private string GetAuthorizationUrl()
        {
            var clientSecrets = new ClientSecrets
            {
                ClientId = "803652464494-4drca0n2olkik9tq1pfv119ervqa17kg.apps.googleusercontent.com",
                ClientSecret = "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",
            };

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = clientSecrets,
                Scopes = new[] { CalendarService.Scope.Calendar }
            });

            // Ensure this URI matches the one registered in Google Cloud Console
            var redirectUri = "https://localhost:44316/Appointment.aspx/"; // Change to match your registered URI

            var authorizationUrl = flow.CreateAuthorizationCodeRequest(redirectUri).Build().AbsoluteUri;

            return authorizationUrl;
        }

        private string GenerateAppointmentId()
        {
            // Generate a unique appointment ID (you can customize this logic)
            return Guid.NewGuid().ToString();
        }
    }
}