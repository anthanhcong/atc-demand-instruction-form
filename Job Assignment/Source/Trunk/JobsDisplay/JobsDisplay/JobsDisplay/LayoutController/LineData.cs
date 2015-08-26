namespace LayoutControl
{
    public class LineDataBuilder
    {
        public string  LineName { get; set; }
        public string LineTitle { get; set; }

        public string LableDataString { get; set; }
        public string WSTDataString { get; set; }

        public LineDataBuilder()
        {
            LineName = string.Empty;
            LineTitle = string.Empty;
            LableDataString = string.Empty;
            WSTDataString = string.Empty;
        }
    }
}
