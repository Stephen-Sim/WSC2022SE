using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreshDesktop
{
    public partial class Form1 : FreshDesktop.Core
    {
        public Form1()
        {
            InitializeComponent();
            ent = new WSC2022SE_Session1Entities();
        }

        WSC2022SE_Session1Entities ent;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var typeId = !string.IsNullOrEmpty(textBox1.Text) ? 1 : 2;

            var user = ent.Users.FirstOrDefault(x => x.Username == (typeId == 1 ? textBox1.Text : textBox2.Text) 
                && x.Password == textBox3.Text && x.UserTypeID == typeId);

            if (user == null)
            {
                MessageBox.Show("invalid user login");
                return;
            }

            if (typeId == 1)
            {
                var findUser = ent.Users.FirstOrDefault(x => x.Username == textBox2.Text);

                if (findUser == null)
                {
                    MessageBox.Show("user not found");
                    return;
                }

                if (checkBox1.Checked)
                {
                    Properties.Settings.Default.UserId = user.ID;
                    Properties.Settings.Default.UserTypeId = 1;

                    Properties.Settings.Default.Save();
                }

                this.Hide();

                new EmployeeManagementForm(findUser.ID).ShowDialog();
            }
            else
            {
                if (checkBox1.Checked)
                {
                    Properties.Settings.Default.UserId = user.ID;
                    Properties.Settings.Default.UserTypeId = 2;

                    Properties.Settings.Default.Save();
                }

                this.Hide();

                new UserManagementForm(user.ID).ShowDialog();
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.UseSystemPasswordChar = !checkBox2.Checked;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();

            new CreateAccontForm().ShowDialog();
        }
    }
}
