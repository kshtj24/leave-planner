using System.Collections.Generic;
using System.Drawing;

namespace LeaveTracker.Models
{
    public class CalendarItem
    {
        public Color HighlightColor { get; set; } = System.Drawing.Color.LightGreen;
        public string Date { get; set; }
        public string Day { get; set; }
        public int LeaveCount { get; set; }
        public List<LeaveDetail> Leaves { get; set; }
    }
}
