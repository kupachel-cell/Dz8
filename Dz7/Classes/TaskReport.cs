namespace dz8lat {
    public class TaskReport
    {
        public string Text { get; set; }
        public DateTime CompletionDate { get; set; }
        public Employee Executor { get; }

        public TaskReport(string text, Employee executor)
        {
            Text = text;
            Executor = executor;
            CompletionDate = DateTime.Now;
        }
    }
}
