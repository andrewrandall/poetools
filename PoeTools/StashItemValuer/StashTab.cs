using System;
using System.Collections.Generic;
using System.Text;

namespace StashItemValuer
{
    public class StashTab
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public List<ApiModel.Item> Items { get; set; }
    }
}
