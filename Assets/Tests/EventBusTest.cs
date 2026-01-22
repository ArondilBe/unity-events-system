using System.Collections.Generic;
using Events;
using NUnit.Framework;

public class EventBusTest
{
    private class TestEvent1 : IGameEvent
    {
        public int Value;
    }

    private class TestEvent2 : IGameEvent
    {
        public int Value;
    }

    private class TestEvent3 : IGameEvent
    {
        public int Value;
    }

    private class TestEvent4 : IGameEvent
    {
        public int Value;
    }

    [Test]
    public void Subscribe_Publish_InvokesHandler()
    {
        var called = false;
        EventBus.Subscribe<TestEvent1>(e => called = true);
        EventBus.Publish(new TestEvent1());
        Assert.IsTrue(called);
    }

    [Test]
    public void MultipleSubscribers_AreAllInvoked_InOrder()
    {
        var order = new List<int>();
        EventBus.Subscribe<TestEvent2>(e => order.Add(1));
        EventBus.Subscribe<TestEvent2>(e => order.Add(2));
        EventBus.Publish(new TestEvent2());
        Assert.AreEqual(new List<int> { 1, 2 }, order);
    }

    [Test]
    public void Unsubscribe_RemovesHandler()
    {
        var called = false;
        void Handler(TestEvent3 e) => called = true;

        EventBus.Subscribe<TestEvent3>(Handler);
        EventBus.Unsubscribe<TestEvent3>(Handler);
        EventBus.Publish(new TestEvent3());
        Assert.IsFalse(called);
    }

    [Test]
    public void DuplicateSubscription_InvokesMultipleTimes()
    {
        var count = 0;
        void Handler(TestEvent4 e) => count++;

        EventBus.Subscribe<TestEvent4>(Handler);
        EventBus.Subscribe<TestEvent4>(Handler);
        EventBus.Publish(new TestEvent4());
        Assert.AreEqual(2, count);
    }

    [Test]
    public void MutationDuringPublish_SnapshotPreventsMidPublishChanges()
    {
        var calls = new List<string>();

        void HandlerA(TestEvent1 e)
        {
            calls.Add("A");

            EventBus.Unsubscribe<TestEvent1>(HandlerB);
        }

        void HandlerB(TestEvent1 e)
        {
            calls.Add("B");
        }

        EventBus.Subscribe<TestEvent1>(HandlerA);
        EventBus.Subscribe<TestEvent1>(HandlerB);

        EventBus.Publish(new TestEvent1());
        Assert.AreEqual(new List<string> { "A", "B" }, calls);

        calls.Clear();

        EventBus.Publish(new TestEvent1());
        Assert.AreEqual(new List<string> { "A" }, calls);
    }
}
