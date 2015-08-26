using System;
using System.Windows.Forms;
using System.Drawing;

using System.Globalization;

namespace LayoutControl
{
    public class LineLabel : Label
    {
        private const string defaultName = "Label";
        private const string defaultText = "Line --";

        enum LineLabelProp
        {
            //Position & size
            Top = 0,
            Left,
            Width,
            Height,

            //Info
            ContentString,

            //End
            Properties_Terminator,
        }

        private void CreateBaseObject()
        {
            Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            Location = new System.Drawing.Point(263, 17);
            Size = new System.Drawing.Size(100, 23);
            Text = "Tiltle";
            AutoSize = true;
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }

        public LineLabel(Point initPoint)
        {
            CreateBaseObject();
            this.Location = initPoint;
            LayoutCtrl.Init(this);
        }

        public LineLabel(Point initPoint, bool allowToModify)
        {
            CreateBaseObject();
            this.Location = initPoint;

            if (allowToModify)
            {
                LayoutCtrl.Init(this);
            }
        }


        public LineLabel(string name, string propertiesStr)
        {
            CreateBaseObject();

            string[] properties = propertiesStr.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (properties.Length == (int)LineLabelProp.Properties_Terminator)
            {
                this.Top = int.Parse(properties[(int)LineLabelProp.Top]);
                this.Left = int.Parse(properties[(int)LineLabelProp.Left]);
                this.Width = int.Parse(properties[(int)LineLabelProp.Width]);
                this.Height = int.Parse(properties[(int)LineLabelProp.Height]);

                this.Text = properties[(int)LineLabelProp.ContentString];
                this.Name = name;

                LayoutCtrl.Init(this);
            }
        }

        public LineLabel(string name, string propertiesStr, bool allowToModify)
        {
            CreateBaseObject();

            string[] properties = propertiesStr.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            if (properties.Length == (int)LineLabelProp.Properties_Terminator)
            {
                this.Top = int.Parse(properties[(int)LineLabelProp.Top]);
                this.Left = int.Parse(properties[(int)LineLabelProp.Left]);
                this.Width = int.Parse(properties[(int)LineLabelProp.Width]);
                this.Height = int.Parse(properties[(int)LineLabelProp.Height]);

                this.Text = properties[(int)LineLabelProp.ContentString];
                this.Name = name;

                if (allowToModify)
                {
                    LayoutCtrl.Init(this);                    
                }
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
                    + "," + this.Text
                    + "*";

            return true;
        }

        public void AddContextMenu(MouseEventHandler hanlder)
        {
            this.MouseDown += hanlder;
        }

    }
}
