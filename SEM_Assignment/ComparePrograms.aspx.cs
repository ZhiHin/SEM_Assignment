using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEM_Assignment
{
    public partial class ComparePrograms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string programId = Request.QueryString["selectedId"];
                if (!string.IsNullOrEmpty(programId))
                {
                    LoadOriginalProgramDetails(int.Parse(programId));
                    LoadComparisonPrograms();
                }
            }
        }

        private void LoadOriginalProgramDetails(int programId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Programs WHERE ProgramId = @ProgramId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProgramId", programId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    lblOriginalProgramName.Text = reader["ProgramName"].ToString();
                    lblOriginalDepartment.Text = reader["Department"].ToString();
                    lblOriginalDuration.Text = reader["Duration"].ToString();
                    lblOriginalCredits.Text = reader["Credits"].ToString();
                    lblOriginalCareerOpportunities.Text = reader["CareerOpportunities"].ToString();
                    lblOriginalEntryRequirements.Text = reader["EntryRequirements"].ToString();
                    lblOriginalTuitionFees.Text = reader["TuitionFees"].ToString();
                    lblOriginalAdditionalFees.Text = reader["AdditionalFees"].ToString(); 
                    lblOriginalTotalEstimatedCost.Text = reader["TotalEstimatedCost"].ToString(); 
                    lblOriginalModeOfStudy.Text = reader["ModeOfStudy"].ToString(); 
                    lblOriginalAccreditation.Text = reader["Accreditation"].ToString(); 
                }
            }
        }

        private void LoadComparisonPrograms()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string originalProgramId = Request.QueryString["selectedId"]; // Get the original program ID

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Exclude the original program from the dropdown list
                string query = "SELECT ProgramId, ProgramName FROM Programs WHERE ProgramId != @OriginalProgramId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OriginalProgramId", originalProgramId); // Exclude the original program

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dtPrograms = new DataTable();
                adapter.Fill(dtPrograms);

                ddlComparisonPrograms.DataSource = dtPrograms;
                ddlComparisonPrograms.DataTextField = "ProgramName";
                ddlComparisonPrograms.DataValueField = "ProgramId";
                ddlComparisonPrograms.DataBind();

                // Optionally, add a default item
                ddlComparisonPrograms.Items.Insert(0, new ListItem("Select a program", ""));
            }
        }

        protected void ddlComparisonPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected program ID from the dropdown
            string selectedProgramId = ddlComparisonPrograms.SelectedValue;

            // Check if a valid program is selected
            if (!string.IsNullOrEmpty(selectedProgramId))
            {
                // Load the details of the selected comparison program
                LoadComparisonProgramDetails(int.Parse(selectedProgramId));
            }
            else
            {
                // Clear the comparison program details if no valid selection is made
                ClearComparisonProgramDetails();
            }
        }

        private void LoadComparisonProgramDetails(int programId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Programs WHERE ProgramId = @ProgramId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProgramId", programId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    lblComparisonProgramName.Text = reader["ProgramName"].ToString();
                    lblComparisonDepartment.Text = reader["Department"].ToString();
                    lblComparisonDuration.Text = reader["Duration"].ToString();
                    lblComparisonCredits.Text = reader["Credits"].ToString();
                    lblComparisonCareerOpportunities.Text = reader["CareerOpportunities"].ToString();
                    lblComparisonEntryRequirements.Text = reader["EntryRequirements"].ToString();
                    lblComparisonTuitionFees.Text = reader["TuitionFees"].ToString();
                    lblComparisonAdditionalFees.Text = reader["AdditionalFees"].ToString(); 
                    lblComparisonTotalEstimatedCost.Text = reader["TotalEstimatedCost"].ToString(); 
                    lblComparisonModeOfStudy.Text = reader["ModeOfStudy"].ToString(); 
                    lblComparisonAccreditation.Text = reader["Accreditation"].ToString(); 
                }
            }
        }

        private void ClearComparisonProgramDetails()
        {
            lblComparisonProgramName.Text = "";
            lblComparisonDepartment.Text = "";
            lblComparisonDuration.Text = "";
            lblComparisonCredits.Text = "";
            lblComparisonCareerOpportunities.Text = "";
            lblComparisonEntryRequirements.Text = "";
            lblComparisonTuitionFees.Text = "";
            lblComparisonAdditionalFees.Text = ""; 
            lblComparisonTotalEstimatedCost.Text = ""; 
            lblComparisonModeOfStudy.Text = ""; 
            lblComparisonAccreditation.Text = ""; 
        }

        protected void btnCompareSelected_Click(object sender, EventArgs e)
        {
            string selectedProgramId = ddlComparisonPrograms.SelectedValue;
            string originalProgramId = Request.QueryString["selectedId"];

            if (!string.IsNullOrEmpty(selectedProgramId) && selectedProgramId != originalProgramId)
            {
                // Load details for both programs
                ProgramDetails originalProgram = LoadProgramDetails(int.Parse(originalProgramId));
                ProgramDetails comparisonProgram = LoadProgramDetails(int.Parse(selectedProgramId));

                // Generate comparison results
                string comparisonResults = GenerateComparisonResults(originalProgram, comparisonProgram);

                // Display results in the comparison results section
                litComparisonResults.Text = comparisonResults;
                pnlComparisonResults.Visible = true; // Show the results section
            }
        }

        private ProgramDetails LoadProgramDetails(int programId)
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            ProgramDetails programDetails = new ProgramDetails();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Programs WHERE ProgramId = @ProgramId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProgramId", programId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    programDetails.ProgramName = reader["ProgramName"].ToString();
                    programDetails.Department = reader["Department"].ToString();
                    programDetails.Duration = reader["Duration"].ToString();
                    programDetails.Credits = reader["Credits"].ToString();
                    programDetails.CareerOpportunities = reader["CareerOpportunities"].ToString();
                    programDetails.EntryRequirements = reader["EntryRequirements"].ToString();
                    programDetails.TuitionFees = reader["TuitionFees"].ToString();
                    programDetails.AdditionalFees = reader["AdditionalFees"].ToString(); 
                    programDetails.TotalEstimatedCost = reader["TotalEstimatedCost"].ToString(); 
                    programDetails.ModeOfStudy = reader["ModeOfStudy"].ToString(); 
                    programDetails.Accreditation = reader["Accreditation"].ToString(); 
                }
            }

            return programDetails;
        }

        private string GenerateComparisonResults(ProgramDetails originalProgram, ProgramDetails comparisonProgram)
        {
            var resultHtml = new System.Text.StringBuilder();

            resultHtml.Append("<table style='width:100%; border-collapse: collapse;'>");
            resultHtml.Append("<tr><th>Field</th><th>Original Program</th><th>Comparison Program</th></tr>");

            // Compare and highlight differences
            AppendComparisonRow(resultHtml, "Program Name", originalProgram.ProgramName, comparisonProgram.ProgramName);
            AppendComparisonRow(resultHtml, "Department", originalProgram.Department, comparisonProgram.Department);
            AppendComparisonRow(resultHtml, "Duration", originalProgram.Duration, comparisonProgram.Duration);
            AppendComparisonRow(resultHtml, "Credits", originalProgram.Credits, comparisonProgram.Credits);
            AppendComparisonRow(resultHtml, "Career Opportunities", originalProgram.CareerOpportunities, comparisonProgram.CareerOpportunities);
            AppendComparisonRow(resultHtml, "Entry Requirements", originalProgram.EntryRequirements, comparisonProgram.EntryRequirements);
            AppendComparisonRow(resultHtml, "Tuition Fees", originalProgram.TuitionFees, comparisonProgram.TuitionFees);
            AppendComparisonRow(resultHtml, "Additional Fees", originalProgram.AdditionalFees, comparisonProgram.AdditionalFees); 
            AppendComparisonRow(resultHtml, "Total Estimated Cost", originalProgram.TotalEstimatedCost, comparisonProgram.TotalEstimatedCost); 
            AppendComparisonRow(resultHtml, "Mode of Study", originalProgram.ModeOfStudy, comparisonProgram.ModeOfStudy); 
            AppendComparisonRow(resultHtml, "Accreditation", originalProgram.Accreditation, comparisonProgram.Accreditation); 

            resultHtml.Append("</table>");

            return resultHtml.ToString();
        }

        private void AppendComparisonRow(System.Text.StringBuilder resultHtml, string fieldName, string originalValue, string comparisonValue)
        {
            resultHtml.Append("<tr>");
            resultHtml.Append($"<td>{fieldName}</td>");

            // If the field is "Program Name", do not apply any highlighting
            if (fieldName == "Program Name")
            {
                resultHtml.Append($"<td>{originalValue}</td>");
                resultHtml.Append($"<td>{comparisonValue}</td>");
            }
            else
            {
                // Highlight original value if it differs from comparison value
                if (originalValue != comparisonValue)
                {
                    resultHtml.Append($"<td style='background-color: lightcoral;'>{originalValue}</td>");
                    resultHtml.Append($"<td style='background-color: lightgreen;'>{comparisonValue}</td>");
                }
                else
                {
                    resultHtml.Append($"<td>{originalValue}</td>");
                    resultHtml.Append($"<td>{comparisonValue}</td>");
                }
            }

            resultHtml.Append("</tr>");
        }

        // Helper class to hold program details
        private class ProgramDetails
        {
            public string ProgramName { get; set; }
            public string Department { get; set; }
            public string Duration { get; set; }
            public string Credits { get; set; }
            public string CareerOpportunities { get; set; }
            public string EntryRequirements { get; set; }
            public string TuitionFees { get; set; }
            public string AdditionalFees { get; set; } 
            public string TotalEstimatedCost { get; set; } 
            public string ModeOfStudy { get; set; } 
            public string Accreditation { get; set; } 
        }
    }
}