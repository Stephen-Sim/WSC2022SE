using FreshDesktop.Models;
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
    public partial class AdvancedSearchForm : FreshDesktop.Core
    {
        public AdvancedSearchForm()
        {
            InitializeComponent();
            ent = new WSC2022SE_Session4Entities();
        }

        WSC2022SE_Session4Entities ent;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            new SimpleSearchForm().ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is ComboBox)
                {
                    ((ComboBox)item).SelectedIndex = -1;
                }
                else if (item is NumericUpDown)
                {
                    ((NumericUpDown)item).Value = 1;
                }
            }

            numericUpDown4.Value = 1000;
            comboBox6.Enabled = false;
            comboBox7.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                MessageBox.Show("start date couldn't bigger than end date");
                return;
            }

            dataGridView1.Rows.Clear();

            var itemPrices = ent.ItemPrices.ToList().Where(x => x.Date.Date >= dateTimePicker1.Value.Date &&
                x.Date.Date <= dateTimePicker2.Value.Date &&
                x.Item.Capacity >= numericUpDown2.Value &&
                x.Item.MinimumNights >= numericUpDown1.Value &&
                x.Price >= numericUpDown3.Value && x.Price <= numericUpDown4.Value).ToList();

            if (comboBox1.SelectedIndex != - 1)
            {
                itemPrices = itemPrices.Where(x => x.Item.AreaID == (long)comboBox1.SelectedValue).ToList();
            }

            if (comboBox2.SelectedIndex != -1)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemAttractions.Any(y => y.AttractionID == (long)comboBox2.SelectedValue)).ToList();
            }

            if (comboBox3.SelectedIndex != -1)
            {
                itemPrices = itemPrices.Where(x => x.ItemID == (long)comboBox3.SelectedValue).ToList();
            }

            if (comboBox4.SelectedIndex != -1)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemTypeID == (long)comboBox4.SelectedValue).ToList();
            }

            if (comboBox5.SelectedIndex != -1)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemAmenities.Any(y => y.AmenityID == (long)comboBox5.SelectedValue)).ToList();
            }

            if (comboBox6.SelectedIndex != -1)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemAmenities.Any(y => y.AmenityID == (long)comboBox6.SelectedValue)).ToList();
            }

            if (comboBox7.SelectedIndex != -1)
            {
                itemPrices = itemPrices.Where(x => x.Item.ItemAmenities.Any(y => y.AmenityID == (long)comboBox7.SelectedValue)).ToList();
            }

            this.Size = new Size(816, 656);

            var rows = itemPrices.OrderBy(x => x.Item.Title).ThenBy(x => x.Date).Select(x => new
            {
                x.ItemID,
                Property = x.Item.Title,
                Area = x.Item.Area.Name,
                AverageScore = new Func<string>(() =>
                {
                    var item = x.Item;

                    if (item.ItemScores == null)
                    {
                        return string.Empty;
                    }

                    return item.ItemScores.Average(y => y.Value).ToString("0.0");

                })(),
                TotalReserved = ent.BookingDetails.Where(y => y.ItemPrice.ItemID == x.ItemID).Count(),
                Amount = x.Price + "$",
                Date = x.Date.ToString("dd/MM/yyyy")
            }).ToList();

            foreach (var item in rows)
            {
                dataGridView1.Rows.Add(item.Property, item.Area, item.AverageScore, item.TotalReserved, item.Amount, item.Date);
            }

            label15.Text = $"Display {rows.Count} optionis from {rows.GroupBy(x => x.ItemID).Count()} properties";
        }

        private void AdvancedSearchForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.Items' table. You can move, or remove it, as needed.
            this.itemsTableAdapter.Fill(this.wSC2022SE_Session4DataSet.Items);
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.ItemTypes' table. You can move, or remove it, as needed.
            this.itemTypesTableAdapter.Fill(this.wSC2022SE_Session4DataSet.ItemTypes);
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.Amenities' table. You can move, or remove it, as needed.
            this.amenitiesTableAdapter.Fill(this.wSC2022SE_Session4DataSet.Amenities);
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.Attractions' table. You can move, or remove it, as needed.
            this.attractionsTableAdapter.Fill(this.wSC2022SE_Session4DataSet.Attractions);
            // TODO: This line of code loads data into the 'wSC2022SE_Session4DataSet.Areas' table. You can move, or remove it, as needed.
            this.areasTableAdapter.Fill(this.wSC2022SE_Session4DataSet.Areas);

            button2_Click(null, null);
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex != -1)
            {
                comboBox6.Enabled = true;

                var temp = comboBox5.Items.Cast<object>().ToList();
                temp.Remove(comboBox5.SelectedItem);

                comboBox6.DataSource = temp;
                comboBox6.DisplayMember = "Name";
                comboBox6.ValueMember = "ID";
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex != -1)
            {
                comboBox7.Enabled = true;

                var temp = comboBox6.Items.Cast<object>().ToList();
                temp.Remove(comboBox5.SelectedItem);

                comboBox7.DataSource = temp;
                comboBox7.DisplayMember = "Name";
                comboBox7.ValueMember = "ID";
            }
        }
    }
}
