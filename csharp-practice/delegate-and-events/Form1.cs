using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csharp_practice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            calculator1.OnCalculationComplete += Calculator1_OnCalculationComplete;

        }

        private void _FillLabel(string text)
        {
            label1.Text = text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.TextBoxFilled += _FillLabel;
            form2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Calculator1_OnCalculationComplete(object sender, Calculator.CalculationCompleteEventArgs e)
        {
            lblResult.Text = $"Num1 = {e.Num1},  Num2 = {e.Num2}, Num3 = {e.Result}";
        }
    }
}
