using System;

namespace NGuava
{
  //thats just for test.
    public class Listener
    {   
        [Subscribe]
        public void Send(String s)
        {
            Console.WriteLine(s);
        }

        public void Send2(String s)
        {
            //doing nothing.
        }

        private void Send3(String s)
        {

        }

        public virtual void Send4(String s)
        {

        }


    }
}
