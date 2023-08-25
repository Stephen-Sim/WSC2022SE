using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FreshDesktop
{
    public partial class AdvancedSearchForm : FreshDesktop.Core
    {
        public AdvancedSearchForm()
        {
            InitializeComponent();
        }

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
                    ((NumericUpDown)item).Value = 0;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
