using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEM_Assignment
{
    public partial class FeesCalculator : System.Web.UI.Page
    {
        double discount
        {
            get { return ViewState["Discount"] != null ? (double)ViewState["Discount"] : 0; }
            set { ViewState["Discount"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                txtFinalFee.Visible = false;
                lblFinalFee.Visible = false;
                discount = 0;
                string programId = Request.QueryString["selectedId"];
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
                    txtCourse.Text = reader["ProgramName"].ToString();
                    txtFee.Text = reader["TuitionFees"].ToString();
                }
            }
        }

        protected void ddlGradeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGradeType = ddlGradeType.SelectedValue;

            switch (selectedGradeType)
            {
                case "CGPA":
                    txtGrade.Attributes["placeholder"] = "Enter CGPA";
                    break;
                case "A-Level":
                    txtGrade.Attributes["placeholder"] = "Enter A-Level grades (No. of A's)";
                    break;
            }
        }

        protected void btnCheckEligibility_Click(object sender, EventArgs e)
        {
            string selectedGradeType = ddlGradeType.SelectedValue;
            string gradeInput = txtGrade.Text;
            double conCGPAGrade;
            int conALevelGrade;
            string result = "Not Eligible for any scholarship.";


            if (!string.IsNullOrEmpty(gradeInput))
            {
                // For CGPA-based eligibility
                if (selectedGradeType == "CGPA")
                {
                    conCGPAGrade = double.Parse(gradeInput);
                    if (conCGPAGrade >= 3.85)
                    {
                        result = "Eligible for 100% scholarship.";
                        discount = 1;
                    }
                    else if (conCGPAGrade >= 3.75)
                    {
                        result = "Eligible for 50% scholarship.";
                        discount = 0.5;
                    }

                }
                // For A-Level-based eligibility
                else if (selectedGradeType == "A-Level")
                {
                    conALevelGrade = (int)Math.Floor(double.Parse(gradeInput));
                    if (conALevelGrade >= 3)
                    {
                        result = "Eligible for 100% scholarship.";
                        discount = 1;
                    }
                    else if (conALevelGrade == 2)
                    {
                        result = "Eligible for 50% scholarship.";
                        discount = 0.5;
                    }
                    else if (conALevelGrade == 1)
                    {
                        result = "Eligible for 25% scholarship.";
                        discount = 0.25;
                    }
                }
            }
            lblMessage.CssClass = "text-red-500";
            if (selectedGradeType != "")
            {
                if (discount != 0)
                {
                    lblMessage.CssClass = "text-green-500";
                }
            }
            else
            {
                result = "Please select a grade type";
            }
            lblMessage.Visible = true;
            lblMessage.Text = result;
            lblFinalFee.Visible = true;
            txtFinalFee.Visible = true;
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            double finalResult = 0;
            finalResult = double.Parse(txtFee.Text) * (1 - discount);
            txtFinalFee.Text = finalResult.ToString();
        }
    }
}