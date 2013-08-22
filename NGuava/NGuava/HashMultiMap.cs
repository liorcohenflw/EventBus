using System;
using System.Collections.Generic;
using System.Text;

namespace NGuava
{
    public class HashMultiMap<K, V> : IMultiMap<K, V>
    {
        /// <summary>
        /// This dictionary holds for each key uniqueue values with no repetitions.
        /// </summary>
        private readonly Dictionary<K, HashSet<V>> dictionary;
        //TODO : the HashSet can change to different data structure.
        /// <summary>
        /// This the number of pairs (key and value) on this HashMultiMap.
        /// </summary>
        private int amountOfPairs;
        private HashMultiMap()
        {
            dictionary = new Dictionary<K, HashSet<V>>();
        }
        /// <summary>
        /// Factory of HashMultiMap.
        /// </summary>
        /// <returns>A new HashMultiMap</returns>
        public static HashMultiMap<K, V> Create()
        {
            return new HashMultiMap<K, V>();
        }
        /// <summary>
        /// Evacuating all pairs in the HashMultiMap.
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
            CountPairs = 0;
        }
        /// <summary>
        /// Checks if the pair is in the HashMultiMap.
        /// </summary>
        /// <Preconditions>
        /// pair.Key and pair.Value are not null
        /// </Preconditions>
        /// <param name="pair">Pair to be check</param>
        /// <returns>True if the pair is in the HashMultiMap false otherwise</returns>
        public Boolean ContainsKeyValuePair(KeyValuePair<K, V> pair)
        {
            Preconditions.CheckNotNullArgument(pair.Key, "key is null");
            Boolean isContains = false;
            if (dictionary.ContainsKey(pair.Key))
            {
                HashSet<V> values = dictionary[pair.Key];
                isContains = values.Contains(pair.Value);
            }
            return isContains;
        }
        /// <summary>
        /// Checks if the key is in the HashMultiMap.
        /// </summary>
        /// <PreConditions>
        /// Key is not null.
        /// </PreConditions>
        /// <param name="key">key to be checked</param>
        /// <returns></returns>
        public Boolean ContainsKey(K key)
        {
            Preconditions.CheckNotNullArgument(key, "key is null .");
            return dictionary.ContainsKey(key);
        }
        /// <summary>
        /// Checks if the value is in the HashMultiMap.
        /// </summary>
        /// <remarks >
        /// Value can be null.
        /// </remarks>
        /// <param name="value"></param>
        /// <returns></returns>
        public Boolean ContainsValue(V value)
        {
            foreach (K key in Keys)
            {
                if (dictionary[key].Contains(value))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Adds the key and value to the HashMultiMap.
        /// </summary>
        /// <PreConditions>
        /// Key and value are not null.
        /// </PreConditions>
        /// <param name="key">key to add</param>
        /// <param name="value">value to add</param>
        public void Add(K key, V value)
        {
            Preconditions.CheckNotNullArgument(key, "key object is null .");

            if (dictionary.ContainsKey(key))
            {
                HashSet<V> values = dictionary[key];
                values.Add(value);

            }
            else
            {
                HashSet<V> values = new HashSet<V>();
                values.Add(value);
                dictionary.Add(key, values);
            }
            CountPairs = CountPairs + 1;
        }
        /// <summary>
        /// This method gives access to values associated with key.
        /// </summary>
        /// <Preconditions>
        /// Key is not null.
        /// Key is contined with in the dictionary.
        /// </Preconditions>
        /// <param name="key">key that associated with values to be traverse.</param>
        /// <returns>IEnumarble of key to be traverse</returns>
        IEnumerable<V> IMultiMap<K, V>.Get(K key)
        {
            Preconditions.CheckNotNullArgument(key, "key is null .");
            Preconditions.CheckArgument(dictionary.ContainsKey(key), "key is not found .");
            return dictionary[key];

        }
        public ISet<V> GetSet(K key)
        {
            Preconditions.CheckArgument(dictionary.ContainsKey(key), "Key is not contained.");
            return dictionary[key];
        }
        /// <summary>
        /// Removes the key value pair from the dictionary.
        /// </summary>
        /// <Preconditions>
        /// Key is not null.
        /// </Preconditions>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Remove(K key, V value)
        {
            Preconditions.CheckNotNullArgument(key, "key is null");
            if (dictionary.ContainsKey(key))
            {
                HashSet<V> values = dictionary[key];
                values.Remove(value);
                RemovePairIfValuesIsEmpty(key, values);
                CountPairs = CountPairs - 1;
            }
        }
        /// <summary>
        /// Removes all values associated with key.
        /// </summary>
        /// <Preconditions>
        /// key is not null.
        /// </Preconditions>
        /// <param name="key">key associated to removed values.</param>
        void IMultiMap<K, V>.RemoveAll(K key)
        {
            Preconditions.CheckNotNullArgument(key, "key is null .");
            if (dictionary.ContainsKey(key))
            {
                HashSet<V> values = dictionary[key];
                CountPairs = CountPairs - values.Count;
                dictionary.Remove(key);
            }

        }
        /// <summary>
        /// Property that returns IEnumarble of the keys in dictionary.
        /// </summary>
        public IEnumerable<K> Keys { get { return dictionary.Keys; } }
        /// <summary>
        /// Returns the amount of pairs in the HashMultiMap.It is actualy the amount of all values.
        /// </summary>
        public int CountPairs { get { return amountOfPairs; } private set { amountOfPairs = value; } }
        /// <summary>
        /// This method add all the the pairs of received map.
        /// </summary>
        /// <param name="map">map to be added to HashMultiMap.</param>
        public void AddAll(IMultiMap<K, V> map)
        {
            foreach (K key in map.Keys)
            {
                foreach (V value in map.Get(key))
                {
                    Add(key, value);
                }
            }

        }
    
        public override bool Equals(object obj)
        {
            Boolean answer = false;
            if (obj is HashMultiMap<K, V>)
            {
                HashMultiMap<K, V> other = (HashMultiMap<K, V>)obj;
                answer = true;
                foreach(K key in this.Keys)
                {
                  
                }


            }
            return answer;
        }
        /// <summary>
        /// Generate the hash code of this HashMultiMap.
        /// </summary>
        /// <returns>hash code depends on amount of pairs the dictionary and the set of values.</returns>
        public override int GetHashCode()
        {
            return dictionary.GetHashCode()  ^ amountOfPairs.GetHashCode();
        }

        private void RemovePairIfValuesIsEmpty(K key, HashSet<V> values)
        {
            if (values.Count == 0)
            {
                dictionary.Remove(key);
            }
        }
    }
}
