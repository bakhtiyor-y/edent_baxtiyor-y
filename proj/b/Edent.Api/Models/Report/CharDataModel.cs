using System;
using System.Collections.Generic;

namespace Edent.Api.Models.Report
{
    public class CharDataModel<T>
    {
        public CharDataModel()
        {
            Labels = new List<string>();
            Datasets = new List<ChartDataSetModel<T>>();
        }
        public ICollection<string> Labels { get; set; }

        public ICollection<ChartDataSetModel<T>> Datasets { get; set; }
    }
}

