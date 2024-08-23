namespace CloneControllerAccount.EventBusConfig
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Func<object, Task>>> _subscribers = new();

        public void Subscribe<T>(Func<T, Task> handler) where T : AccountEvent
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)] = new List<Func<object, Task>>();
            }
            _subscribers[typeof(T)].Add(async e => await handler((T)e));
        }

        public async Task Publish<T>(T eventMessage) where T : AccountEvent
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                foreach (var handler in _subscribers[typeof(T)])
                {
                    try
                    {
                        await handler(eventMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error handling event: {ex.Message}");
                    }
                }
            }
        }
    }
}
