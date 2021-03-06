﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Attendance_System
{
    public partial class Maintain_Modules : Form
    {
        Maintain_Helper maintainhelper = new Maintain_Helper();

        SqlConnection conn = null;
        SqlCommand comm;
        SqlDataAdapter adap;
        DataSet ds;
        SqlDataReader dreader;
        string sql;

        public Maintain_Modules()
        {
            InitializeComponent();
        }

        private void btnAddModule_Click(object sender, EventArgs e)
        {
            string modID = txtModuleID.Text;
            string modDes = txtModuleDescription.Text;

            if(modID == "" || modDes == "")
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.ForeColor = System.Drawing.Color.Red;
                lblErrorMessage.Text = "Please fill in all the fields!";
            }
            else
            {
                int result = maintainhelper.Insert(modID, modDes);

                if (result > 0)
                {
                    MessageBox.Show("Record has been succesfully updated.");
                    Reset();
                }
                else if (result == 0)
                {
                    MessageBox.Show("No record could be updated.");
                }
                else
                    MessageBox.Show("There was a problem updating the record.");
            }
        }

        private void btnReturnToMenu_Click(object sender, EventArgs e)
        {
            Owner.Show();
            this.Close();
        }

        private void btnDeleteModule_Click(object sender, EventArgs e)
        {
            if (txtModuleID.Text == "")
            {
                lblErrorMessage.ForeColor = System.Drawing.Color.Red;
                lblErrorMessage.Text = "Please fill in the field below!";
            }
            else
            {
                int result = maintainhelper.Delete(txtModuleID.Text);

                if (result > 0)
                {
                    MessageBox.Show("Record has been succesfully deleted.");
                    Reset();
                }
                else if (result == 0)
                {
                    MessageBox.Show("No record could be deleted.");
                }
                else
                    MessageBox.Show("There was a problem deleting the record.");
            }

            Reset();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            string id = dgvModules.CurrentRow.Cells[0].Value.ToString();

            sql = "SELECT Module_ID, Description FROM MODULES WHERE Module_ID = @id";

            try 
            {
                Connect();
                conn.Open();

                comm = new SqlCommand(sql, conn);
                comm.Parameters.AddWithValue("@id", id);

                dreader = comm.ExecuteReader();

                while (dreader.Read())
                {
                    txtModuleID.Text = dreader.GetString(0);
                    txtModuleDescription.Text = dreader.GetString(1);
                }

                conn.Close();

                btnUpdate.Enabled = true;
                btnDeleteModule.Enabled = true;
            }
            catch(SqlException se)
            {
                MessageBox.Show(se.Message);
            }
            

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int result = maintainhelper.Update(txtModuleID.Text, txtModuleDescription.Text);

            if (result > 0)
            {
                MessageBox.Show("Record has been succesfully updated.");
                Reset();
            }
            else if (result == 0)
            {
                MessageBox.Show("No record could be updated.");
            }
            else
                MessageBox.Show("There was a problem updating the record.");

            Reset();
        }

        private void Connect()
        {
            if (conn == null)
                conn = new SqlConnection(maintainhelper.ConnectionString);
        }

        public void ModuleSearch(string caller, string criteria)
        {
            sql = "SELECT Module_ID, Description FROM MODULES WHERE " + caller + " LIKE  '%" + criteria + "%'";// FIELDS TO DISPLAY
            SQLSubmit();
        }

        private void DisplayAll()
        {
            sql = "SELECT Module_ID, Description FROM MODULES";
            SQLSubmit();
        }

        private void SQLSubmit()
        {
            Connect();
            conn.Open();

            try
            {
                comm = new SqlCommand(sql, conn);
                adap = new SqlDataAdapter();
                ds = new DataSet();

                adap.SelectCommand = comm;
                adap.Fill(ds, "MODULES");

                dgvModules.DataSource = ds;
                dgvModules.DataMember = "MODULES";
            }
            catch (SqlException se)
            {
                MessageBox.Show(se.Message);
            }

            dgvModules.Columns[0].Width = 150;
            dgvModules.Columns[1].Width = 150;

            conn.Close();
        }

        private void ClearSTextBox(char caller)
        {
            if (caller != 'I')
                txtMID.Clear();

            if (caller != 'D')
                txtMDescription.Clear();
        }

        private void Reset()
        {
            ClearSTextBox(' ');
            txtModuleID.Clear();
            txtModuleDescription.Clear();
            
            btnUpdate.Enabled = false;
            btnDeleteModule.Enabled = false;

            DisplayAll();
        }

        private void txtMID_TextChanged(object sender, EventArgs e)
        {
            ClearSTextBox('I');
            ModuleSearch("Module_ID", txtMID.Text);
        }

        private void txtMDescription_TextChanged(object sender, EventArgs e)
        {
            ClearSTextBox('D');
            ModuleSearch("Description", txtMDescription.Text);
        }

        private void Maintain_Modules_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;
            DisplayAll();
        }
    }
}
