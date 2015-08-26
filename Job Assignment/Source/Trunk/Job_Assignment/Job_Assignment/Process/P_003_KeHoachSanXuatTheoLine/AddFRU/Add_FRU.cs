using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AddFRU
{
    public partial class Add_FRU : Form
    {
        public string Shift;
        public int NumWST;

        public Add_FRU()
        {
            InitializeComponent();
            OK_BT.DialogResult = DialogResult.OK;
            Cancel_BT.DialogResult = DialogResult.Cancel;
            this.AcceptButton = OK_BT;
            this.CancelButton = Cancel_BT;
        }

        private void Shift_CheckChange(object sender, EventArgs e)
        {
            if (Shift_1.Checked) Shift = "Shift_1";
            else if (Shift_2.Checked) Shift = "Shift_2";
            else if (Shift_3.Checked) Shift = "Shift_3";
            else Shift = "Shift_1";
        }

        private void NumOfShift_TextChanged(object sender, EventArgs e)
        {
            string input = NumOfWST.Text ;
            if (input.Length > 0)
            {
                bool isValid = Regex.IsMatch(input, @"^[0-9]+$");
                if (isValid == false)
                {
                    NumOfWST.Text = NumOfWST.Text.Substring(0, NumOfWST.Text.Length - 1);
                }
            }
        }

        private void OK_BT_Click(object sender, EventArgs e)
        {
            try
            {
                NumWST = Convert.ToInt32(NumOfWST.Text.Trim());
            }
            catch
            {
                MessageBox.Show("Can not get num of WST input.\nWill get Default is 1");
                NumWST = 1;
            }
        }
    }
}
