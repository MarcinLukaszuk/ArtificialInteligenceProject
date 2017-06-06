using System;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;

namespace ProjektSI
{


    public class FieldColorConverter : IValueConverter
    {

        public Brush WallBrush { get; set; }
        public Brush HallBrush { get; set; }
        public Brush StartNodeBrush { get; set; }
        public Brush VisitedNodeBrush { get; set; }
        public Brush EndNodeBrush { get; set; }
        public Brush SpecialNodeBrush { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Block val = (Block)value;
            if (val == Block.Wall)
                return WallBrush;
            else if (val == Block.Start)
                return StartNodeBrush;
            else if (val == Block.End)
                return EndNodeBrush;
            else if (val == Block.Visited)
                return VisitedNodeBrush;
            else if (val == Block.Special)
                return SpecialNodeBrush;
            else
                return HallBrush;

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
