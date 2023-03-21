using UnityEngine;
using HandleTimes = System.Collections.Generic.List<int>;
using Handler = System.Action<dynamic>;
using System.Collections.Generic;
using System.Linq;

public class EventListeners<C> where C: Component
{
    Dictionary<string, List<(int, Handler)>> handlerMap;
    List<IListen<C>> listeners;
    C component;

    public EventListeners(C component)
    {
        this.component = component;
        listeners = Finder.FindInterfaces<IListen<C>>();
        handlerMap = new Dictionary<string, List<(int, Handler)>>();
    }

    public void RegisterEvent(IEvent<C> @event)
    {
        foreach (var listener in listeners)
        {
            var (handleTimes, handler) = listener.HandleEvent(component, @event);
            if (handler == null) continue;
            var newHandlers = handleTimes.Select((handleTime) => (handleTime, handler));
            if (handlerMap.TryGetValue(@event.GetName(), out var handlers))
            {
                handlers.AddRange(newHandlers);
            }
            else
            {
                handlerMap[@event.GetName()] = new List<(int, Handler)>(newHandlers);
            }
        }

        // Sort by handle time
        if(handlerMap.TryGetValue(@event.GetName(), out var unsortedHandlers))
            handlerMap[@event.GetName()] = unsortedHandlers.OrderBy(handler => handler.Item1).ToList();
    }

    public void RaiseEvent(IEvent<C> @event)
    {
        if (handlerMap.TryGetValue(@event.GetName(), out var handlers))
        {
            foreach (var (_, handler) in handlers)
                handler?.Invoke(@event.GetPayload());
        }
    }
}

public interface IListen<C>
    where C : Component
{
    (HandleTimes, Handler) HandleEvent(C component, IEvent<C> @event);
}

public interface IEvent<P> {
    dynamic GetPayload();
    string GetName();
}
