namespace CloneControllerAccount.EventBusConfig
{
    public class AccountEvent 
    {
        public string UserName { get; set; } = string.Empty; 
        public DateTime EventTime { get; set; }
    }
}
