using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XMLPad
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private IEnumerable<TreeNode> GetNodes(TreeNode node, XElement element)
        {
            return element.HasElements ?
                node.AddRange(from item in element.Elements()
                              let tree = new TreeNode(item.ToString().Truncate(20, true)) { Tag = item }
                              from newNode in GetNodes(tree, item)
                              select newNode)
                              :
                new[] { node };
        }


        XDocument doc;
        public IEnumerable<string> GetXPathValues(XNode node, string xpath)
        {
            foreach (XObject xObject in (IEnumerable)node.XPathEvaluate(xpath))
            {
                if (xObject is XElement)
                    yield return ((XElement)xObject).Value;
                else if (xObject is XAttribute)
                    yield return ((XAttribute)xObject).Value;
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            var d = AutoDialog.DialogHelpers.StartDialog();
            d.AddStringField("query", "Query");
            d.ShowDialog();

            var x = d.GetStringField("query");
            //var nodes = doc.XPathSelectElements (x);
            /*foreach (var item in GetXPathValues(doc.Root, x))
            {

            }*/
            var tt = doc.XPathEvaluate(x);
            StringBuilder sb = new StringBuilder();
            /*foreach ( var node in nodes )
            {
                sb.Append(node.Value);
            }*/

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var el = e.Node.Tag as XElement;
            richTextBox1.Text = el.Value;
            listView1.Items.Clear();
            foreach (var item in el.Attributes())
            {
                listView1.Items.Add(new ListViewItem(new string[] { item.Name.ToString(), item.Value }) { });
            }
        }

        private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            doc = XDocument.Load(ofd.FileName);
            UpdateTree();
        }

        void UpdateTree()
        {
            var root = doc.Root;
            var x = GetNodes(new TreeNode(root.ToString().Truncate(20, true)) { Tag = root }, root).ToArray();

            treeView1.Nodes.AddRange(x);
        }

        private void aggregateValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var el = treeView1.SelectedNode.Tag as XElement;
            StringBuilder sb = new StringBuilder();
            char separator = ' ';
            foreach (var item in el.Elements())
            {
                sb.Append($"{item.Value}{separator}");
            }
            Clipboard.SetText(sb.ToString());
            MessageBox.Show("saved to clipboard");
        }
    }
}
