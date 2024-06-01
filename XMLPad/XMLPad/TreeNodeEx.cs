﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XMLPad
{
    public static class TreeNodeEx
    {
        public static IEnumerable<TreeNode> AddRange(this TreeNode collection, IEnumerable<TreeNode> nodes)
        {
            var items = nodes.ToArray();
            collection.Nodes.AddRange(items);
            return new[] { collection };
        }
    }
}
