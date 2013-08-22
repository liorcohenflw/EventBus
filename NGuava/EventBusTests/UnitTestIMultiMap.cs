using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NGuava;

namespace EventBusTests
{
    [TestClass]
    public class UnitTestIMultiMap
    {
        [TestMethod]
        public void TestClear()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            Assert.IsTrue(map.CountPairs == 0);
            map.Add(1, 2);
            map.Clear();
            Assert.IsTrue(map.CountPairs == 0);

        }

        [TestMethod]
        public void TestContainsKey()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(3, 2);
            Assert.IsTrue(map.ContainsKey(3));
            Assert.IsFalse(map.ContainsKey(45));
        }

        [TestMethod]
        public void TestContainsValue()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(4, 6);
            Assert.IsTrue(map.ContainsValue(6));
        }

        [TestMethod]
        public void TestContainsKeyValuePair()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(100, 101);
            Assert.IsTrue(map.ContainsKeyValuePair(new System.Collections.Generic.KeyValuePair<int,int>(100,101)));

        }

        [TestMethod]
        public void TestAdd()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(2, 3);
            Assert.IsTrue(map.ContainsKeyValuePair(new System.Collections.Generic.KeyValuePair<int,int>(2,3)));
        }

        [TestMethod]
        public void TestGet()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(2, 3);
            map.Add(2, 4);
            map.Add(2, 5);
            foreach (int item in map.Get(2))
            {
                Assert.IsTrue(item == 3 || item == 4 || item == 5);
            }
        }

        [TestMethod]
        public void TestCountPair()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(1, 3);
            map.Add(2,3); 
            map.Add(3,4) ;
            map.Add (4,5);
            map.Add(5, 6);
            Assert.IsTrue(map.CountPairs == 5);
            map.Clear();
            Assert.IsTrue(map.CountPairs == 0);
            
        }

        [TestMethod]
        public void TestRemove()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(2, 3);
            map.Add(2, 4);
            map.Add(2, 5);
            map.Remove(2, 5);
            Assert.IsFalse(map.ContainsKeyValuePair(new System.Collections.Generic.KeyValuePair<int, int>(2, 5)));
            map.Remove(2, 4);
            Assert.IsFalse(map.ContainsKeyValuePair(new System.Collections.Generic.KeyValuePair<int, int>(2, 4)));
            map.Remove(2, 3);
            Assert.IsFalse(map.ContainsKeyValuePair(new System.Collections.Generic.KeyValuePair<int, int>(2, 3)));
        }

        [TestMethod]
        public void TestRemoveAll()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(2, 3);
            map.Add(2, 4);
            map.Add(2, 5);
            map.RemoveAll(2);
            Assert.IsFalse(map.ContainsKey(2));
            Assert.IsFalse(map.ContainsValue(4));
            Assert.IsFalse(map.ContainsValue(5));
            Assert.IsFalse(map.ContainsValue(3));
        }

        [TestMethod]
        public void TestGetSet()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(2, 3);
            map.Add(2, 4);
            map.Add(2, 5);
            System.Collections.Generic.ISet<int> set = map.GetSet(2);
            Assert.IsTrue(set.Contains(3));
            Assert.IsTrue(set.Contains(4));
            Assert.IsTrue(set.Contains(5));
        }

        [TestMethod]
        public void TestCountPairs()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(2, 3);
            map.Add(2, 4);
            map.Add(2, 5);
            map.Add(7, 8);
            map.Add(7, 5);
            map.Add(1, 5);
            map.Add(29, 5);
            Assert.IsTrue(map.CountPairs == 7);
        }

        [TestMethod]
        public void TestAddAll()
        {
            IMultiMap<int, int> map = HashMultiMap<int, int>.Create();
            map.Add(2, 5);
            map.Add(2, 4);
            map.Add(2, 3);
            map.Add(1, 5);
            map.Add(1, 6);
            IMultiMap<int, int> map2 = HashMultiMap<int, int>.Create();
            map.Add(7, 5);
            map.Add(7, 4);
            map.Add(7, 3);
            map.Add(8, 5);
            map.Add(8, 6);
            map.AddAll(map2);
            foreach (int key in map2.Keys)
            {
                Assert.IsTrue(map.ContainsKey(key));
                foreach (int value in map2.Get(key))
                {
                    Assert.IsTrue(map.ContainsValue(value));
                }
            }
        }

       
    }
}
