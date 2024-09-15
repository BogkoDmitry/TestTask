using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TestTask.WebApi.Entities;
using TestTask.WebApi.Exceptions;
using TestTask.WebApi.Infrastructure;

namespace TestTask.WebApi.Controllers
{
    [Tags(Tags.TreeNode)]
    [ApiController]
    public class TreeNodeController : ControllerBase
    {
        private readonly TestTaskDbContext _context;

        public TreeNodeController(TestTaskDbContext context)
        {
            _context = context;
        }

        /// <remarks>
        /// Create a new node in your tree. You must to specify a parent node ID that belongs to your tree. A new node name must be unique across all siblings.
        /// </remarks>
        [HttpPost("api.user.tree.node.create")]
        public async Task<TreeNode> Create([Required] string treeName, [Required] long parentNodeId, [Required] string nodeName)
        {
            var node = await _context.Nodes.FirstOrDefaultAsync(x => x.Id == parentNodeId && x.TreeName == treeName);

            if (node is null)
            {
                throw new SecureException($"There is no node with Id: {parentNodeId} in tree: {treeName}");
            }

            var siblingHasSameName = await _context.Nodes.CountAsync(x => x.TreeName == treeName && x.Name == nodeName) > 0;

            if (siblingHasSameName)
            {
                throw new SecureException($"There is already node with the same name: {nodeName}");
            }

            var newNode = new TreeNode() { TreeName = treeName, Name = nodeName, ParentId = parentNodeId };
            await _context.Nodes.AddAsync(newNode);
            await _context.SaveChangesAsync();

            return newNode;
        }

        /// <remarks>
        /// Rename an existing node in your tree. You must specify a node ID that belongs your tree. A new name of the node must be unique across all siblings.
        /// </remarks>
        [HttpPost("api.user.tree.node.rename")]
        public async Task<TreeNode> Rename([Required] string treeName, [Required] long nodeId, [Required] string nodeName)
        {

            var node = await _context.Nodes.FirstOrDefaultAsync(x => x.Id == nodeId && x.TreeName == treeName);

            if (node is null)
            {
                throw new SecureException($"There is no node with Id: {nodeId} in tree: {treeName}");
            }

            var siblingHasSameName = await _context.Nodes.CountAsync(x => x.TreeName == treeName && x.ParentId == node.ParentId && x.Name == nodeName) > 0;

            if (siblingHasSameName)
            {
                throw new SecureException($"There is already node with the same name: {nodeName}");
            }

            node.Name = nodeName;

            _context.Nodes.Update(node);
            await _context.SaveChangesAsync();

            return node;
        }

        /// <remarks>
        /// Delete an existing node in your tree. You must specify a node ID that belongs your tree.
        /// </remarks>
        [HttpPost("api.user.tree.node.delete")]
        public async Task Delete([Required] string treeName, [Required] long nodeId)
        {
            var node = await _context.Nodes.FirstOrDefaultAsync(x => x.TreeName == treeName && x.Id == nodeId);

            if (node is null)
            {
                throw new SecureException($"There is no node with Id: {nodeId} in tree: {treeName}");
            }

            var hasChildren = await _context.Nodes.CountAsync(x => x.TreeName == treeName && x.ParentId == nodeId) > 0;

            if (hasChildren)
            {
                throw new SecureException("You have to delete all children nodes first");
            }

            _context.Nodes.Remove(node);
            await _context.SaveChangesAsync();
        }
    }
}
