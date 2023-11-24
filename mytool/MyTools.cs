using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyToolsUnity
{
    public class MyTools : MonoBehaviour
    {

        // Constructor care primește un MonoBehaviour pentru a rula corutine


        // Funcție pentru un timer global
        public void GlobalTimer(float time, System.Action onTimerComplete)
        {
            StartCoroutine(StartGlobalTimer(time, onTimerComplete));
        }

        // Corutină pentru timer global
        private IEnumerator StartGlobalTimer(float time, System.Action onTimerComplete)
        {
            // Așteaptă timpul specificat
            yield return new WaitForSeconds(time);

            // Adaugă codul pe care dorești să îl rulezi când timer-ul este complet
            // Exemplu: Debug.Log("Timer complet!");
              
            // Apelăm acțiunea (callback) pentru a notifica că timer-ul s-a încheiat
            onTimerComplete?.Invoke();
        }
        
        // Funcție pentru amestecarea aleatorie a unui array
        public void Shuffle<T>(T[] array)
        {
            System.Random rng = new System.Random();

            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (array[k], array[n]) = (array[n], array[k]);
            }
        }

        // Funcție pentru filtrarea unui array în funcție de un predicat
        public T[] Filter<T>(T[] array, Predicate<T> predicate)
        {
            List<T> result = new List<T>();
            foreach (T item in array)
            {
                if (predicate(item))
                {
                    result.Add(item);
                }
            }
            return result.ToArray();
        }
        // Funcție pentru inversarea unui șir de caractere
        public string ReverseString(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        
        // Funcție pentru calculatorul de șanse
        public bool ShotCalculator(int successPercentage, int failurePercentage)
        {
            Dictionary<string, string> result = CalculatePercentages(successPercentage, failurePercentage);

            if (result.ContainsKey("Error"))
            {
                Debug.LogError(result["Error"] + "***");
                return false;
            }
            else
            {
                float randomValue = UnityEngine.Random.Range(0f, 100f);
                float successPercentageValue = float.Parse(result["Success Percentage"].Trim('%'));
                bool isSuccess = randomValue < successPercentageValue;
                return isSuccess;
            }
        }

        // Funcție pentru calcularea procentajelor
        private Dictionary<string, string> CalculatePercentages(int successCount, int failureCount)
        {
            float total = successCount + failureCount;
            Dictionary<string, string> percentages = new Dictionary<string, string>();

            if (total == 0)
            {
                percentages.Add("Error", "No data to calculate percentages.");
            }
            else
            {
                float successPercentage = (successCount / total) * 100;
                float failurePercentage = (failureCount / total) * 100;

                percentages.Add("Success Percentage", successPercentage + "%");
                percentages.Add("Failure Percentage", failurePercentage + "%");
            }

            return percentages;
        }
        
        public static bool InvokeIfNotNull<T>(T obj)
        {
            return obj != null;
        }

        public int GenerateRandomNumber(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
        

    }
}
