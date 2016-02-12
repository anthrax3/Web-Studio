using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prism.Events;

namespace Web_Studio.Events
{
    /// <summary>
    /// Class to mange events
    /// </summary>
    public static class EventSystem
    {
        private static IEventAggregator _current;

        /// <summary>
        /// Prism event system class
        /// </summary>
        public static IEventAggregator Current
        {
            get
            {
                return _current ?? (_current = new EventAggregator());
            }
        }

        private static PubSubEvent<TEvent> GetEvent<TEvent>()
        {
            return Current.GetEvent<PubSubEvent<TEvent>>();
        }

        /// <summary>
        /// Static method to publish an event
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        public static void Publish<TEvent>()
        {
            Publish<TEvent>(default(TEvent));
        }

        /// <summary>
        /// Static method to publish the event
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        public static void Publish<TEvent>(TEvent @event)
        {
            GetEvent<TEvent>().Publish(@event);
        }

        /// <summary>
        /// Method to subscribe for an event
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="action"></param>
        /// <param name="threadOption"></param>
        /// <param name="keepSubscriberReferenceAlive"></param>
        /// <returns></returns>
        public static SubscriptionToken Subscribe<TEvent>(Action action, ThreadOption threadOption = ThreadOption.PublisherThread, bool keepSubscriberReferenceAlive = false)
        {
            return Subscribe<TEvent>(e => action(), threadOption, keepSubscriberReferenceAlive);
        }

        /// <summary>
        /// Method to subscribe for a filter event
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="action"></param>
        /// <param name="threadOption"></param>
        /// <param name="keepSubscriberReferenceAlive"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static SubscriptionToken Subscribe<TEvent>(Action<TEvent> action, ThreadOption threadOption = ThreadOption.PublisherThread, bool keepSubscriberReferenceAlive = false, Predicate<TEvent> filter = null)
        {
            return GetEvent<TEvent>().Subscribe(action, threadOption, keepSubscriberReferenceAlive, filter);
        }

        /// <summary>
        /// Static method to unsubscribe for an event with token
        /// </summary>
        /// <param name="token"></param>
        /// <typeparam name="TEvent"></typeparam>
        public static void Unsubscribe<TEvent>(SubscriptionToken token)
        {
            GetEvent<TEvent>().Unsubscribe(token);
        }

        /// <summary>
        /// Static method to unsubscribe for an event with action
        /// </summary>
        /// <param name="subscriber"></param>
        /// <typeparam name="TEvent"></typeparam>
        public static void Unsubscribe<TEvent>(Action<TEvent> subscriber)
        {
            GetEvent<TEvent>().Unsubscribe(subscriber);
        }
    }
}