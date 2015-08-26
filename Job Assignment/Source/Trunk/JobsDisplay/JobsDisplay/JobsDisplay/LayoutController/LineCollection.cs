using System;
using System.Collections.Generic;
using System.Linq;

namespace LayoutControl
{
    class LineCollection
    {
        private List<LineDataBuilder> _linesList = new List<LineDataBuilder>();

        public LineCollection()
        {
            
        }

        public bool IsThisLineExist(Line line)
        {
            var itemToRemove = _linesList.SingleOrDefault(r => r.LineName == line.LineName);

            if (itemToRemove != null)
            {
                return true;
            }

            return false;
        }

        public void AddLineData(Line line)
        {
            LineDataBuilder data = line.ExportLineDataBuilder();

            _linesList.Add(data);
        }

        public bool RemoveLineData(Line line)
        {
            var itemToRemove = _linesList.SingleOrDefault(r => r.LineName == line.LineName);

            if (itemToRemove != null)
            {
                _linesList.Remove(itemToRemove);
                return true;
            }

            return false;
        }

        public bool OverwriteLineData(Line line)
        {
            var itemToRemove = _linesList.SingleOrDefault(r => r.LineName == line.LineName);

            if (itemToRemove != null)
            {
                _linesList.Remove(itemToRemove);
            }

            LineDataBuilder data = line.ExportLineDataBuilder();

            _linesList.Add(data);

            return true;
        }

        //public bool BuildGraphicLine( string lineName)
        //{
        //    foreach (var item in _linesList)
        //    {
        //        if (item.LineName == lineName)
        //        {
        //            lblData = item.LableDataString;
        //            wstData = item.WSTDataString;
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public List<String> GetListOfLineName()
        {
            List<String> str = new List<string>();

            foreach (var item in _linesList)
            {
                str.Add(item.LineName);
            }
            
            return str;
        }

        public LineDataBuilder GetLineData(string lineName)
        {
            foreach (var item in _linesList)
            {
                if (item.LineName == lineName)
                {
                    return item;
                }
            }

            return null;
        }
    }
}
