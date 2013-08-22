using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;
using System.Collections.Concurrent;

namespace NGuava
{
    class Program
    {
        
        static void Main(string[] args)
        {

            ConcurrentDictionary<int, int> dic = new ConcurrentDictionary<int, int>();
            
            Console.ReadKey();
        }

        public static  IEnumerable<MethodInfo> GetMarkedMethods(Object clazz)
        {
            Type typeOfClass = clazz.GetType();
            return typeOfClass.GetMethods().Where<MethodInfo>((method) =>
            {
                Attribute attribute = method.GetCustomAttribute(typeof(Subscribe));
                return attribute == null ? false : true;
            });
        }
    }
}
