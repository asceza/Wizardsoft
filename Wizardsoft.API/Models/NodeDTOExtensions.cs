using Microsoft.AspNetCore.Razor.TagHelpers;
using Wizardsoft.DAL.Models;

namespace Wizardsoft.API.Models
{
    public static class NodeDTOExtensions
    {
        public static Node ToNode(this CreateNodeRequest node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else
            {
                Node newNode = new Node();
                newNode.Name = node.Name;
                //newNode.ParentId = node.ParentId;

                foreach(var child in node.Children)
                {
                    newNode.Children.Add(child.ToNode());

                }
                return newNode;
            }
        }


        public static NodeResponse ToNodeResponse(this Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            else
            {
                NodeResponse newNode = new NodeResponse();
                newNode.Id = node.Id;
                newNode.Name = node.Name;
                //newNode.ParentId = node.ParentId;
                newNode.Children = node.Children.ToList();
                return newNode;
            }
        }
    }
}
