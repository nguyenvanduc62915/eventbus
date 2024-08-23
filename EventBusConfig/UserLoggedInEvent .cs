namespace CloneControllerAccount.EventBusConfig
{
    public class UserLoggedInEvent : AccountEvent
    {
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = "User Logged In Successfully";
    }
}
