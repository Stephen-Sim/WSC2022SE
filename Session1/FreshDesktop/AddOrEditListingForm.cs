using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FreshDesktop
{
    public partial class AddOrEditListingForm : FreshDesktop.Core
    {

        public User user { get; set; }
        public long? itemId { get; set; }
        public WSC2022SE_Session1Entities ent { get; set; }

        public AddOrEditListingForm(long userId)
        {
            InitializeComponent();

            ent = new WSC2022SE_Session1Entities();

            this.user = ent.Users.FirstOrDefault(x => x.ID == userId);

            this.Text = "Seoul Stay - Add Lisitng";

            this.button2.Text = "Cancel";
        }

        public AddOrEditListingForm(long userId, long itemId)
        {
            InitializeComponent();

            ent = new WSC2022SE_Session1Entities();

            this.user = ent.Users.FirstOrDefault(x => x.ID == userId);

            this.itemId = itemId;

            this.Text = "Seoul Stay - Edit Lisitng";

            this.button1.Visible = false;

            this.button2.Text = "Close";
        }

        private void AddOrEditListingForm_Load(object sender, System.EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2022SE_Session1DataSet.ItemTypes' table. You can move, or remove it, as needed.
            this.itemTypesTableAdapter.Fill(this.wSC2022SE_Session1DataSet.ItemTypes);

            comboBox1.SelectedIndex = -1;

            loadData();
        }

        int tabCounter = 0;

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (itemId == null)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    if (!validateInput(tabPage1))
                    {
                        return;
                    }

                    if (numericUpDown5.Value > numericUpDown6.Value)
                    {
                        MessageBox.Show("minimum nights could not bigger than maximum night.");
                        return;
                    }

                    tabControl1.SelectedIndex = ++tabCounter;
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    tabControl1.SelectedIndex = ++tabCounter;

                    button1.Visible = false;

                    button2.Text = "Finish";
                    button2.BackColor = Color.FromArgb(51, 51, 51);
                    button2.ForeColor = Color.FromArgb(187, 187, 187);
                }
            }
        }

        void loadData()
        {
            if (itemId == null)
            {
                var amens = ent.Amenities.ToList();

                dataGridView1.Rows.Clear();

                for (int i = 0; i < amens.Count(); i++)
                {
                    dataGridView1.Rows.Add(amens[i].ID, amens[i].Name, false);

                    if (i % 2 == 1)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }

                var attracs = ent.Attractions.ToList();

                dataGridView2.Rows.Clear();

                for (int i = 0; i < attracs.Count(); i++)
                {
                    dataGridView2.Rows.Add(attracs[i].ID, attracs[i].Name, attracs[i].Area.Name);

                    if (i % 2 == 1)
                    {
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }
            }
            else
            {
                var item = ent.Items.FirstOrDefault(x => x.ID == this.itemId);

                comboBox1.SelectedValue = item.ItemTypeID;
                textBox1.Text = item.Title;
                textBox2.Text = item.ApproximateAddress;
                textBox3.Text = item.ExactAddress;
                textBox4.Text = item.Description;
                textBox5.Text = item.HostRules;

                numericUpDown1.Value = item.Capacity;
                numericUpDown2.Value = item.NumberOfBeds;
                numericUpDown3.Value = item.NumberOfBedrooms;
                numericUpDown4.Value = item.NumberOfBathrooms;
                numericUpDown5.Value = item.MinimumNights;
                numericUpDown6.Value = item.MaximumNights;

                var amens = ent.Amenities.ToList();

                dataGridView1.Rows.Clear();

                for (int i = 0; i < amens.Count(); i++)
                {
                    var any = item.ItemAmenities.Any(x => x.AmenityID == amens[i].ID);
                    dataGridView1.Rows.Add(amens[i].ID, amens[i].Name, any);

                    if (i % 2 == 1)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }

                var attracs = ent.Attractions.ToList();

                dataGridView2.Rows.Clear();

                for (int i = 0; i < attracs.Count(); i++)
                {
                    var any = item.ItemAttractions.FirstOrDefault(x => x.AttractionID == attracs[i].ID);

                    dataGridView2.Rows.Add(attracs[i].ID, attracs[i].Name, attracs[i].Area.Name,
                        any != null ? any.Distance : null,
                        any != null ? any.DurationOnFoot : null,
                        any != null ? any.DurationByCar : null);

                    if (i % 2 == 1)
                    {
                        dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                    }
                }
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (button2.Text == "Cancel")
            {
                this.Hide();
                new UserManagementForm(this.user.ID).ShowDialog();

                return;
            }

            if (itemId == null)
            {
                if (dataGridView2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[3].Value != null).Count() < 2)
                {
                    MessageBox.Show("you should input at least two attraction.");
                    return;
                }

                var item = new Item();

                item.Title = textBox1.Text;
                item.ApproximateAddress = textBox2.Text;
                item.ExactAddress = textBox3.Text;
                item.Description = textBox4.Text;
                item.HostRules = textBox5.Text;

                item.Capacity = (int)numericUpDown1.Value;
                item.NumberOfBeds = (int)numericUpDown2.Value;
                item.NumberOfBedrooms = (int)numericUpDown3.Value;
                item.NumberOfBathrooms = (int)numericUpDown4.Value;
                item.NumberOfBathrooms = (int)numericUpDown5.Value;
                item.MaximumNights = (int)numericUpDown6.Value;

                item.ItemTypeID = (long)comboBox1.SelectedValue;

                item.UserID = this.user.ID;
                item.GUID = Guid.NewGuid();
                item.AreaID = 1;

                ent.Items.Add(item);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (bool.Parse(row.Cells[2].Value.ToString()))
                    {
                        var itemAmen = new ItemAmenity
                        {
                            AmenityID = int.Parse(row.Cells[0].Value.ToString()),
                            ItemID = item.ID,
                            GUID = Guid.NewGuid(),
                        };

                        ent.ItemAmenities.Add(itemAmen);
                    }
                }

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[3].Value != null && !string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                    {
                        try
                        {
                            var itemAttrac = new ItemAttraction
                            {
                                AttractionID = int.Parse(row.Cells[0].Value.ToString()),
                                ItemID = item.ID,
                                GUID = Guid.NewGuid(),
                            };

                            itemAttrac.Distance = decimal.Parse(row.Cells[3].Value.ToString());
                            itemAttrac.DurationOnFoot = row.Cells[4].Value != null ? (long?)long.Parse(row.Cells[4].Value.ToString()) : null;
                            itemAttrac.DurationByCar = row.Cells[5].Value != null ? (long?)long.Parse(row.Cells[5].Value.ToString()) : null;

                            ent.ItemAttractions.Add(itemAttrac);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("invalid input for attraction value.");
                        }
                    }
                }

                ent.SaveChanges();

                MessageBox.Show("item added!");

                this.Hide();

                new UserManagementForm(this.user.ID).ShowDialog();
            }
            else
            {
                if (dataGridView2.Rows.Cast<DataGridViewRow>().Where(x => x.Cells[3].Value != null).Count() < 2)
                {
                    MessageBox.Show("you should input at least two attraction.");
                    return;
                }

                var item = ent.Items.FirstOrDefault(x => x.ID == this.itemId);

                item.Title = textBox1.Text;
                item.ApproximateAddress = textBox2.Text;
                item.ExactAddress = textBox3.Text;
                item.Description = textBox4.Text;
                item.HostRules = textBox5.Text;

                item.Capacity = (int)numericUpDown1.Value;
                item.NumberOfBeds = (int)numericUpDown2.Value;
                item.NumberOfBedrooms = (int)numericUpDown3.Value;
                item.NumberOfBathrooms = (int)numericUpDown4.Value;
                item.NumberOfBathrooms = (int)numericUpDown5.Value;
                item.MaximumNights = (int)numericUpDown6.Value;

                item.ItemTypeID = (long)comboBox1.SelectedValue;

                ent.ItemAmenities.RemoveRange(item.ItemAmenities);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (bool.Parse(row.Cells[2].Value.ToString()))
                    {
                        var itemAmen = new ItemAmenity
                        {
                            AmenityID = int.Parse(row.Cells[0].Value.ToString()),
                            ItemID = item.ID,
                            GUID = Guid.NewGuid(),
                        };

                        ent.ItemAmenities.Add(itemAmen);
                    }
                }

                ent.ItemAttractions.RemoveRange(item.ItemAttractions);

                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells[3].Value != null && !string.IsNullOrEmpty(row.Cells[3].Value.ToString()))
                    {
                        try
                        {
                            var itemAttrac = new ItemAttraction
                            {
                                AttractionID = int.Parse(row.Cells[0].Value.ToString()),
                                ItemID = item.ID,
                                GUID = Guid.NewGuid(),
                            };

                            itemAttrac.Distance = decimal.Parse(row.Cells[3].Value.ToString());
                            itemAttrac.DurationOnFoot = row.Cells[4].Value != null ? (long?)long.Parse(row.Cells[4].Value.ToString()) : null;
                            itemAttrac.DurationByCar = row.Cells[5].Value != null ? (long?)long.Parse(row.Cells[5].Value.ToString()) : null;

                            ent.ItemAttractions.Add(itemAttrac);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("invalid input for attraction value.");
                        }
                    }
                }

                ent.SaveChanges();

                MessageBox.Show("item modified!");

                this.Hide();

                new UserManagementForm(this.user.ID).ShowDialog();
            }
        }

        private void tabControl1_Click(object sender, System.EventArgs e)
        {
            if (itemId == null)
            {
                MessageBox.Show("you can't switch the tab!");
                tabControl1.SelectedIndex = tabCounter;
            }
        }
    }
}
