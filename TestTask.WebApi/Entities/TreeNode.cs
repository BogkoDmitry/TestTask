namespace TestTask.WebApi.Entities
{
    public class TreeNode
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string? TreeName { get; set; }
    }
}
