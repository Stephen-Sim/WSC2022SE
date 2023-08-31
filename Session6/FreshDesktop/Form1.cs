using FreshDesktop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    }
}
