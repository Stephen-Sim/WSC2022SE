using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreshDesktop
{
    public partial class CreateAccontForm : FreshDesktop.Core
    {
        public WSC2022SE_Session1Entities ent { get; set; }

        public CreateAccontForm()
        {
            InitializeComponent();
            ent = new WSC2022SE_Session1Entities();

            radioButton1.Checked = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();

            new Form1().ShowDialog();
        }

        bool isRead = false;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (StreamReader sw = new StreamReader("Terms.txt"))
            {
                isRead = true;
                MessageBox.Show(sw.ReadToEnd());
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (isRead == false)
            {
                MessageBox.Show("You have to read terms and conditions at least once.");

                checkBox1.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.validateInput(groupBox1))
            {
                return;
            }

            if (!checkBox1.Checked)
            {
                MessageBox.Show("You need to agree terms and condition.");
                return;
            }

            if (ent.Users.Any(x => x.Username == textBox1.Text))
            {
                MessageBox.Show("Username is taken.");
                return;
            }

            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("Family count could be 0 or lesser value.");
                return;
            }

            if (textBox3.Text != textBox4.Text)
            {
                MessageBox.Show("Wrong retype password.");
                return;
            }

            if (textBox3.Text.Length < 5)
            {
                MessageBox.Show("Pasword length should be at 5");
                return;
            }

            var user = new User()
            {
                GUID = Guid.NewGuid(),
                UserTypeID = 2, 
                Username = textBox1.Text,
                FullName = textBox2.Text,
                Password = textBox3.Text,
                Gender  = radioButton1.Checked,
                BirthDate = dateTimePicker1.Value.Date,
                FamilyCount = (int)numericUpDown1.Value
            };

            CreateUserAction createUserAction = new CreateUserAction();
            createUserAction.InsertUser(user);

            this.Hide();

            new UserManagementForm(user.ID).ShowDialog();
        }
    }
}
