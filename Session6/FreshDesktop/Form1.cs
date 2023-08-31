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
    public partial class Form1 : FreshDesktop.Core
    {
        public WSC2022SE_Session6Entities ent { get; set; }
        public Form1()
        {
            InitializeComponent();

            ent = new WSC2022SE_Session6Entities();
        }

        void clearFilter()
        {
            dateTimePicker1.Value = new DateTime(2022, 01, 01);
            dateTimePicker2.Value = new DateTime(2022, 01, 01);

            var areas = ent.Areas.ToList().OrderBy(a => a.Name).ToList();
            comboBox1.DataSource = areas;
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
            comboBox1.SelectedIndex = -1;

            var hosts = ent.Users.ToList().Where(x => x.Items.Count > 0).OrderBy(x => x.FullName).ToList();
            comboBox2.DataSource = hosts;
            comboBox2.DisplayMember = "FullName";
            comboBox2.ValueMember = "ID";
            comboBox2.SelectedIndex = -1;

            var guests = ent.Users.ToList().Where(x => x.Bookings.Count > 0).OrderByDescending(x => x.Bookings.Count).ToList();
            comboBox3.DataSource = guests;
            comboBox3.DisplayMember = "FullName";
            comboBox3.ValueMember = "ID";
            comboBox3.SelectedIndex = -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clearFilter();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearFilter();
        }
    }
}
