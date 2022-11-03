using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JDR.Utils
{
    [System.Serializable]
    public struct WeightedElement<T>
    {
        public T element;
        public float weight;
        public WeightedElement(T element, float weight)
        {
            this.element = element;
            this.weight = weight;
        }
    }

    public static class WeightedListExtensionMethods
    {
        public static T GetRandomElement<T>(this ICollection<WeightedElement<T>> list)
        {
            if (list == null)
                throw new System.ArgumentNullException("list is null");

            if (list.Count == 0)
                throw new System.ArgumentException("List is empty");

            float totalWeight = 0;
            foreach (WeightedElement<T> element in list)
            {
                totalWeight += element.weight;
            }

            float randomPoint = UnityEngine.Random.value * totalWeight;
            foreach (WeightedElement<T> element in list)
            {
                if (randomPoint < element.weight)
                    return element.element;
                randomPoint -= element.weight;
            }

            return list.Last<WeightedElement<T>>().element;
        }

        public static void Add<T>(this ICollection<WeightedElement<T>> list, T element, float weight)
        {
            list.Add(new WeightedElement<T>(element, weight));
        }
    }

    /**
        public class TestClass
        {
            public List<WeightedElement<string>> list;
            public TestClass()
            {
                list = new List<WeightedElement<string>>();
                list.Add("Test1: 1", 1f);
                list.Add("Test2: 10", 10f);
                list.Add("Test3: 10", 10f);
                list.Add("Test4: 25", 25f);
    
                string randomElement = list.GetRandomElement<string>();
                UnityEngine.Debug.Log(randomElement);
            }
        }
    */
}