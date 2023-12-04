using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Library_Management_System
{
    public partial class AddStudent : Form
    {
        public AddStudent()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm?", "Alert", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.Close();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtEnrollment.Clear();
            txtDepartment.Clear();
            txtSemester.Clear();
            txtContact.Clear();

            txtEmail.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtEnrollment.Text != "" && txtDepartment.Text != "" && txtSemester.Text != "" && txtContact.Text != "" && txtEmail.Text != "")
            {
                String name = txtName.Text;
                String enroll = txtEnrollment.Text;
                String dep = txtDepartment.Text;
                String sem = txtSemester.Text;
                Int64 mobile = Int64.Parse(txtContact.Text);
                String email = txtEmail.Text;

                if (IsEnrollmentNumberUnique(enroll))
                {
                    SqlConnection con = new SqlConnection();
                    con.ConnectionString = "data source = NHUNG ; database = library; integrated security=True ";
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    con.Open();
                    cmd.CommandText = "INSERT INTO NewStudent (sname, enroll, dep, sem, contact, email) VALUES ('" + name + "', '" + enroll + "', '" + dep + "', '" + sem + "', '" + mobile + "', '" + email + "')";
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Data Saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Enrollment number already exists. Please use a different enrollment number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please Fill Empty Field!", "Suggest", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool IsEnrollmentNumberUnique(string enrollmentNumber)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source = NHUNG ; database = library; integrated security=True ";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;

            con.Open();
            cmd.CommandText = "SELECT COUNT(*) FROM NewStudent WHERE enroll = '" + enrollmentNumber + "'";
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();

            return count == 0;
        }


    }
}
