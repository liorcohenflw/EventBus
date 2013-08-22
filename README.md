EventBus
========

Event Bus inspired by google guava project. On .NET platform.
This project is a porting of the Guava EventBus to .NET platform.
EventBus is a robust and optimized publish/subscribe component.It 
is simplifies the communication between components in the application.
Moreover it communicates between components in a loosely coupled manner 
, where each component never needs to know or hold explicitly each other.
It is very convenient tool for programmers , due to the fact that it is easy 
to integrate the publishers and subscribers , no interfaces needed.

Event Bus Api
=============
* Register a subscriber to events . To integrate it there simple steps to do , moreover on it is described at User Guide paragraph.
void Register(Object subscriber)
* Unregister a subscriber from this point and on  the subscriber will not get a message when event associated with subscriber is dispatched.
void UnRegister(Object subscriber)
* Post an event associated with subscribers. The creation of communication from event to subscribers is described at User Guide paragraph.
void Post(Object event)

User Guide
=========
So we have three important Entities in this story : subscriber , publisher and event.The subscriber that wants to register to some kind of events or single event , publisher who desire to publish events at the correct time and transfer some data that relates to the subscriber (data that will probably will change the state of the subscriber or not), and for last is the event some type of object that determines the type of the event that happens.
So who can be the publisher subscriber , it can be any components that has reference to event bus and event can be any type of variable.
So without further explanations lets give some example : 
First lets define an instance of EventBus some where in the app.
<code>
IEventBus eventBus = new EventBus();
</code>
The subscriber : 
----------------------
<pre>
<code>
public class StockRecorder
{
      [Subscribe] 
       public void OnStockChanged(StockChangeEvent event)
       {
                   UpdateStockValue(event.GetValue)
       }
}
</code>
</pre>
 The event:
---------------
<pre>
<code>
public class StockChangeEvent
{
      private Decimal valueStock;
      public StockChangeEvent(Decimal value)
      {
               valueStock = value;
      }

      public Decimal GetValue { get { return  stockValue;} private set; 
}
</code>
</pre>
The registration:
----------------------
Some where in the app registration happens.
<pre>
<code>
StockRecoder recoderSubscriber = new StockRecoder();
eventBus.Register(recordSubscriber);
</code>
</pre>
Much later the publisher doing : 
---------------------------------------
<pre>
<code>
StockService service = GetStockService("MicroSoft");
Decimal valueStock = service.GetValue();
StockChangeEvent event = new StockChangeEvent(valueStock);
eventBus.Post(event);
</code>
</pre>
This is a simple scenario where there is a stock that changes over time , the actual value is received from a service called StockService.The event is a change in the value of stock . The subscriber is some object that needs the MicroSoft's change of stock .
So every method marked with Subscribe attribute in the subscriber is actually 
registered at the event bus , the event is determined by the type of the event.
A publisher can be any component that knows StockChangeEvent class.
So when the post is called with new StockChangeEvent instance, every object that made registration earlier and has method that gets StockChangeEvent instance with a method marked with Subscribe attribute is getting now the new instance of   StockChangeEvent .It is important that the component who does the registration will hold the instance of the subscriber , in order to do Unregister later.
