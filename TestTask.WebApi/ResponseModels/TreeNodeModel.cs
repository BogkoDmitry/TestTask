namespace TestTask.WebApi.ResponseModels
{
    public sealed class TreeNodeModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<TreeNodeModel> Children { get; set; }
    }
}
