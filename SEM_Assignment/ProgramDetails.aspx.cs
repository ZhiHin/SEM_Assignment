using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEM_Assignment
{
    public partial class ProgramDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string programId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(programId))
                {
                    LoadProgramDetails(int.Parse(programId));
                }
            }
        }

        private void LoadProgramDetails(int programId)
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
                    // Bind the program details to the page
                    lblProgramName.Text = reader["ProgramName"].ToString();
                    lblDepartment.Text = reader["Department"].ToString();
                    lblDuration.Text = reader["Duration"].ToString();
                    lblCredits.Text = reader["Credits"].ToString();
                    lblCareerOpportunities.Text = reader["CareerOpportunities"].ToString();
                    lblEntryRequirements.Text = reader["EntryRequirements"].ToString();
                    lblTuitionFees.Text = reader["TuitionFees"].ToString();
                    lblAdditionalFees.Text = reader["AdditionalFees"] != DBNull.Value ? reader["AdditionalFees"].ToString() : "0.00"; // Handle null case
                    lblTotalEstimatedCost.Text = reader["TotalEstimatedCost"].ToString();
                    lblModeOfStudy.Text = reader["ModeOfStudy"].ToString();
                    lblAccreditation.Text = reader["Accreditation"].ToString();
                }
            }
        }

        protected void btnCompare_Click(object sender, EventArgs e)
        {
            string programId = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(programId))
            {
                Response.Redirect($"ComparePrograms.aspx?selectedId={programId}");
            }
        }
        protected void btnCalc_Click(object sender, EventArgs e)
        {
            string programId = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(programId))
            {
                Response.Redirect($"FeesCalculator.aspx?selectedId={programId}");
            }
        }
    }
}