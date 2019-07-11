using ELearning.DataAccess;
using ELearning.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ELearning.ViewModels.ControlModels
{
    public static class TreeViewFactory
    {
        /// <summary>
        /// 通过数据访问服务获取树节点集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityRepository"></param>
        /// <returns></returns>
        public static List<TreeNode> GetTreeNodes<T>(IEntityRepository<T> entityRepository) where T : class, IEntity, new()
        {
            var treeViewItems = new List<TreeNode>();
            var sourceItems = SelfReferentialItemFactory<T>.GetCollection(entityRepository,false);
            treeViewItems = GetTreeNodes(sourceItems);

            return treeViewItems;
        }

        public static List<TreeNode> GetTreeNodes<T>(IEntityRepository<T> entityRepository,Expression<Func<T, bool>> predicate) where T : class, IEntity, new()
        {
            var treeViewItems = new List<TreeNode>();
            var sourceItems = SelfReferentialItemFactory<T>.GetCollection(entityRepository, predicate, false);
            treeViewItems = GetTreeNodes(sourceItems);

            return treeViewItems;
        }

        /// <summary>
        /// 根据子引用的元素集合创建树节点集合
        /// </summary>
        /// <param name="sourceItems"></param>
        /// <returns></returns>
        public static List<TreeNode> GetTreeNodes(List<SelfReferentialItem> sourceItems)
        {
            var treeViewItems = new List<TreeNode>();
            var rootItems = sourceItems.Where(x => x.ParentID == x.ID || x.ParentID == "");
            foreach (var item in rootItems)
            {
                var treeNode = _GetTreeNode(item);
                treeViewItems.Add(treeNode);
                _GetSubNodes(treeNode, item, sourceItems);
            }
            return treeViewItems;
        }

        /// <summary>
        /// 递归处理
        /// </summary>
        /// <param name="rootTreeNode"></param>
        /// <param name="rootSourceNode"></param>
        /// <param name="sourceItems"></param>
        private static void _GetSubNodes(TreeNode rootTreeNode, SelfReferentialItem rootSourceNode, List<SelfReferentialItem> sourceItems)
        {
            var subItems = sourceItems.Where(sn => sn.ParentID == rootSourceNode.ID && sn.ID != rootSourceNode.ParentID).OrderBy(o => o.SortCode);
            foreach (var item in subItems)
            {
                var treeNode = _GetTreeNode(item);
                rootTreeNode.Nodes.Add(treeNode);
                _GetSubNodes(treeNode, item, sourceItems);
            }

        }

        private static TreeNode _GetTreeNode(SelfReferentialItem selfReferentialItem)
        {
            var node = new TreeNode();
            node.Id = selfReferentialItem.ID;
            node.Text = selfReferentialItem.DisplayName;
            node.Icon = "glyphicons glyphicons-folder-minus";
            node.SelectedIcon = "glyphicons glyphicons-folder-plus";
            node.Href = "javascript:gotoTypePage('" + selfReferentialItem.ID + "')";
            return node;
        }

    }
}
