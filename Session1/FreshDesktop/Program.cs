using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreshDesktop
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (Properties.Settings.Default.UserTypeId == 1)
            {
                Application.Run(new EmployeeManagementForm(Properties.Settings.Default.UserTypeId));
            }
            else if (Properties.Settings.Default.UserTypeId != 0)
            {
                Application.Run(new UserManagementForm(Properties.Settings.Default.UserTypeId));
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
