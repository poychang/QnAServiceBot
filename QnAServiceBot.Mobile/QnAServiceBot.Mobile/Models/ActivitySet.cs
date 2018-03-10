using System.Collections.Generic;

namespace QnAServiceBot.Mobile.Models
{
    public class ActivitySet
    {
        public List<Activity> Activities { get; set; }
        public string Watermark { get; set; }
    }
}
