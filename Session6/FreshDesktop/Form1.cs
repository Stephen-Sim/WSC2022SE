﻿using FreshDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

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
            // Property of Lisitng Summary
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

            var cancalledReservations = ent.BookingDetails.Where(x => x.isRefund == true).Count();
            label27.Text = $"Cancelled reservations: {cancalledReservations}";

            var mostUsedCoupon = ent.Coupons.ToList().Select(x => new
            {
                x.CouponCode,
                Count = x.Bookings.Count()
            }).OrderByDescending(x => x.Count).First().CouponCode;
            label10.Text = $"Most used coupon: {mostUsedCoupon}";

            var notBookedNight = ent.ItemPrices.ToList().Where(x => x.BookingDetails.Count == 0 || x.BookingDetails.Where(y => y.isRefund == true).Any()).Count();
            var totalavailableNight = ent.ItemPrices.ToList().Where(x => x.BookingDetails.Count == 0).Count();
            label11.Text = $"Vancay ratio: {notBookedNight} : {totalavailableNight}";

            // Scores Summary
            var avgScore = ent.ItemScores.ToList().Average(x => x.Value).ToString("0.00");
            label12.Text = $"Average score for listings: {avgScore}";

            var highestAvgScore = ent.Items.ToList().Select(x => new
            {
                Title = x.Title,
                Avg = x.ItemScores.Any() ? x.ItemScores.Average(y => y.Value) : 0,
            }).OrderByDescending(x => x.Avg).First().Title;
            label13.Text = $"Name of lisitng with highest score: {highestAvgScore}";


            var topOwners = ent.Users.ToList().Select(x => new
            {
                x.FullName,
                Avg = new Func<decimal>(() => {
                    var itemScoreCount = x.Items.SelectMany(y => y.ItemScores).Count();

                    if (itemScoreCount == 0)
                    {
                        return 0.0m;
                    }

                    var itemSum = x.Items.SelectMany(y => y.ItemScores).Sum(y => y.Value);

                    return itemSum * 1.0m / itemScoreCount;
                })(),
            }).ToList();

            var maxValue = topOwners.Max(x => x.Avg);
            topOwners = topOwners.Where(x => x.Avg == maxValue).ToList();

            var topOwnerNames = string.Join(", ", topOwners.Select(x => x.FullName));
            label14.Text = $"Top owner / manager by avarage score: {topOwnerNames}";

            var leastCleanessOwners = ent.ItemScores.Where(x => x.ScoreID == 2).Select(x => new
            {
                x.Item.User.FullName,
                x.Value
            }).ToList();

            var leastScore = leastCleanessOwners.Min(x => x.Value);
            leastCleanessOwners = leastCleanessOwners.Where(x => x.Value == leastScore).ToList();

            var leastCleanessOwnerNames = string.Join(", ", leastCleanessOwners.Distinct().Select(x => x.FullName));
            label15.Text = $"The least clean owner / manager by avarage score: {leastCleanessOwnerNames}";

            // Monthly Vacancy Ratio
            var date = dateTimePicker2.Value;
            var firstDayOfCurrentMonth = new DateTime(date.Year, date.Month, 1);
            var threeMonthsAgo = firstDayOfCurrentMonth.AddMonths(-3); // Calculate the date three months ago

            var lastThreeMonthVacancyRatio = ent.ItemPrices.ToList()
                .Where(x => x.Date >= threeMonthsAgo && x.Date < firstDayOfCurrentMonth) // Filter data for the last three months
                .GroupBy(x => new
                {
                    Month = x.Date.ToString("MMM"),
                })
                .Select(x => new
                {
                    Month = x.Key.Month,
                    BookedCount = x.Count(y => y.BookingDetails.Count() > 0),
                    NotBookedCount = x.Count(y => y.BookingDetails.Count() == 0)
                })
                .ToList();

            chart1.Series[0].Name = "Vacant";
            chart1.Series[1].Name = "Reserved";

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();

            foreach (var item in lastThreeMonthVacancyRatio)
            {
                chart1.Series[0].Points.AddXY(item.Month, item.BookedCount);
                chart1.Series[1].Points.AddXY(item.Month, item.NotBookedCount);
            }

            //Financial Summary
            var avgNetRevenue = ent.BookingDetails.ToList().Select(x => new
            {
                Price = new Func<decimal?>(() =>
                {
                    if (x.isRefund == true)
                    {
                        TimeSpan difference = x.ItemPrice.Date - (DateTime)x.RefundDate;
                        var numberOfDays = difference.Days;

                        if (x.ItemPrice.CancellationPolicy.CancellationRefundFees.Any(y => y.DaysLeft == numberOfDays))
                        {
                            var percent = x.ItemPrice.CancellationPolicy.CancellationRefundFees.FirstOrDefault(y => y.DaysLeft == numberOfDays).PenaltyPercentage / 100;
                            return x.ItemPrice.Price * (1 - percent) / 2;
                        }

                        return 0;
                    }

                    var itemPrice = x.ItemPrice;
                    var booking = x.Booking;
                    var discount = booking.BookingDetails.Sum(y => y.ItemPrice.Price) - booking.AmountPaid;
                    var couponPercent = itemPrice.Price / booking.BookingDetails.Sum(y => y.ItemPrice.Price);
                    var commission = itemPrice.Price * (itemPrice.CancellationPolicy.Commission / 100);

                    return itemPrice.Price - (1 - couponPercent * discount) - commission;
                })(),
                x.ItemPrice.Item.UserID
            }).Sum(x => x.Price) / ent.Users.Count();
            label16.Text = $"Average net revenue of all owners / managers: {avgNetRevenue?.ToString("0.00")}";

            var hightestNetRevenueofanOwner = ent.BookingDetails.ToList().Select(x => new
            {
                Price = new Func<decimal?>(() =>
                {
                    if (x.isRefund == true)
                    {
                        TimeSpan difference = x.ItemPrice.Date - (DateTime)x.RefundDate;
                        var numberOfDays = difference.Days;

                        if (x.ItemPrice.CancellationPolicy.CancellationRefundFees.Any(y => y.DaysLeft == numberOfDays))
                        {
                            var percent = x.ItemPrice.CancellationPolicy.CancellationRefundFees.FirstOrDefault(y => y.DaysLeft == numberOfDays).PenaltyPercentage / 100;
                            return x.ItemPrice.Price * percent / 2;
                        }

                        return 0;
                    }

                    var itemPrice = x.ItemPrice;
                    var booking = x.Booking;
                    var discount = booking.BookingDetails.Sum(y => y.ItemPrice.Price) - booking.AmountPaid;
                    var couponPercent = itemPrice.Price / booking.BookingDetails.Sum(y => y.ItemPrice.Price);
                    var commission = itemPrice.Price * (itemPrice.CancellationPolicy.Commission / 100);
                    
                    return itemPrice.Price - (1 - couponPercent * discount) - commission;
                })(),
                x.ItemPrice.Item.UserID
            }).GroupBy(x => new
            {
                x.UserID
            }).Select(x => new
            {
                x.Key.UserID,
                TotalRevenue = x.Sum(y => y.Price)
            }).OrderByDescending(x => x.TotalRevenue).First().TotalRevenue;
            label17.Text = $"Highest net revenue for an owner / manager: {hightestNetRevenueofanOwner?.ToString("0.00")}";

            var totalRevenueCancel = ent.BookingDetails.ToList().Where(x => x.isRefund == true).Select(x => new
            {
                RefundCommission = new Func<decimal?>(() =>
                {
                    TimeSpan difference = x.ItemPrice.Date - (DateTime)x.RefundDate;
                    var numberOfDays = difference.Days;
                    
                    if (x.ItemPrice.CancellationPolicy.CancellationRefundFees.Any(y => y.DaysLeft == numberOfDays))
                    {
                        var percent = x.ItemPrice.CancellationPolicy.CancellationRefundFees.FirstOrDefault(y => y.DaysLeft == numberOfDays).PenaltyPercentage / 100;
                        return x.ItemPrice.Price * percent;
                    }

                    return 0;
                })()
            }).Sum(x => x.RefundCommission / 2);
            label18.Text = $"Our total revenue from cancellations: {totalRevenueCancel?.ToString("0.00")}";

            var totalCouponDiscount = ent.BookingDetails.ToList().Where(x => x.Booking.Coupon != null).Sum(x => x.ItemPrice.Price) - ent.Bookings.Where(x => x.Coupon != null).Sum(x => x.AmountPaid);

            label19.Text = $"Total discount from coupon: {totalCouponDiscount?.ToString("0.00")}";
        }

        void loadServiceReport()
        {
            var totalservice = ent.AddonServiceDetails.ToList().Where(x => x.FromDate.Date < DateTime.Now.Date).Count();
            label23.Text = totalservice.ToString();

            var mostBookedService = ent.AddonServiceDetails.ToList().GroupBy(x => new { x.Service })
                .Select(x => new {
                    Name = x.Key.Service.Name,
                    Count = x.Count()
                }).ToList().OrderByDescending(x => x.Count).First().Name;
            label24.Text = mostBookedService;

            var totalRevenue = ent.AddonServices.ToList().Select(x => new
            {
                TotalPrice = new Func<decimal>(() =>
                {
                    var total = x.AddonServiceDetails.Sum(y => y.Price);

                    if (x.CouponID != null)
                    {
                        var discount = total * (x.Coupon.DiscountPercent / 100) > x.Coupon.MaximimDiscountAmount ? x.Coupon.MaximimDiscountAmount : total * (x.Coupon.DiscountPercent / 100);
                        total -= discount;
                    }

                    return total;
                })()
            }).Sum(x => x.TotalPrice);

            label25.Text = totalRevenue.ToString("0.00");

            var datas = ent.ServiceTypes.ToList().Select(x => new
            {
                x.Name,
                Availability = new Func<bool>(() =>
                {
                    if (x.Services.Count() == 0)
                    {
                        return false;
                    }

                    return true;
                })()
            }).ToList();

            var start = new DateTime(dateTimePicker1.Value.Date.Year, dateTimePicker1.Value.Date.Month, 1);
            var end = new DateTime(dateTimePicker2.Value.Date.Year, dateTimePicker2.Value.Date.Month, 1);

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            foreach (var item in datas)
            {
                dataGridView2.Rows.Add(item.Name);
            }

            for (var i = new DateTime(DateTime.Now.Year, 1, 1); i < new DateTime(DateTime.Now.Year + 1, 1, 1); i = i.AddMonths(1))
            {
                dataGridView1.Columns.Add("", i.ToString("MMM"));
            }

            for (int i = 0; i < datas.Count(); i++)
            {
                dataGridView1.Rows.Add();

                if (datas[i].Availability)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            clearFilter();

            loadUniversalReport();
            loadServiceReport();
            loadHostAnalysis();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearFilter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                return;
            }
            loadUniversalReport();

            var start = new DateTime(dateTimePicker1.Value.Date.Year, dateTimePicker1.Value.Date.Month, 1);
            var end = new DateTime(dateTimePicker2.Value.Date.Year, dateTimePicker2.Value.Date.Month, 1);

            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            var datas = ent.ServiceTypes.ToList().Select(x => new
            {
                x.Name,
                Availability = new Func<bool>(() =>
                {
                    if (x.Services.Count() == 0)
                    {
                        return false;
                    }

                    return true;
                })()
            }).ToList();

            for (var i = new DateTime(dateTimePicker1.Value.Year, dateTimePicker1.Value.Month, 1); i < new DateTime(dateTimePicker2.Value.Year, dateTimePicker2.Value.Month, 1); i = i.AddMonths(1))
            {
                dataGridView1.Columns.Add("", i.ToString("MMM"));
            }

            for (int i = 0; i < datas.Count(); i++)
            {
                dataGridView1.Rows.Add();

                if (datas[i].Availability)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        void loadHostAnalysis()
        {
            var users = ent.BookingDetails.ToList().Select(x => new
            {
                NetPrice = new Func<decimal>(() =>
                {
                    if (x.isRefund == true)
                    {
                        TimeSpan difference = x.ItemPrice.Date - (DateTime)x.RefundDate;
                        var numberOfDays = difference.Days;

                        if (x.ItemPrice.CancellationPolicy.CancellationRefundFees.Any(y => y.DaysLeft == numberOfDays))
                        {
                            var percent = x.ItemPrice.CancellationPolicy.CancellationRefundFees.FirstOrDefault(y => y.DaysLeft == numberOfDays).PenaltyPercentage / 100;
                            return x.ItemPrice.Price * percent / 2;
                        }

                        return 0;
                    }

                    var itemPrice = x.ItemPrice;
                    var booking = x.Booking;
                    var discount = booking.BookingDetails.Sum(y => y.ItemPrice.Price) - booking.AmountPaid;
                    var couponPercent = itemPrice.Price / booking.BookingDetails.Sum(y => y.ItemPrice.Price);
                    var commission = itemPrice.Price * (itemPrice.CancellationPolicy.Commission / 100);

                    return (decimal)(itemPrice.Price - (1 - couponPercent * discount) - commission);
                })(),
                Total = new Func<decimal>(() =>
                {
                    if (x.isRefund == true)
                    {
                        TimeSpan difference = x.ItemPrice.Date - (DateTime)x.RefundDate;
                        var numberOfDays = difference.Days;

                        if (x.ItemPrice.CancellationPolicy.CancellationRefundFees.Any(y => y.DaysLeft == numberOfDays))
                        {
                            var percent = x.ItemPrice.CancellationPolicy.CancellationRefundFees.FirstOrDefault(y => y.DaysLeft == numberOfDays).PenaltyPercentage / 100;
                            return x.ItemPrice.Price * percent / 2;
                        }

                        return 0;
                    }

                    var itemPrice = x.ItemPrice;
                    var booking = x.Booking;
                    var discount = booking.BookingDetails.Sum(y => y.ItemPrice.Price) - booking.AmountPaid;

                    return itemPrice.Price;
                })(),
                Balance = new Func<decimal>(() =>
                {
                    var booking = x.Booking;

                    if (x.Booking.Transaction.TransactionTypeID == 1)
                    {
                        return 0;
                    }

                    if (x.isRefund == true)
                    {
                        TimeSpan difference = x.ItemPrice.Date - (DateTime)x.RefundDate;
                        var numberOfDays = difference.Days;

                        if (x.ItemPrice.CancellationPolicy.CancellationRefundFees.Any(y => y.DaysLeft == numberOfDays))
                        {
                            var percent = x.ItemPrice.CancellationPolicy.CancellationRefundFees.FirstOrDefault(y => y.DaysLeft == numberOfDays).PenaltyPercentage / 100;
                            return x.ItemPrice.Price * percent / 2;
                        }

                        return 0;
                    }

                    var itemPrice = x.ItemPrice;
                    var discount = booking.BookingDetails.Sum(y => y.ItemPrice.Price) - booking.AmountPaid;
                    var couponPercent = itemPrice.Price / booking.BookingDetails.Sum(y => y.ItemPrice.Price);
                    var commission = itemPrice.Price * (itemPrice.CancellationPolicy.Commission / 100);

                    return (decimal)(itemPrice.Price - (1 - couponPercent * discount) - commission);
                })(),
                x.ItemPrice.Item.UserID,
                x.ItemPrice.Item.User.FullName,
            }).GroupBy(x => new
            {
                x.UserID,
                x.FullName,
            }).Select(x => new
            {
                x.Key.UserID,
                x.Key.FullName,
                Total = x.Sum(y => y.Total),
                TotalNetRevenue = x.Sum(y => y.NetPrice),
                Totalbalance = x.Sum(y => y.Balance),
            }).ToList();

            var allusers = ent.Users.Where(x => x.Items.Count() != 0).ToList();

            dataGridView3.Rows.Clear();

            foreach (var user in allusers)
            {
                if (users.Any(x => x.UserID == user.ID))
                {
                    var tempUser = users.FirstOrDefault(x => x.UserID == user.ID);
                    dataGridView3.Rows.Add(tempUser.UserID, tempUser.FullName, "$" + tempUser.Total.ToString("0.00"), "$" + tempUser.TotalNetRevenue.ToString("0.00"), "$" + tempUser.Totalbalance.ToString("0.00"));
                }
                else
                {
                    dataGridView3.Rows.Add(user.ID, user.FullName);
                }
            }
        }

        public class rowData
        {
            public rowData(string date, string amount, string com, string desc)
            {
                this.date = date;
                this.amount = amount;
                this.com = com;
                this.desc = desc;
            }

            public string date { get; set; }
            public string amount { get; set; }
            public string com { get; set; }
            public string desc { get; set; }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var userId = long.Parse(dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString());

            var user = ent.Users.FirstOrDefault(x => x.ID == userId);
            label26.Text = $"Transaction detail for {user.FullName}";

            var trans = ent.Transactions.ToList();

            dataGridView4.Rows.Clear();

            var rowDatas = new List<rowData>();

            foreach (var tran in trans)
            {
                rowDatas.Add(new rowData(tran.TransactionDate.ToString("dd/MM/yyyy"), new Func<string>(() =>
                {
                    var booking = ent.Bookings.FirstOrDefault(x => x.TransactionID == tran.ID);

                    var sum = booking.BookingDetails.Sum(x => x.ItemPrice.Price *  (1 - x.ItemPrice.CancellationPolicy.Commission / 100));

                    return $"$ {sum.ToString("0.00")}";
                })(),
                new Func<string>(() =>
                {
                    var booking = ent.Bookings.FirstOrDefault(x => x.TransactionID == tran.ID);

                    var sum = booking.BookingDetails.Sum(x => x.ItemPrice.Price * x.ItemPrice.CancellationPolicy.Commission / 100);

                    return $"$ {sum.ToString("0.00")}";
                })(),
                new Func<string>(() =>
                {
                    var booking = ent.Bookings.FirstOrDefault(x => x.TransactionID == tran.ID);

                    var firstItemPriceDate = booking.BookingDetails.OrderBy(x => x.ItemPrice.Date).First().ItemPrice.Date;
                    var lastItemPriceDate = booking.BookingDetails.OrderByDescending(x => x.ItemPrice.Date).First().ItemPrice.Date;

                    string str = string.Join(",", booking.BookingDetails.Select(x => x.ItemPrice.Item.Title));
                    return $"Reserve {str} from {firstItemPriceDate.ToString("dd/MM/yyyy")}-{lastItemPriceDate.ToString("dd/MM/yyyy")}";
                })()));
            }

            var bookingRefunds = ent.BookingDetails.ToList().Where(x => x.isRefund == true).ToList();

            foreach (var bookingRefund in bookingRefunds)
            {
                TimeSpan difference = bookingRefund.ItemPrice.Date - (DateTime)bookingRefund.RefundDate;
                var numberOfDays = difference.Days;
                var amount = bookingRefund.ItemPrice.Price;
                var com = 0.00m;

                if (bookingRefund.ItemPrice.CancellationPolicy.CancellationRefundFees.Any(y => y.DaysLeft == numberOfDays))
                {
                    var percent = bookingRefund.ItemPrice.CancellationPolicy.CancellationRefundFees.FirstOrDefault(y => y.DaysLeft == numberOfDays).PenaltyPercentage / 100;
                    amount = bookingRefund.ItemPrice.Price * (1 - percent);
                    com = bookingRefund.ItemPrice.Price * percent / 2;
                }

                rowDatas.Add(new rowData(bookingRefund.RefundDate?.ToString("dd/MM/yyyy"), $"$ {amount.ToString("0.00")}", $"$ {com.ToString("0.00")}", $"Cancel {bookingRefund.ItemPrice.Item.Title} from {bookingRefund.ItemPrice.Date.ToString("dd/MM/yyyy")}"));
            }

            rowDatas = rowDatas.OrderBy(x => x.date).ThenByDescending(x => x.desc).ToList();

            foreach(var rowData in rowDatas)
            {
                dataGridView4.Rows.Add(rowData.date, rowData.amount, rowData.com, rowData.desc);
            }
        }
    }
}
