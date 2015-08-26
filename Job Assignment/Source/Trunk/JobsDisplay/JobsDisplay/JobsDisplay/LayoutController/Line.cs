using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

using System;

namespace LayoutControl
{
    public class Line
    {
        public Color ActiveColor { get; set; }
        public Color InActiveColor { get; set; }
        public Color ReadyColor { get; set; }
        public Color FRU_Inactive { get; set; }
        public Color FRU_Active { get; set; }
        public Color DisableColor { get; set; }
        public Color AltColorForBlinking { get; set; }
        public string LineTile { get; set; }
        public string LineName { get; set; }
        public int BlinkingTime { get; set; }
        public int BlinkingSpeed { get; set; }

        private Color _defaultActiveColor = Color.LightYellow;
        private Color _defaultReadyColor = Color.LightGreen;
        private Color _defaultInActiveColor = Color.Red;
        private Color _defaultDisableColor = Color.LightGray;
        private Color _defaultAltColorForBlink = Color.Yellow;
        private Color _defaultFruInactive = Color.Orange;
        private Color _defaultFruActive = Color.SkyBlue;

        private int _defaultBlinkingTime = 10;
        public Timer _tmr;
        private WST _currActiveWorkStation;
        private int _blinkingTime;

        private List<WST> _wstList = new List<WST>();
        private List<LineLabel> _lblList = new List<LineLabel>();

        private MouseEventHandler MouseClickHandler { get; set; }
        private MouseEventHandler MouseLabelClickHandler { get; set; }

        private WST _lastContextMenuSource;
        private LineLabel _lastLabelContextMenuSource;

        private ContextMenuStrip contextMenu;
        private ToolStripMenuItem editWorkStationInfoToolStripMenuItem;
        private ToolStripMenuItem removeThisWorkStationToolStripMenuItem;

        private ContextMenuStrip contextMenuLabel;
        private ToolStripMenuItem editContentToolStripMenuItem;
        private ToolStripMenuItem removeThisLabelToolStripMenuItem;

        private Panel _linePannel;
        private Form _lineForm;
        private int _index;
        private bool _allowObjectModification = true;

        private void CreateContextMenu()
        {
            #region Init the context menu
            contextMenu = new System.Windows.Forms.ContextMenuStrip();
            editWorkStationInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeThisWorkStationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuLabel = new System.Windows.Forms.ContextMenuStrip();
            editContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            removeThisLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editWorkStationInfoToolStripMenuItem,
            this.removeThisWorkStationToolStripMenuItem});
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(208, 48);
            // 
            // editWorkStationInfoToolStripMenuItem
            // 
            this.editWorkStationInfoToolStripMenuItem.Name = "editWorkStationInfoToolStripMenuItem";
            this.editWorkStationInfoToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.editWorkStationInfoToolStripMenuItem.Text = "Edit WorkStation Info";
            this.editWorkStationInfoToolStripMenuItem.Click += new System.EventHandler(this.editWorkStationInfoToolStripMenuItem_Click);
            // 
            // removeThisWorkStationToolStripMenuItem
            // 
            this.removeThisWorkStationToolStripMenuItem.Name = "removeThisWorkStationToolStripMenuItem";
            this.removeThisWorkStationToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.removeThisWorkStationToolStripMenuItem.Text = "Remove this WorkStation";
            this.removeThisWorkStationToolStripMenuItem.Click += new System.EventHandler(this.removeThisWorkStationToolStripMenuItem_Click);
            // 
            // contextMenuLabel
            // 
            this.contextMenuLabel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editContentToolStripMenuItem,
            this.removeThisLabelToolStripMenuItem});
            this.contextMenuLabel.Name = "contextMenuLabel";
            this.contextMenuLabel.Size = new System.Drawing.Size(168, 48);
            // 
            // editContentToolStripMenuItem
            // 
            this.editContentToolStripMenuItem.Name = "editContentToolStripMenuItem";
            this.editContentToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.editContentToolStripMenuItem.Text = "Edit Content";
            this.editContentToolStripMenuItem.Click += new System.EventHandler(this.editContentToolStripMenuItem_Click);
            // 
            // removeThisLabelToolStripMenuItem
            // 
            this.removeThisLabelToolStripMenuItem.Name = "removeThisLabelToolStripMenuItem";
            this.removeThisLabelToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.removeThisLabelToolStripMenuItem.Text = "Remove this label";
            this.removeThisLabelToolStripMenuItem.Click += new System.EventHandler(this.removeThisLabelToolStripMenuItem_Click);
            #endregion

            MouseClickHandler = buttonClickHandler;
            MouseLabelClickHandler = LabelClickHandler;
        }

        private void LoadDefaultData()
        {
            ActiveColor = _defaultActiveColor;
            InActiveColor = _defaultInActiveColor;
            ReadyColor = _defaultReadyColor;
            DisableColor = _defaultDisableColor;
            FRU_Inactive = _defaultInActiveColor;
            FRU_Active = _defaultFruActive;
            AltColorForBlinking = _defaultAltColorForBlink;
            BlinkingTime = _defaultBlinkingTime;
            BlinkingSpeed = 300;


            _tmr = new Timer();
            _tmr.Stop();
            _tmr.Interval = BlinkingSpeed;
            _tmr.Tick += new EventHandler(_tmr_Tick);
        }
        public Line(Form frm, Panel containner)
        {
            _index = 0;
            _linePannel = containner;
            _lineForm = frm;
            
            LoadDefaultData();

            CreateContextMenu();
        }

        public Line(Form frm, Panel containner, bool allowObjectModification, bool allowUsingMenu)
        {
            _index = 0;
            _linePannel = containner;
            _lineForm = frm;

            LoadDefaultData();


            _allowObjectModification = allowObjectModification;

            if (allowUsingMenu)
            {
                CreateContextMenu();
            }
        }

        public Line(Form frm, Panel containner, LineDataBuilder data)
        {
            _index = 0;
            _linePannel = containner;
            _lineForm = frm;

            LoadDefaultData();

            CreateContextMenu();

            this.LineName = data.LineName;
            this.LineTile = data.LineTitle;

            BuildLineFromDataBuilder(data);
        }

        #region Functions for Context Menu
        private void buttonClickHandler(object sender, MouseEventArgs e)
        {
            if (_allowObjectModification == false)
            {
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _lastContextMenuSource = (WST)sender;
                contextMenu.Show(Cursor.Position);
            }
        }

        private void editWorkStationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WST_DTO.WST_ID = _lastContextMenuSource.ID;
            WST_DTO.WST_Desc = _lastContextMenuSource.DescString;

            Form info = new frmInfo();
            info.ShowDialog();

            _lastContextMenuSource.ID = WST_DTO.WST_ID;
            _lastContextMenuSource.DescString = WST_DTO.WST_Desc;
            _lastContextMenuSource.RefreshInfo();
        }

        private void removeThisWorkStationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Object(_lastContextMenuSource);
        }

        private void editContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Label_DTO.Content = _lastLabelContextMenuSource.Text;

            Form info = new frmInfoLabel();
            info.ShowDialog();
            _lastLabelContextMenuSource.Text = Label_DTO.Content;
        }

        private void removeThisLabelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Remove_Object(_lastLabelContextMenuSource);
        }

        private void LabelClickHandler(object sender, MouseEventArgs e)
        {
            if (_allowObjectModification == false)
            {
                return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _lastLabelContextMenuSource = (LineLabel)sender;
                contextMenuLabel.Show(Cursor.Position);
            }
        }
        #endregion

        #region Manipulate Object
        public void Add_Object (WST wst)
        {
            wst.Name = _index.ToString();
            wst.AddContextMenu(MouseClickHandler);

            _index++;
            _wstList.Add(wst);
            _linePannel.Controls.Add(wst);
        }

        private void Add_ContextMenuForObject()
        {
            foreach (var item in _wstList)
            {
                item.AddContextMenu(MouseClickHandler);
            }
        }

        public void Add_Object(LineLabel lbl)
        {
            lbl.Name = _index.ToString();
            lbl.AddContextMenu(MouseLabelClickHandler);

            _index++;
            _lblList.Add(lbl);
            _linePannel.Controls.Add(lbl);
        }

        public void Remove_Object(LineLabel lbl)
        {
            var itemToRemove = _lblList.SingleOrDefault(r => r.Name == lbl.Name);

            if (itemToRemove != null)
            {
                _lblList.Remove(itemToRemove);

                if (_linePannel.Controls.Contains(itemToRemove))
                {
                    _linePannel.Controls.Remove(itemToRemove);
                }
            }
        }

        public void Remove_Object(WST wst)
        {
            var itemToRemove = _wstList.SingleOrDefault(r => r.Name == wst.Name);

            if (itemToRemove != null)
            {
                _wstList.Remove(itemToRemove);

                if (_linePannel.Controls.Contains(itemToRemove))
                {
                    _linePannel.Controls.Remove(itemToRemove);
                }
            }
        }

        public void Remove_All_WST()
        {
            foreach (var itemToRemove in _wstList)
            {
                if (_linePannel.Controls.Contains(itemToRemove))
                {
                    _linePannel.Controls.Remove(itemToRemove);
                }
            }

            _wstList.Clear();
        }

        public void Remove_All_Label()
        {
            foreach (var itemToRemove in _lblList)
            {
                if (_linePannel.Controls.Contains(itemToRemove))
                {
                    _linePannel.Controls.Remove(itemToRemove);
                }
            }

            _lblList.Clear();
        }

        #endregion

        #region Save and Load Line Using String

        public bool GetLocationInfo(out string lblStr, out string wstStr)
        {
            lblStr = string.Empty;
            foreach (LineLabel control in _lblList)
            {
                string str = string.Empty;

                control.BuildPropString(ref str);
                lblStr += str;
            }

            wstStr = string.Empty;
            foreach (WST control in _wstList)
            {
                string str = string.Empty;

                control.BuildPropString(ref str);
                wstStr += str;
            }

            return true;
        }

        public void BuildLineFromString(string lblStr, string wstStr)
        {
            //Clean all object
            Remove_All_Label();
            Remove_All_WST();

            //Rebuild all object
            if (lblStr != "")
            {
                CreateLabelsFromString(_linePannel, lblStr);
            }

            if (wstStr != "")
            {
                CreateWorkStationsFromString(_linePannel, wstStr);
            }
        }

        public void BuildLineFromDataBuilder(LineDataBuilder data)
        {
            BuildLineFromString(data.LableDataString, data.WSTDataString);
        }

        private void CreateWorkStationsFromString(Panel _linePannel, string objStr)
        {
            string[] controlsInfo = objStr.Split(new[] { "*" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string controlInfo in controlsInfo)
            {
                string[] info = controlInfo.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                WST obj = new WST(info[0], info[1]);
                Add_Object(obj);
            }
        }

        private void CreateLabelsFromString(Panel _linePannel, string objStr)
        {
            string[] controlsInfo = objStr.Split(new[] { "*" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string controlInfo in controlsInfo)
            {
                string[] info = controlInfo.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                LineLabel obj = new LineLabel(info[0], info[1]);
                Add_Object(obj);
            }
        }

        public LineDataBuilder ExportLineDataBuilder()
        {
            LineDataBuilder line = new LineDataBuilder();
            line.LineName = this.LineName;
            line.LineTitle = this.LineTile;

            string lblStr, wstStr;

            GetLocationInfo(out lblStr, out wstStr);
            line.LableDataString = lblStr;
            line.WSTDataString = wstStr;

            return line;
        }

        public bool Get_WST_LocationAndSize(string WST_ID, out int x, out int y, out int w, out int h)
        {
            foreach (WST wst in _wstList)
            {
                if (wst.ID == WST_ID)
                {
                    x = wst.Location.X;
                    y = wst.Location.Y;
                    w = wst.Size.Width;
                    h = wst.Size.Height;
                    return true;
                }
            }

            x = 0;
            y = 0;
            w = 0;
            h = 0;            
            return false;
        }
        #endregion

        #region Display WST
        public void _tmr_Tick(object sender, EventArgs e)
        {
            if (_currActiveWorkStation == null)
            {
                _tmr.Stop();
                return;
            }

            if (_currActiveWorkStation.BackColor == ActiveColor)
            {
                _currActiveWorkStation.BackColor = AltColorForBlinking;
            }
            else
            {
                _currActiveWorkStation.BackColor = ActiveColor;
            }

            _blinkingTime -= (_blinkingTime > 0) ? 1 : 0;

            if (_blinkingTime == 0)
            {
                _tmr.Stop();
                if (_lineForm != null)_lineForm.Close();
            }
        }

        public void SetInactiveWST(string workStationID)
        {
            WST workStation = null;

            workStation = _wstList.Find(c => (c.ID == workStationID));

            if (workStation != null)
            {
                workStation.BackColor = InActiveColor;
                workStation.ExtraString = "";
                workStation.RefreshInfo();
            }
        }

        public void SetActiveWST(string workStationID, string msnv)
        {
            _tmr.Stop();
            _currActiveWorkStation = _wstList.Find(c => (c.ID == workStationID.Trim()));

            if (_currActiveWorkStation != null)
            {
                _currActiveWorkStation.BackColor = ActiveColor;
                _currActiveWorkStation.ExtraString = msnv;
                _currActiveWorkStation.Select();
                _currActiveWorkStation.RefreshInfo();
                StartActiveTimer();
            }
        }

        public void SetReady_WST(string workStationID, string msnv)
        {
            WST workStation = null;

            workStation = _wstList.Find(c => (c.ID == workStationID));


            if (workStation != null)
            {
                if (workStation.BackColor != ReadyColor)
                {
                    workStation.BackColor = ReadyColor;
                }

                workStation.ExtraString = msnv;
                workStation.RefreshInfo();
            }
        }

        public void SetReady_FRU_WST(string workStationID, string msnv)
        {
            WST workStation = null;

            workStation = _wstList.Find(c => (c.ID == workStationID));


            if (workStation != null)
            {
                if (workStation.BackColor != FRU_Active)
                {
                    workStation.BackColor = FRU_Active;
                }

                workStation.ExtraString = msnv;
                workStation.RefreshInfo();
            }
        }

        public void SetReady_FRU_Inactive_WST(string workStationID)
        {
            WST workStation = null;

            workStation = _wstList.Find(c => (c.ID == workStationID));


            if (workStation != null)
            {
                if (workStation.BackColor != FRU_Inactive)
                {
                    workStation.BackColor = FRU_Inactive;
                }
                workStation.RefreshInfo();
            }
        }


        public void SetDisable_WST(string workStationID)
        {
            WST workStation = null;

            workStation = _wstList.Find(c => (c.ID == workStationID));


            if (workStation != null)
            {
                if (workStation.BackColor != DisableColor)
                {
                    workStation.BackColor = DisableColor;
                }
                workStation.RefreshInfo();
            }
        }

        public void SetInactiveLine()
        {
            foreach (var item in _wstList)
            {
                item.BackColor = InActiveColor;
                item.ExtraString = "";
                item.RefreshInfo();
            }
        }

        public void SetDisableLine()
        {
            foreach (var item in _wstList)
            {
                item.BackColor = DisableColor;
                item.ExtraString = "";
                item.RefreshInfo();
            }
        }

        private void StartActiveTimer()
        {
            _blinkingTime = BlinkingTime;
            _tmr.Interval = BlinkingSpeed;
            _tmr.Start();
        }
        #endregion

    }
}
