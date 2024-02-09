namespace Mazzatech.Domain.EntitiesModels
{
    public class DbErrorLoggerEntityModel
    {
        public int Id { get; set; }
        public string DocumentKey { get; set; }
        public string DocumentContent { get; set; }
        public string Queue { get; set; }    
        public string Exchange {  get; set; }
        public string RoutingKey { get; set; }
        public string ErrorMessage { get; set; }
        public string EventDateTime {  get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
}
