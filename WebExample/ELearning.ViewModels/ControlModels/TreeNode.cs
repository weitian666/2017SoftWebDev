using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ELearning.ViewModels.ControlModels
{
    /// <summary>
    /// 用于构建树结构列表
    /// </summary>
    public class TreeNode
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("selectedIcon")]
        public string SelectedIcon { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }

        [JsonProperty("nodes")]
        public List<TreeNode> Nodes { get; set; } = new List<TreeNode>();

        public static TreeNode GetTreeNode(string nodeText)
        {
            var node = new TreeNode();
            node.Text = nodeText;
            node.Icon = "glyphicons glyphicons-folder-minus";
            node.SelectedIcon = "glyphicons glyphicons-folder-plus";
            return node;
        }

        public static TreeNode GetTreeNode(SelfReferentialItem selfReferentialItem)
        {
            var node = new TreeNode();
            node.Text = selfReferentialItem.DisplayName;
            node.Icon = "glyphicons glyphicons-folder-minus";
            node.SelectedIcon = "glyphicons glyphicons-folder-plus";
            node.Href = "gotoTypePage(\"" + selfReferentialItem.ID + "\")";
            return node;
        }

    }
}
