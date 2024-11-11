using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conveyor
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public HtmlAgilityPack.HtmlDocument Doc { get; set; }

        // Временные метки
        public DateTime CreationTime { get; set; }
        public DateTime QueueStage1Time { get; set; }
        public DateTime StartStage1Time { get; set; }
        public DateTime QueueStage2Time { get; set; }
        public DateTime StartStage2Time { get; set; }
        public DateTime QueueStage3Time { get; set; }
        public DateTime StartStage3Time { get; set; }
        public DateTime DestructionTime { get; set; }

        // Данные рецепта
        public int IssueId = 9162;
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Steps { get; set; }
        public string ImageUrl { get; set; }
    }   
}
