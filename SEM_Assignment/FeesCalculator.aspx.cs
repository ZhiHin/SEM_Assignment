using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SEM_Assignment
{
    public partial class FeesCalculator : Page
    {
        double discount
        {
            get { return ViewState["Discount"] != null ? (double)ViewState["Discount"] : 0; }
            set { ViewState["Discount"] = value; }
        }

        double duration
        {
            get { return ViewState["Duration"] != null ? (double)ViewState["Duration"] : 0; }
            set { ViewState["Duration"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;
                txtFinalFee.Visible = false;
                lblFinalFee.Visible = false;
                ddlStudType.SelectedIndex = 0;
                string programId = Request.QueryString["selectedId"];
                if (!string.IsNullOrEmpty(programId))
                {
                    LoadProgramDetails(int.Parse(programId));
                }
            }
        }

        private void LoadProgramDetails(int programId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

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
                    txtCourse.Text = reader["ProgramName"].ToString();
                    duration = int.Parse(reader["DurationSem"].ToString());
                    txtFee.Text = ddlStudType.SelectedValue == "Local" ?
                                  reader["TuitionFees"].ToString() :
                                  reader["InterTuitionFees"].ToString();
                }
                else
                {
                    lblMessage.Text = "Program details not found.";
                    lblMessage.CssClass = "text-red-500";
                    lblMessage.Visible = true;
                }
                reader.Close();
            }
        }

        protected void ddlGradeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedGradeType = ddlGradeType.SelectedValue;
            txtGrade.Attributes["placeholder"] = selectedGradeType == "CGPA" ? "Enter CGPA" : "Enter A-Level grades (No. of A's)";
        }

        protected void btnCheckEligibility_Click(object sender, EventArgs e)
        {
            string selectedGradeType = ddlGradeType.SelectedValue;
            string gradeInput = txtGrade.Text;
            double conCGPAGrade;
            int conALevelGrade;
            string result = "Not Eligible for any TARUMT administered scholarship.";
            discount = 0;
            if (!string.IsNullOrEmpty(gradeInput))
            {
                try
                {
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
                    else if (selectedGradeType == "A-Level")
                    {
                        conALevelGrade = int.Parse(gradeInput);
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
                catch (FormatException)
                {
                    result = "Invalid grade input. Please enter a valid number.";
                }

                if (discount == 0)
                {
                    LoadRecommendedScholarships();
                }
            }
            else
            {
                result = "Please enter your grade.";
            }

            lblMessage.CssClass = discount > 0 ? "text-green-500" : "text-red-500";
            lblMessage.Text = result;
            lblMessage.Visible = true;
            lblFinalFee.Visible = true;
            txtFinalFee.Visible = true;
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            // Get the tuition fee from the text box
            double tuitionFee = double.Parse(txtFee.Text);

            // Check if there is a discount
            if (discount != 0)
            {
                // Calculate the final fee with the discount
                double finalResult = tuitionFee * (1 - discount);
                txtFinalFee.Text = finalResult.ToString("F2");
                lblAvgFee.Text = $"Average Fee: RM{(finalResult / duration):F2} per Semester";
            }
            else
            {
                // Access the selected value of the RadioButtonList
                string selectedScholarshipValue = Request.Form["ctl00$ContentPlaceHolder1$rblScholarship"];

                // Check if a scholarship is selected
                if (!string.IsNullOrEmpty(selectedScholarshipValue))
                {
                    // Calculate the final fee with the selected scholarship
                    double scholarshipDiscount = double.Parse(selectedScholarshipValue);
                    double finalResult = tuitionFee * (1 - scholarshipDiscount);
                    txtFinalFee.Text = finalResult.ToString("F2");
                    lblAvgFee.Text = $"Average Fee: RM{(finalResult / duration):F2} per Semester";
                }
                else
                {
                    // Handle the case where no scholarship is selected
                    lblMessage.Text = "Please select a scholarship option.";
                    lblMessage.CssClass = "text-red-500";
                    lblMessage.Visible = true;
                }
            }
        }

        protected void ddlStudType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string programId = Request.QueryString["selectedId"];
            if (!string.IsNullOrEmpty(programId))
            {
                LoadProgramDetails(int.Parse(programId));
            }
        }

        protected void LoadRecommendedScholarships()
        {
            string studentType = ddlStudType.SelectedValue;
            string financialStatus = ddlFinancialStatus.SelectedValue;
            string disability = chkDisability.Checked ? "Y" : "N";
            string cgpaInput = txtAim.Text;
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = " SELECT scholarshipID, name, amount, url " +
                    "FROM scholarship " +
                    "WHERE studentType = @studentType " +
                    "AND financialStatus = @financialStatus " +
                    "AND minCgpa <= @cgpaInput " +
                    "AND disability = @disability";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentType", studentType);
                cmd.Parameters.AddWithValue("@financialStatus", financialStatus);
                cmd.Parameters.AddWithValue("@cgpaInput", cgpaInput);
                cmd.Parameters.AddWithValue("@disability", disability);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                RadioButtonList rblScholarship = new RadioButtonList { ID = "rblScholarship" };
                if (reader.HasRows)
                {
                    pScholarship.Controls.Clear();
                    while (reader.Read())
                    {
                        rblScholarship.Items.Add(new ListItem(reader["name"].ToString() + $" <a href='{reader["url"].ToString()}'>more details</a>", reader["amount"].ToString()));
                    }
                }
                else
                {
                    lblMessage.Text = "No recommended scholarships found.";
                    lblMessage.CssClass = "text-red-500";
                    lblMessage.Visible = true;
                }
                pScholarship.Controls.Add(rblScholarship);
                reader.Close();
            }
        }
    }
}
