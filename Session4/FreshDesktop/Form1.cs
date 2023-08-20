using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FreshDesktop
{
    public partial class Form1 : FreshDesktop.Core
    {
        public Form1()
        {
            InitializeComponent();

            // Add items vertically
            ListViewItem item1 = new ListViewItem("Item 1, Column 1");
            item1.SubItems.Add("Item 1, Column 2");
            listView1.Items.Add(item1);

            ListViewItem item2 = new ListViewItem("Item 2, Column 1");
            item2.SubItems.Add("Item 2, Column 2");
            listView1.Items.Add(item2);

            listView1.Visible = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                string column1Value = selectedItem.SubItems[0].Text;
                string column2Value = selectedItem.SubItems[1].Text;

                textBox1.Text = column1Value;
            }
        }
    }
}
