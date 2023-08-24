using FreshDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FreshDesktop
{
    public partial class Form1 : FreshDesktop.Core
    {
        WSC2022SE_Session4Entities ent;
        public Form1()
        {
            InitializeComponent();

            ent = new WSC2022SE_Session4Entities();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string column1Value = selectedItem.SubItems[0].Text;
                string column2Value = selectedItem.SubItems[1].Text;

                switch (column2Value)
                {
                    case "Area":
                        criteriaTypeId = 1;
                        break;
                    case "Attraction":
                        criteriaTypeId = 2;
                        break;
                    case "Listing":
                        criteriaTypeId = 3;
                        break;
                    case "Property Type":
                        criteriaTypeId = 4;
                        break;
                    case "Amenity":
                        criteriaTypeId = 5;
                        break;
                }

                textBox1.Text = column1Value;

                listView1.Visible = false;
                listView1.Items.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length >= 3)
            {
                listView1.Items.Clear();

                var areas = ent.Areas.ToList().Where(x => x.Name.ToLower().StartsWith(textBox1.Text.ToLower())).ToList();

                foreach (var item in areas)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Area");
                    listView1.Items.Add(listViewItem);
                }

                var attracs = ent.Attractions.ToList().Where(x => x.Name.ToLower().StartsWith(textBox1.Text.ToLower())).ToList();

                foreach (var item in attracs)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Attraction");
                    listView1.Items.Add(listViewItem);
                }

                var listings = ent.Items.ToList().Where(x => x.Title.ToLower().StartsWith(textBox1.Text.ToLower())).ToList();

                foreach (var item in listings)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Title);
                    listViewItem.SubItems.Add("Listing");
                    listView1.Items.Add(listViewItem);
                }

                var propertyTypes = ent.ItemTypes.ToList().Where(x => x.Name.ToLower().StartsWith(textBox1.Text.ToLower())).ToList();

                foreach (var item in propertyTypes)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Property Type");
                    listView1.Items.Add(listViewItem);
                }

                var amens = ent.Amenities.ToList().Where(x => x.Name.ToLower().StartsWith(textBox1.Text.ToLower())).ToList();

                foreach (var item in amens)
                {
                    ListViewItem listViewItem = new ListViewItem(item.Name);
                    listViewItem.SubItems.Add("Amenity");
                    listView1.Items.Add(listViewItem);
                }

                listView1.Visible = true;
            }
            else
            {
                listView1.Visible = false;
            }
        }

        int criteriaTypeId = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            /*if (dateTimePicker1.Value.Date > DateTime.Today.Date)
            {
                MessageBox.Show("date of start reservation could be before the current date.");
                return;
            }*/

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("input field cannot be null");
                return;
            }

            this.Size = new Size(802, 587);

            dataGridView1.Rows.Clear();

            var itemPrices = ent.ItemPrices.ToList().Where(x => x.Date.Date == dateTimePicker1.Value.Date &&
            x.Item.Capacity >= numericUpDown2.Value &&
            x.Item.MinimumNights >= numericUpDown1.Value).ToList();

            if (criteriaTypeId == 1)
            {
                itemPrices = itemPrices.Where(x => x.Item.Area.Name == textBox1.Text).ToList();
            }
            else if (criteriaTypeId == 2)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemAttractions.Any(y => y.Attraction.Name == textBox1.Text)).ToList();
            }
            else if (criteriaTypeId == 3)
            {
                itemPrices = itemPrices.Where(x => x.Item.Title == textBox1.Text).ToList();
            }
            else if (criteriaTypeId == 4)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemType.Name == textBox1.Text).ToList();
            }
            else if (criteriaTypeId == 5)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemAmenities.Any(y => y.Amenity.Name == textBox1.Text)).ToList();
            }

            foreach (var itemPrice in itemPrices)
            {
                var item = itemPrice.Item;
                dataGridView1.Rows.Add(item.Title, item.Area.Name, new Func<string>(() =>
                {
                    if (item.ItemScores == null)
                    {
                        return string.Empty;
                    }

                    return item.ItemScores.Average(x => x.Value).ToString();

                })(), ent.BookingDetails.Where(x => x.ItemPrice.ItemID == item.ID).Count(), itemPrice.Price + "$");
            }

            label5.Text = $"Displaying {itemPrices.Count} options";
        }
    }
}
