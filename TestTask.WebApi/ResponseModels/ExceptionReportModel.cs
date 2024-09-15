namespace TestTask.WebApi.ResponseModels
{
    public sealed class ExceptionReportModel
    {
        public long Id { get; set; }
        public string EventId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Text { get; set; }
    }
}
