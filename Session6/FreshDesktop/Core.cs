using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreshDesktop
{
    public partial class Core : Form
    {
        public Core()
        {
            InitializeComponent();
        }

        PrivateFontCollection pfc = new PrivateFontCollection();

        private void Core_Load(object sender, EventArgs e)
        {
            try
            {
                this.panel1.BackColor = Color.FromArgb(85, 85, 85);

                SetStyle();

                pfc.AddFontFile("OpenSans-Regular.ttf");

                this.panel1.Font = new Font(pfc.Families[0], 9f);
                // this.panel1.ForeColor = Color.FromArgb(187, 187, 187);
            }
            catch (Exception)
            {

            }

            SetStyle();
        }

        void SetStyle()
        {
            foreach (var control in this.panel1.Controls)
            {
                if (control.GetType() == typeof(Button))
                {
                    ((Button)control).BackColor = Color.FromArgb(51, 51, 51);
                    // ((Button)control).ForeColor = Color.FromArgb(187, 187, 187);

                    if (((Button)control).Text.ToLower().Contains("cancel") || ((Button)control).Text.ToLower().Contains("delete"))
                    {
                        ((Button)control).ForeColor = ColorTranslator.FromHtml("#e51a2e");
                    }
                }
            }
        }

        protected bool validateInput(Control control)
        {
            foreach (var c in control.Controls)
            {
                if (c.GetType() == typeof(TextBox) && string.IsNullOrEmpty(((TextBox)c).Text))
                {
                    MessageBox.Show("all fields are required.");
                    return false;
                }

                if (c.GetType() == typeof(NumericUpDown) && ((NumericUpDown)c).Value <= 0)
                {
                    MessageBox.Show("all fields couldn't be zero or lesser value.");
                    return false;
                }

                if (c.GetType() == typeof(ComboBox) && ((ComboBox)c).SelectedIndex == -1)
                {
                    MessageBox.Show("drop down is not selected");
                    return false;
                }
            }

            return true;
        }
    }
}
