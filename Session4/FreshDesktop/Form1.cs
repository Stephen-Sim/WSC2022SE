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

                textBox1.Text = column1Value;

                listView1.Visible = false;
                listView1.Items.Clear();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
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
    }
}
