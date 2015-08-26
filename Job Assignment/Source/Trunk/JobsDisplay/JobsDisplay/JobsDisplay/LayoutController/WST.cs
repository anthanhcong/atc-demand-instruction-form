using System;
using System.Windows.Forms;
using System.Drawing;

using System.Globalization;


namespace LayoutControl
{
    public class WST : Button
    {
        private const string _defaultWorStationName = "WST";
        private const string _defaultWorStationID = "";
        private const string _defaultWorStationFuncDesc = "";
        private Size _defaultWorStationMinSize = new System.Drawing.Size(50, 50);
        private Size _defaultWorStationSize = new System.Drawing.Size(100, 100);
        private Color _defaultWorStationColor = System.Drawing.Color.Aquamarine;
        private Font _defaultFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F,
                                                            System.Drawing.FontStyle.Regular,
                                                            System.Drawing.GraphicsUnit.Point,
                                                            ((byte)(163)));

        public string WST_Name { get; set; }
        public string ID { get; set; }
        public string DescString { get; set; }
        public string ExtraString { get; set; }

        public bool ShowDesStr { get; set; }
        public bool ShowID { get; set; }
        public bool ShowExtraString { get; set; }

        enum WST_Properties
        {
            //Position & size
            Top = 0,
            Left,
            Width,
            Height,

            //Info
            WST_Desc,
            WST_ID,

            //End
            Properties_Terminator,
        }

        private string GetMyString()
        {
            string str = "";

            if (ShowDesStr && (DescString != "")) str += DescString + "\n\n";
            if (ShowID && (ID != "")) str += ID + "\n\n";
            if (ShowExtraString && (ExtraString != "")) str += ExtraString + "\n\n";

            return str;
        }

        public bool CreateBaseObject()
        {

            this.Name = _defaultWorStationName;
            //Style 1-----------------------------

            //this.BackColor = _defaultWorStationColor;
            //this.Font = _defaultFont;
            //this.Size = _defaultWorStationSize;
            //this.UseVisualStyleBackColor = false;

            //Style 2------------------------------
            BackColor = System.Drawing.SystemColors.Control;
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Size = new System.Drawing.Size(96, 78);
            UseVisualStyleBackColor = false;
            //------------------------------------
            this.SetStyle(ControlStyles.Selectable, false);

            this.WST_Name = _defaultWorStationName;
            this.ID = _defaultWorStationID;
            this.DescString = _defaultWorStationFuncDesc;
            this.Text = GetMyString();

            this.MinimumSize = _defaultWorStationMinSize;


            //Assign which info to show
            ShowExtraString = true;
            ShowDesStr = true;
            ShowID = true;
            return true;
        }

        public WST(Point InitPoint, bool allowToModify)
        {
            CreateBaseObject();
            this.Location = InitPoint;

            if (allowToModify)
            {
                LayoutCtrl.Init(this);
            }
        }

        public WST(Point InitPoint)
        {
            CreateBaseObject();
            this.Location = InitPoint;
            LayoutCtrl.Init(this);
        }

        public WST(Point InitPoint, Size size)
        {
            CreateBaseObject();
            this.Location = InitPoint;

            if ((size.Width > _defaultWorStationMinSize.Width) || (size.Height > _defaultWorStationMinSize.Height))
            {
                this.Size = size;
            }
            LayoutCtrl.Init(this);
        }

        public WST(Point InitPoint, Size size, bool allowToModify)
        {
            CreateBaseObject();
            this.Location = InitPoint;

            if ((size.Width > _defaultWorStationMinSize.Width) || (size.Height > _defaultWorStationMinSize.Height))
            {
                this.Size = size;
            }

            if (allowToModify)
            {
                LayoutCtrl.Init(this);
            }
        }

        //public WST(Point InitPoint, Size size)
        //{
        //    CreateBaseObject();
        //    this.Location = InitPoint;

        //    if ((size.Width > _defaultWorStationMinSize.Width) || (size.Height > _defaultWorStationMinSize.Height))
        //    {
        //        this.Size = size;
        //    }
        //    LayoutCtrl.Init(this);
        //}

        public WST(string name, string propertiesStr)
        {
            CreateBaseObject();

            string[] properties = propertiesStr.Split(new[] { "," }, StringSplitOptions.None);
            if (properties.Length == (int)WST_Properties.Properties_Terminator)
            {
                this.Top = int.Parse(properties[(int)WST_Properties.Top]);
                this.Left = int.Parse(properties[(int)WST_Properties.Left]);
                this.Width = int.Parse(properties[(int)WST_Properties.Width]);
                this.Height = int.Parse(properties[(int)WST_Properties.Height]);

                this.DescString = properties[(int)WST_Properties.WST_Desc];
                this.ID = properties[(int)WST_Properties.WST_ID];
                this.Name = name;
                this.Text = GetMyString();

                LayoutCtrl.Init(this);
            }
        }

        public bool BuildPropString(ref string str)
        {
            str = string.Empty;

            CultureInfo cultureInfo = new CultureInfo("en");

            str += this.Name
                    + ":" + this.Top.ToString(cultureInfo)
                    + "," + this.Left.ToString(cultureInfo)
                    + "," + this.Width.ToString(cultureInfo)
                    + "," + this.Height.ToString(cultureInfo)
                    + "," + this.DescString
                    + "," + this.ID
                    + "*";

            return true;
        }

        public void AddContextMenu(MouseEventHandler hanlder)
        {
            this.MouseDown += hanlder;
        }

        public void RefreshInfo()
        {
            this.Text = GetMyString();
        }
    }
}
