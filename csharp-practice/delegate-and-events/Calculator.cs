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
    public partial class Calculator : UserControl
    {
        public class CalculationCompleteEventArgs : EventArgs
        {
            public decimal Result { get; }
            public decimal Num1 { get; }
            public decimal Num2 { get; }
        
            public CalculationCompleteEventArgs(decimal result, decimal num1, decimal num2)
            {
                Result = result;
                Num1 = num1;
                Num2 = num2;
            }
        }
        public event EventHandler<CalculationCompleteEventArgs> OnCalculationComplete;
        protected virtual void RaiseOnCalculationComplete(CalculationCompleteEventArgs e)
        {
            OnCalculationComplete?.Invoke(this, e);
        }
        public void RaiseOnCalculationComplete()
        {
            RaiseOnCalculationComplete(new CalculationCompleteEventArgs(_Result, _Num1, _Num2));
        }


        public Calculator()
        {
            InitializeComponent();
        }

        private decimal _Num1 = 0, _Num2 = 0, _Result = 0;
        private bool IsValidNumbers()
        {
            if (!string.IsNullOrWhiteSpace(txbNum1.Text) && !string.IsNullOrWhiteSpace(txbNum2.Text)
                && decimal.TryParse(txbNum1.Text, out decimal Num1) && decimal.TryParse(txbNum2.Text, out decimal Num2))
            {
                _Num1 = Num1;
                _Num2 = Num2;
                
                return true;
            }
            else
                return false;

        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsValidNumbers())
                _Result = _Num1 + _Num2;

            lblResult.Text = _Result.ToString();
            RaiseOnCalculationComplete();
        }
        private void btnSubstract_Click(object sender, EventArgs e)
        {
            if (IsValidNumbers())
                _Result = _Num1 - _Num2;

            lblResult.Text = _Result.ToString();
            RaiseOnCalculationComplete();

        }
        private void btnMultiply_Click(object sender, EventArgs e)
        {
            if (IsValidNumbers())
                _Result = _Num1 * _Num2;

            lblResult.Text = _Result.ToString();
            RaiseOnCalculationComplete();

        }
        private void btnDivid_Click(object sender, EventArgs e)
        {
            if (IsValidNumbers() && _Num2 != 0)
                _Result = _Num1 / _Num2;

            lblResult.Text = _Result.ToString();
            RaiseOnCalculationComplete();

        }
    }
}
