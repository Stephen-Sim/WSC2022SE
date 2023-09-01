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

        void loadUniversalReport()
        {
            var securedBookings = ent.BookingDetails.ToList()
                .Where(x => x.Booking.BookingDate.Date <= DateTime.Today.Date && x.isRefund == false).Count();
            label6.Text = $"Secured past bookings: {securedBookings}";

            var upcomingbookings = ent.BookingDetails.ToList()
                .Where(x => x.Booking.BookingDate.Date > DateTime.Today.Date && x.isRefund == false).Count();
            label7.Text = $"Upcoming bookings(reservation): {upcomingbookings}";

            var mostDayOfWeekbooked = ent.BookingDetails.ToList()
                .Where(x => x.Booking.BookingDate.Date <= DateTime.Today.Date && x.isRefund == false).GroupBy(x => new
                {
                    x.Booking.BookingDate.DayOfWeek,
                })
                .Select(x => new { 
                    x.Key.DayOfWeek,
                    Count = x.Count()
                }).OrderByDescending(x => x.Count).ToList().First().DayOfWeek.ToString();
            label8.Text = $"Most booked day of week: {mostDayOfWeekbooked}";

            var inactiveListingsCount = ent.Items.ToList().Where(x => x.ItemPrices.Count == 0).Count();
            label9.Text = $"Inactive listings or properties: {inactiveListingsCount}";

            var mostUsedCoupon = ent.Coupons.ToList().Select(x => new
            {
                x.CouponCode,
                Count = x.Bookings.Count()
            }).OrderByDescending(x => x.Count).First().CouponCode;
            label10.Text = $"Most used coupon: {mostUsedCoupon}";

            var notBookedNight = ent.ItemPrices.ToList().Where(x => x.BookingDetails.Count == 0 || x.BookingDetails.Where(y => y.isRefund == true).Any()).Count();
            var totalavailableNight = ent.ItemPrices.ToList().Where(x => x.BookingDetails.Count == 0).Count();
            label11.Text = $"Vancay ratio: {notBookedNight} : {totalavailableNight}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clearFilter();

            loadUniversalReport();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearFilter();
        }
    }
}
