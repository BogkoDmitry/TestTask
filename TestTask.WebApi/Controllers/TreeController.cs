using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TestTask.WebApi.Entities;
using TestTask.WebApi.Infrastructure;
using TestTask.WebApi.ResponseModels;

namespace TestTask.WebApi.Controllers
{
    [Tags(Tags.Tree)]
    [ApiController]
    public class TreeController : ControllerBase
    {
        private readonly TestTaskDbContext _context;

        public TreeController(TestTaskDbContext context)
        {
            _context = context;
        }

        /// <remarks>
        /// Returns your entire tree. If your tree doesn't exist it will be created automatically.
        /// </remarks>
        [HttpPost("api.user.tree.get")]
        public async Task<TreeNodeModel> Get([Required] string treeName)
        {
            var node = await _context.Nodes.FirstOrDefaultAsync(x => x.TreeName == treeName);

            if (node is null)
            {
                var newNode = new TreeNode() { Name = treeName, TreeName = treeName, ParentId = null };
                await _context.Nodes.AddAsync(newNode);
                await _context.SaveChangesAsync();

                return ToTreeNodeModel(newNode);
            }
            else
            {
                var result = _context.Nodes.FromSqlRaw(@$"
                        WITH RECURSIVE cte AS (
                            SELECT ""Id"", ""ParentId"", ""Name"", ""TreeName""
                            FROM ""Nodes"" WHERE ""ParentId"" IS NULL AND ""TreeName"" = '{treeName}'
                        UNION
                            SELECT n.""Id"", n.""ParentId"", n.""Name"", n.""TreeName""
                            FROM ""Nodes"" n
                            JOIN cte ON n.""ParentId"" = cte.""Id""
                        )
                        SELECT * FROM cte
                    ").ToList();

                return ToTreeNodeModel(result);
            }
        }

        private TreeNodeModel ToTreeNodeModel(TreeNode treeNode)
        {
            return new TreeNodeModel() { Id = treeNode.Id, Name = treeNode.Name, Children = new List<TreeNodeModel>() };
        }

        private TreeNodeModel ToTreeNodeModel(List<TreeNode> treeNodes)
        {
            var lookup = treeNodes.ToLookup(item => treeNodes.FirstOrDefault(parent => parent.Id == item.ParentId), child => child);

            var root = lookup.First().First();
            
            var result = new TreeNodeModel() { Id = root.Id, Name = root.Name, Children = new List<TreeNodeModel>()};
            result.Children = GetChildren(lookup, lookup.FirstOrDefault(x => x.Key == root).Key);

            return result;
        }

        private List<TreeNodeModel> GetChildren(ILookup<TreeNode, TreeNode> lookup, TreeNode key)
        {
            var result = new List<TreeNodeModel>();
            foreach (var data in lookup[key])
            {
                var child = new TreeNodeModel() { Id = data.Id, Name = data.Name, Children = new List<TreeNodeModel>() };
                child.Children.AddRange(GetChildren(lookup, data));

                result.Add(child);
            }

            return result;
        }
    }
}
