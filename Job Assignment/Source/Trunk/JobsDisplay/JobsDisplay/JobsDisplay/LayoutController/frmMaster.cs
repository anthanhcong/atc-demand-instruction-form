using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LayoutControl
{
    public partial class frmMaster : Form
    {
        private Line _line;
        private LineCollection _lineCollection;
        private string _layoutLabelString;
        private string _layoutWSTString;

        string Connect_Str = "";

        public frmMaster(string con_str)
        {
            Connect_Str = con_str;
            InitializeComponent();
            LoadLinesFromDatabase();
            
        }

        private void btn_AddWST_Click(object sender, EventArgs e)
        {
            if (CheckLineExist() == false)
            {
                return;
            }

            Point initPoint = new Point(0, 0);
            WST wst = new WST(initPoint);

            _line.Add_Object(wst);
        }

        private void btn_AddLayout_Click(object sender, EventArgs e)
        {
            //Clean all item in the pannel
            pnl_Main.Controls.Clear();

            //txt_LineName.Text = "";
            //txt_LineName.BackColor = Color.LightYellow;
            //txt_LineName.Focus();

            //Create a new line
            _line = new Line(null, pnl_Main);
            _line.LineName = "HH01";

            MessageBox.Show("A new line layout has been added. Please add the name for this line first",
                            "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_ClearAllWST_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Clear ALL workstations ? ",
                "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (_line != null)
            {
                _line.Remove_All_WST();
            }

            MessageBox.Show("All workstation have been removed",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btn_SaveLayout_Click(object sender, EventArgs e)
        {
            if (_line != null)
            {
                _line.GetLocationInfo(out _layoutLabelString, out _layoutWSTString);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_line != null)
            {
                _line.BuildLineFromString(_layoutLabelString, _layoutWSTString);
            }

        }

        public bool CheckLineExist()
        {
            if (_line == null)
            {
                MessageBox.Show("Please ADD a new Layout first !!!",
                    "Warning !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        private void btn_AddLabel_Click(object sender, EventArgs e)
        {
            if (CheckLineExist() == false)
            {
                return;
            }

            Point initPoint = new Point(500, 0);
            LineLabel lbl = new LineLabel(initPoint);

            _line.Add_Object(lbl);
        }

        private void btn_ClearAllLabels_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Clear ALL labels ? ",
                                                    "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            if (_line != null)
            {
                _line.Remove_All_Label();
            }

            MessageBox.Show("All labels have been removed",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_AddToGallery_Click(object sender, EventArgs e)
        {
            //if (_line == null)
            //{
            //    MessageBox.Show("There is no layout to add",
            //        "Warning !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    return;
            //}

            //if (_lineCollection == null)
            //{
            //    _lineCollection = new LineCollection();
            //}

            //if (txt_LineName.Text == "")
            //{
            //    MessageBox.Show("Please choose a name for this line",
            //        "Warning !!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            //    //txt_LineName.Focus();

            //    return;
            //}

            //_line.LineName = txt_LineName.Text;
            //if (_lineCollection.IsThisLineExist(_line))
            //{
            //    DialogResult result =  MessageBox.Show(_line.LineName + " already exist, OVERWRITE IT ? \nPress Yes if you want to overwrite \nOtherwise, please press No and choose another name",
            //        "Warning !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            //    if (result == System.Windows.Forms.DialogResult.No)
            //    {
            //        return;
            //    }
            //    else
            //    {
            //        _lineCollection.RemoveLineData(_line);
            //    }
            //}

            //_line.LineName = txt_LineName.Text;
            //_lineCollection.AddLineData(_line);

            //List<String> str = _lineCollection.GetListOfLineName();

            //lst_Layout.Items.Clear();

            //foreach (var item in str)
            //{
            //    lst_Layout.Items.Add(item);
            //}
        }

        private void lst_Layout_SelectedIndexChanged(object sender, EventArgs e)
        {
            string lineName = lst_Layout.SelectedItem.ToString();

            if (lineName != "")
            {
                //Draw on screen. Assign to _line
                if (_line != null)
                {
                    //There is another layout is shown on the screen --> Clean it
                    _line.Remove_All_Label();
                    _line.Remove_All_WST();                    
                }

                //Draw another line
                _line = new Line(null, pnl_Main, true, false);
                _line.LineName = lineName;
                //txt_LineName.Text = lineName;

                //Get the list of wst from database
                LinesColletion_DataBase conn = new LinesColletion_DataBase(Connect_Str);

                string lineID = conn.FindLineID(lineName);

                if (lineID == "")
                {
                    return;
                }

                DataTable wstList = conn.Load_List_of_WST(lineID);

                foreach (DataRow row in wstList.Rows)
                {
                    int x = (int)(row["WST_x"].ToString() != "" ? row["WST_x"] : 0);
                    int y = (int)(row["WST_y"].ToString() != "" ? row["WST_y"] : 0);

                    int w = (int)(row["WST_width"].ToString() != "" ? row["WST_width"] : 0);
                    int h = (int)(row["WST_heigh"].ToString() != "" ? row["WST_heigh"] : 0);


                    Point initPoint = new Point(x, y);
                    Size initSize = new Size(w, h);
                    WST wst = new WST(initPoint, initSize);
                    wst.DescString = row["WST_Name"].ToString();
                    wst.ID = row["WST_ID"].ToString();
                    wst.RefreshInfo();
                    _line.Add_Object(wst);
                }




                //LineDataBuilder data = _lineCollection.GetLineData(lst_Layout.SelectedItem.ToString());
                
                //if (data != null)
                //{
                //    //Clearn all current drawing on the panel,
                //    _line.Remove_All_Label();
                //    _line.Remove_All_WST();

                //    _line = new Line(pnl_Main, data);
                //    txt_LineName.Text = data.LineName;
                //}
            }
        }

        private void btn_RemoveFromGallery_Click(object sender, EventArgs e)
        {
            if (lst_Layout == null)
            {
                return;
            }

            if (lst_Layout.SelectedItem.ToString() == "")
            {
                return;
            }

            DialogResult result = MessageBox.Show("Do you want to remove " + _line.LineName + " from Gallery ?",
                "Warning !!!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

             //Clearn all current drawing on the panel,
            _line.Remove_All_Label();
            _line.Remove_All_WST();

            //LineDataBuilder data = _lineCollection.GetLineData(lst_Layout.SelectedItem.ToString());
            _lineCollection.RemoveLineData(_line);

            List<String> str = _lineCollection.GetListOfLineName();

            lst_Layout.Items.Clear();
            //txt_LineName.Text = "";

            foreach (var item in str)
            {
                lst_Layout.Items.Add(item);
            }

        }

        private void btn_LoadFromDatabase_Click(object sender, EventArgs e)
        {
            LoadLinesFromDatabase();
        }

        public void LoadLinesFromDatabase( )
        {
            LinesColletion_DataBase conn = new LinesColletion_DataBase(Connect_Str);

            DataTable lineList = conn.Load_List_of_Line();

            lst_Layout.Items.Clear();

            foreach (DataRow row in lineList.Rows)
            {
                lst_Layout.Items.Add(row["Line_Name"].ToString());
            }
        }

        private void btn_SaveToDataBase_Click(object sender, EventArgs e)
        {
            if (_line == null)
            {
                //There is no line active, nothing to save
                MessageBox.Show("There is nothing to save",
                                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            LinesColletion_DataBase conn = new LinesColletion_DataBase(Connect_Str);

            conn.Save_LineInfo(_line);

            MessageBox.Show("The changes on layout have been saved",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
