using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Answer
{
    public class ChartItemModel
    {
        public int Score { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class ChartColumnsModel
    {
        public List<ChartItemModel> Math { get; set; }
        public List<ChartItemModel> Science { get; set; }
        public List<ChartItemModel> History { get; set; }
        public List<ChartItemModel> Geographic { get; set; }
        public List<ChartItemModel> Social { get; set; }
        public List<ChartItemModel> English { get; set; }
    }

    public class ChartRequestConvertModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
    }

    public class ChartRequestModel
    {
        public long BeginDate { get; set; }
        public long EndDate { get; set; }
        public int UserId { get; set; }
    }

}
