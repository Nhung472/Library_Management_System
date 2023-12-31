using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management_System
{
    public partial class IssueBooks : Form
    {
        public IssueBooks()
        {
            InitializeComponent();
        }

        private void IssueBooks_Load(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = "data source = NHUNG ; database = library; integrated security=True";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            con.Open();

            cmd = new SqlCommand("Select book_name from NewBook", con);
            SqlDataReader Sdr = cmd.ExecuteReader();

            while (Sdr.Read ()) 
            {
                for(int i=0; i<Sdr.FieldCount; i++)
                {
                    comboBoxBook.Items.Add(Sdr.GetString(i));
                }
            }
            Sdr.Close();
            con.Close();

        }

        int count;
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(txtEnrollment.Text !="")
            {
                String eid = txtEnrollment.Text;
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "data source = NHUNG ; database = library; integrated security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                cmd.CommandText = "SELECT * FROM NewStudent WHERE enroll = '"+eid+"' ";
                SqlDataAdapter DA = new SqlDataAdapter(cmd);
                DataSet DS = new DataSet();
                DA.Fill(DS);

                //count how many book that the student had issued
                cmd.CommandText = "SELECT count(std_enroll) FROM IRBook WHERE std_enroll = '" + eid + "' AND book_return_date IS NULL ";
                SqlDataAdapter DA1 = new SqlDataAdapter(cmd);
                DataSet DS1 = new DataSet();
                DA1.Fill(DS1);

                count = int.Parse(DS1.Tables[0].Rows[0][0].ToString());

                if (DS.Tables[0].Rows.Count != 0)
                {
                    txtName.Text = DS.Tables[0].Rows[0][1].ToString();
                    txtDepartment.Text = DS.Tables[0].Rows[0][3].ToString();
                    txtSemester.Text = DS.Tables[0].Rows[0][4].ToString();
                    txtContact.Text = DS.Tables[0].Rows[0][5].ToString();
                    txtEmail.Text = DS.Tables[0].Rows[0][6].ToString();
                }
                else
                {
                    txtName.Clear();
                    txtDepartment.Clear();
                    txtSemester.Clear();
                    txtContact.Clear();
                    txtEmail.Clear();
                    MessageBox.Show("Invalid Enrollment NO", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnIssueBook_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "")
            {
                String enroll = txtEnrollment.Text;
                String sname = txtName.Text;
                String sdep = txtDepartment.Text;
                String sem = txtSemester.Text;
                Int64 contact = Int64.Parse(txtContact.Text);
                String email = txtEmail.Text;
                String bookname = comboBoxBook.Text;
                String bookIssueDate = dateTimePicker.Text;

                SqlConnection con = new SqlConnection();
                con.ConnectionString = "data source = NHUNG ; database = library; integrated security=True";
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;

                con.Open();

                cmd.CommandText = "SELECT COUNT(std_enroll) FROM IRBook WHERE std_enroll = '" + enroll + "' AND book_return_date IS NULL";
                int currentIssuedBooks = (int)cmd.ExecuteScalar();

                if (comboBoxBook.SelectedIndex != -1 && currentIssuedBooks < 5)
                {
                    cmd.CommandText = "INSERT INTO IRBook(std_enroll, std_name, std_dep, std_sem, std_contact, std_email, book_name, book_issue_date) VALUES ('" + enroll + "', '" + sname + "', '" + sdep + "', '" + sem + "', " + contact + ", '" + email + "', '" + bookname + "', '" + bookIssueDate + "') ";
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Book Issued.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The MAXIMUM number of books that can be ISSUED is 5. Please Return a book to continue", "No Book SELECTED", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void txtEnrollment_TextChanged(object sender, EventArgs e)
        {
            if(txtEnrollment.Text == "")
            {
                txtName.Clear();
                txtEmail.Clear();
                txtContact.Clear();
                txtDepartment.Clear();
                txtSemester.Clear();

            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtEnrollment.Clear();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)==DialogResult.OK)
            {
                this.Close();
            }
        }
    }
}
