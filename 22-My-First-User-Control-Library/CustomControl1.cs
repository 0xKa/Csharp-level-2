using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _22_My_First_User_Control_Library
{
    //You can inherit from any control, to extend its features
    public partial class CustomControl1 : TextBox
    {
        public CustomControl1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        //add a property to check if it's required, numric, text, etc...

    }
}
