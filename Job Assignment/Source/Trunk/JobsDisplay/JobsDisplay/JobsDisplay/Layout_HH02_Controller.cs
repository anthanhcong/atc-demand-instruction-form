using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace JobsDisplay
{
    class Layout_HH02_Controller
    {
        private List<Button> _HH02_WSTs = new List<Button>();

        private Color _activeColor = Color.Yellow;
        private Color _readyColor = Color.Green;
        private Color _inActiveColor = SystemColors.Control;
        private Color _altColorForBlink = SystemColors.ControlLightLight;
        private Form _myForm;
        public int _blinkingTime = 10;
        
        public Timer _tmr;
        private Button _currActiveWorkStation;

        public Layout_HH02_Controller(Form form)
        {
            _myForm = form;
            _currActiveWorkStation = null;

            _tmr = new Timer();
            _tmr.Stop();
            _tmr.Interval = 400;
            _tmr.Tick += new EventHandler(_tmr_Tick);
        }

        public void _tmr_Tick(object sender, EventArgs e)
        {
            if (_currActiveWorkStation.BackColor == _activeColor)
            {
                _currActiveWorkStation.BackColor = _altColorForBlink;
            }
            else
            {
                _currActiveWorkStation.BackColor = _activeColor;
            }

            _blinkingTime -= (_blinkingTime > 0) ? 1 : 0;

            if (_blinkingTime == 0)
            {
                _myForm.Close();
            }
        }

        public void SetInActiveColor(Color color)
        {
            _inActiveColor = color;
        }

        public void SetReadyColor(Color color)
        {
            _readyColor = color;
        }

        public void SetBlinkingSpeed(int timeToChange)
        {
            _tmr.Interval = (timeToChange > 0) ? timeToChange : _tmr.Interval;
        }

        public void SetAltColorForBlink(Color color)
        {
            _altColorForBlink = color;
        }

        public void SetActiveColor(Color color)
        {
            _activeColor = color;
        }

        public void Add(Button workStation)
        {
            _HH02_WSTs.Add(workStation);
        }

        public void SetInactiveWorkStation(string workStationName)
        {
            Button workStation = null;

            workStation = _HH02_WSTs.Find(c => (c.Name == workStationName));

            if (workStation != null)
            {
                workStation.BackColor = _inActiveColor;         
            }
        }

        public void SetInactiveLine()
        {
            foreach (var item in _HH02_WSTs)
            {
                item.BackColor = _inActiveColor;
            }
        }

        public void SetActive(string workStationName)
        {
            SetInactiveLine();

            _currActiveWorkStation = _HH02_WSTs.Find(c => (c.Name == workStationName.Trim()));

            if (_currActiveWorkStation != null)
            {
                _currActiveWorkStation.BackColor = _activeColor;
                _currActiveWorkStation.Select();
                _blinkingTime = 10;
                _tmr.Start();
            }
        }

        public void SetStaticActive(string workStationName)
        {
            // SetInactiveLine();

            _currActiveWorkStation = _HH02_WSTs.Find(c => (c.Name == workStationName.Trim()));

            if (_currActiveWorkStation != null)
            {
                _currActiveWorkStation.BackColor = _activeColor;
                _currActiveWorkStation.Select();
                _tmr.Start();
            }
        }

        public void SetReady_WST(string workStationName)
        {
            // SetInactiveLine();
            Button workStation = null;
            workStationName = workStationName.Replace('-', '_');
            string[] temp = workStationName.Split('$');
            string msnv = "";
            workStationName = temp[0].Trim();
            if (temp.Length > 1)
            {
                msnv = temp[1].Trim();
            }

            workStation = _HH02_WSTs.Find(c => (c.Name == workStationName));


            if (workStation != null)
            {
                if (workStation.BackColor != _readyColor)
                {
                    workStation.BackColor = _readyColor;
                    if (msnv != "")
                    {
                        workStation.Text += "\n" + msnv;
                    }
                }
            }
        }
    }
}
