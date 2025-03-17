using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;

using System.Windows.Forms;

namespace LaundryManagement
{
    public partial class Form1 : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        
        DataTable dt = new DataTable();
        public Form1()
        {
            InitializeComponent();
            
            dbcon();
            GetCustomer();
        }
         
        public void dbcon()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\salaa\\OneDrive\\Documents\\Advance Database\\Laundry Management.accdb;";
            conn = new OleDbConnection(connectionString);
        }
        public void GetCustomer()
        {

            try
            {
                string query = "SELECT Customer_fname as Firstname, Customer_lname as Lastname, Contact_Info as Information, Address  from Customer order by Customer_Id asc";
                using (OleDbDataAdapter adapt = new OleDbDataAdapter(query, conn))
                {
                    dt.Clear();
                    adapt.Fill(dt);
                    gridcus.DataSource = dt;
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}");
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Author: {ex.Message}");
            }
        }


       

        private void button2_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel3.BringToFront();
        }

        private void addcus_Click(object sender, EventArgs e)
        {
             if (string.IsNullOrEmpty(txtfname.Text) || string.IsNullOrEmpty(txtlname.Text) ||
                string.IsNullOrEmpty(txtcon.Text) || string.IsNullOrEmpty(txtadd.Text))
            {
                MessageBox.Show("Please fill in all fields."); // Show error if any field is empty
                return; // Exit the method
            }
             //Validate that contact info is numeric
             if (!long.TryParse(txtcon.Text, out _))
            {
                MessageBox.Show("Contact info must be numeric.");
                return;
            }

            string query = "INSERT INTO Customer (Customer_fname,Customer_lname, Contact_Info, Address) VALUES (@firstname, @lastname, @contactinfo, @address)";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@Customer_fname", txtfname.Text);
            cmd.Parameters.AddWithValue("@Customer_lname", txtlname.Text);
            cmd.Parameters.AddWithValue("@Contact_Info", txtcon.Text);
            cmd.Parameters.AddWithValue("Address", txtadd.Text);

            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                MessageBox.Show(rowsAffected > 0 ? "Customer inserted successfully!" : "Failed to add record.");
                if (rowsAffected > 0)
                {
                    txtfname.Text = txtlname.Text = txtcon.Text = txtadd.Text = "";
                    dt.Clear();
                    GetCustomer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

       
    }
    }

