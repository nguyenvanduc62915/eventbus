namespace CloneControllerAccount.EventBusConfig
{
    public class UserRegisteredEvent : AccountEvent
    {
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = "User Registered Successfully";
    }
}
