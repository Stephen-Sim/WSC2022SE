using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace FreshDesktop
{
    public partial class EmployeeManagementForm : FreshDesktop.Core
    {

        public User user { get; set; }
        public WSC2022SE_Session1Entities ent { get; set; }
        public EmployeeManagementForm(long userId)
        {
            InitializeComponent();

            ent = new WSC2022SE_Session1Entities();

            this.user = ent.Users.FirstOrDefault(x => x.ID == userId);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.UserId = 0;
            Properties.Settings.Default.UserTypeId = 0;

            Properties.Settings.Default.Save();

            this.Hide();

            new Form1().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            loadDataGrid1();
        }

        void loadDataGrid1()
        {
            var items = ent.Items.ToList().Where(x => x.UserID == this.user.ID).Select(x => new
            {
                x.ID,
                x.Title,
                x.Capacity,
                AreaName = x.Area.Name,
                TypeName = x.ItemType.Name,
            }).ToList();

            dataGridView1.Rows.Clear();

            for (int i = 0; i < items.Count(); i++)
            {
                dataGridView1.Rows.Add(items[i].ID, items[i].Title, items[i].Capacity, items[i].AreaName, items[i].TypeName);

                if (i % 2 == 1)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }

        }
    }
}
