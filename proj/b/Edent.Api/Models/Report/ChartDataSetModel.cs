using System;
using System.Collections.Generic;

namespace Edent.Api.Models.Report
{
    public class ChartDataSetModel<T>
    {
        public ChartDataSetModel()
        {
            Data = new List<T>();
        }
        public string Label { get; set; }
        public ICollection<T> Data { get; set; }
    }
}

