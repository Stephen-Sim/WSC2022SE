using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FreshDesktop
{
    public partial class UserManagementForm : FreshDesktop.Core
    {
        public User user { get; set; }
        public WSC2022SE_Session1Entities ent { get; set; }
        public UserManagementForm(long userId)
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

        void loadDataGrid1()
        {
            var items = ent.Items.ToList().Select(x => new
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

            label1.Text = $"{items.Count()} items found.";
        }

        void loadDataGrid2()
        {
            var items = ent.Items.ToList().Where(x => x.UserID == this.user.ID).Select(x => new
            {
                x.ID,
                x.Title,
                x.Capacity,
                AreaName = x.Area.Name,
                TypeName = x.ItemType.Name,
            }).ToList();

            dataGridView2.Rows.Clear();

            for (int i = 0; i < items.Count(); i++)
            {
                dataGridView2.Rows.Add(items[i].ID, items[i].Title, items[i].Capacity, items[i].AreaName, items[i].TypeName, "Edit Detials");

                if (i % 2 == 1)
                {
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }

            label1.Text = $"{items.Count()} items found.";
        }

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            loadDataGrid1();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                loadDataGrid1();
            }
            else
            {
                var items = ent.Items.ToList().Where(x => x.Title.StartsWith(textBox1.Text) || 
                    x.Area.Name.StartsWith(textBox1.Text) || 
                    x.ItemAttractions.Any(y => y.Distance < 1 && y.Attraction.Name.StartsWith(textBox1.Text))
                ).Select(x => new
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

                label1.Text = $"{items.Count()} items found.";
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                loadDataGrid1();
            }
            else
            {
                loadDataGrid2();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();

            new AddOrEditListingForm(this.user.ID).ShowDialog();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                var itemId = int.Parse(dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString());

                this.Hide();

                new AddOrEditListingForm (this.user.ID, itemId).ShowDialog();
            }
        }
    }
}
