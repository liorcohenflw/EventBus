EventBus
========

Event Bus inspired by google guava project, on .NET platform.
This project is a porting of the Guava EventBus to .NET platform.
EventBus is a robust and optimized publish/subscribe component. It 
is simplifies the communication between components in the application.
Moreover it communicates between components in a loosely coupled manner, where each component never needs to know or hold explicitly each other.
It is very convenient tool for programmers, due to the fact that it is easy 
to integrate the publishers and subscribers, no interfaces needed.

Event Bus API
=============
* Register a subscriber to events. To integrate it there simple steps to do, moreover on it is described at User Guide paragraph.
void Register(Object subscriber)
* Unregister a subscriber from this point and on  the subscriber will not get a message when event associated with subscriber is dispatched.
void UnRegister(Object subscriber)
* Post an event associated with subscribers. The creation of communication from event to subscribers is described at User Guide paragraph.
void Post(Object event)

User Guide
=========
So we have three important Entities in this story : subscriber , publisher and event.The subscriber that wants to register to some kind of events or single event, publisher who desire to publish events at the correct time and transfer some data that relates to the subscriber (data that will probably will change the state of the subscriber), and for last is the event some type of object that determines the type of the event that happens.
So who can be the publisher subscriber, it can be any components that has reference to event bus and event can be any type of variable.
So without further explanations lets give some example : 
First lets define an instance of EventBus some where in the app.
<pre>
<code>

IEventBus eventBus = new EventBus();
</code>
</pre>
The subscriber : 
----------------------
Subscribers that would like to declare a method as handler for a specific message, should add 
this attribute to the method decleration.
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
<dl>
<dt> 
Please notice the following:
</dt>
<dd>
a.The return type of the subscribed method is irrelevant and generally should be void.
</dd>
<dd>
b.The method must have one and only one parameter, and its type    determines the event type.
</dd>
</dl>
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
StockService service = GetStockService("Microsoft");
Decimal valueStock = service.GetValue();
StockChangeEvent event = new StockChangeEvent(valueStock);
eventBus.Post(event);
</code>
</pre>
This is a simple scenario where there is a stock that changes over time, the actual value is received from a service called StockService. The event is a change in the value of stock. The subscriber is some object that needs the Microsoft's change of stock.
So every method marked with Subscribe attribute in the subscriber is actually 
registered at the event bus, the event is determined by the type of the event object.
A publisher can be any component that knows StockChangeEvent class.
So when the post is called with new StockChangeEvent instance, every object that made registration earlier and has method that gets StockChangeEvent instance with a method marked with Subscribe attribute is getting now the new instance of   StockChangeEvent. It is important that the component who does the registration will hold the instance of the subscriber, in order to do Unregister later.
Prefere Event Bus Over Events and Delegates in .NET
==========================================

Before I'll give an example that shows how event bus is preferred over events and event handlers (delegates) in C# there is a [link](http://lampwww.epfl.ch/~imaier/pub/DeprecatingObserversTR2010.pdf ) with article that explains deprecation of observer pattern which similar in some points to events and delegates in Csharp.
This article is very abstract so I'll give some simple example:
Consider we have a winform application that contains text box, label and button, requirements are when the text box is empty the label should be without text and the button disabled.
 When the text box is filled with text the button should be enabled and some word in the text should  be recognized and displayed at the label.
First lets associate event to its handler:
-------------------------------------------------------
<pre>
<code>
this.textBox1.TextChanged += new System.EventHandler(this.LabelHandler);
 this.textBox1.TextChanged += new System.EventHandler(this.ButtonHandler);
</code>
</pre>
Second is the code of handlers:
---------------------------------------------
<pre>
<code>
private void ButtonHandler(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty)
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
        }

        private void LabelHandler(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty)
            {
                label1.Text = "";
            }
            else if(regExp.IsMatch(textBox1.Text))
            {
                label1.Text = stringMatch();
           }
        }
</code>
</pre>
Next is Example of Event Bus:
-------------------------------------------
First is the subscribers:
<code>
<pre>
public class LabelSubscriber
    {
        private readonly Label label;
        public LabelSubscriber(Label label) { this.label = label; }
        [Subscribe]
        public void OnTextChanged( TextChanged textChanged)
        {
            label.Text = textChanged.Text;
        }
    }

    public class ButtonSubscriber
    {
        private readonly Button button;
        public ButtonSubscriber(Button button) { this.button = button; }
        [Subscribe]
        public void OnTextChanged( TextChanged textChanged)
        {
            if (textChanged.Text.Equals(""))
            {
                button.Enabled = false;
            }
            else
            {
                button.Enabled = true;
            }
        }
    }

This some initialization method:
   private void InitializeUserComponenets()
        {
            bus = new EventBus();
            bus.Register(new LabelSubscriber(label1));
            bus.Register(new ButtonSubscriber(button1));
        }
</code>
</pre>
The event holding data of text box:
-------------------------------------------------
<pre>
<code>
    public class TextChanged
    {
        private readonly String text;
        public TextChanged(String text) { this.text = text; }
        public String Text { get { return text; } }
    }

For last is the posting of text changed event:
  private void TextChangedHandler(object sender, EventArgs e)
        {
            bus.Post(new TextChanged(textBox1.Text));
        }
</code>
</pre>
Compare those examples:
1.In the first example the button handler has access to the txtbox1 and the button1, the label handler has access to the txtbox1 and label1,this is a bad design because the handlers which are the observers has access to the subject which is the text box, if I want to change the subject from text box to combo box 
I will need to change the entire code which breaks the [open/closed principle](http://en.wikipedia.org/wiki/Open/closed_principle).In the second example the code of that will be change is only of the post, which is the extraction of data (text) and the post action.Another important thing that in the first example there is'nt a seperation of concerns, the handler of button trace the changes of txtbox1 and also changes the state of button [mvc pattern solves it](http://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93controller),
but also the event bus solves it (only from the reason that mvc can implemented by using event bus).Another issue is scalability, the graph that created from the first example looks like this:
![alt text](https://github.com/liorcohenflw/AccesoriesFiles/blob/master/dependencyGraph.png?raw=true "dependencies graph")
It is clearly there isn't scalability, this is very simple example in more complex example it will be very even hard to read this graph and to know who associated with who.In contrast the graph that created by second example looks like this:
![alt text]
(https://github.com/liorcohenflw/AccesoriesFiles/blob/master/EventBus.png?raw=true)
This graph is much simpler than the first, it  is easy to read: button and label that listens to text change event, when text changed they gets the data through the event bus, the only interest for them is the data of text box.When the example will be more complex it will be easy to read the graph, each group of components will have the same interest (event).Later there is a complex example.
More practical issues of events and delegates:
1.Resource management - if the lifecycle of some functionality is over we may need to remove some delegates from some events, so we need to remember at which points (events) we installed (using += operator) the delegates.
2.Uniformity - In .NET it is impossible to install different kind of methods (delegates with their signatures) at different events.So it is decreasing uniformity.The event bus has full uniformity the post register and unregister gets object. 
3.Any code that uses the sender Object to extract data, endanger with incorrect casting, which decrease type safety, although the event bus gets object, it is type safe.
More Complex Example:
----------------------------------
For last there is a scenario which can make the first example  complicated structure and very hard to maintain, the scenario is when we have actions need to take place before other actions in the flow. For instance we have event1 event2 and action1, action2, and action3.
If action2 and action3 can't happen before action1 took place , we need a structure where event1 holds action1 and event2 holds action2 and action3, when event1 is invoked action1 is taking place, in the end of action1 the event2 invoked than action2 and action3 taking place. So we've gut a graph like this:
![alt text](https://github.com/liorcohenflw/AccesoriesFiles/blob/master/actionEvent.png?raw=true)
This example of using events and delegates, it can gets much more complicated.
