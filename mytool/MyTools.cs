using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyToolsUnity
{
    public class MyTools : MonoBehaviour
    {
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
        


        public int GenerateRandomNumber(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
        //Global Debager
        //MyTools myTools = GetComponent<MyTools>(); // Asigurați-vă că există o instanță MyTools pe un GameObject
        //myTools.MonitorVariable(() => PlayerData.MissionComplite, (newValue) =>
        //{
        //    Debug.Log($"MissionComplite changed to {newValue}");
        //});

        public void MonitorCurrentSelectedUI(Action<GameObject> onSelectionChange)
        {
            StartCoroutine(MonitorCurrentSelectedUICoroutine(onSelectionChange));
        }
        
        private IEnumerator MonitorCurrentSelectedUICoroutine(Action<GameObject> onSelectionChange)
        {
            GameObject lastSelectedObject = null;
            while (true)
            {
                yield return new WaitForSeconds(0.1f); // Verifică schimbarea la fiecare 0.1 secunde

                GameObject currentSelectedObject = EventSystem.current.currentSelectedGameObject;
                if (currentSelectedObject != lastSelectedObject)
                {
                    onSelectionChange?.Invoke(currentSelectedObject);
                    lastSelectedObject = currentSelectedObject;
                }
            }
        }
        
        
        public void MonitorVariable<T>(Func<T> valueProvider, Action<T> onChange)
        {
            StartCoroutine(MonitorVariableCoroutine(valueProvider, onChange));
        }

        private IEnumerator MonitorVariableCoroutine<T>(Func<T> valueProvider, Action<T> onChange)
        {
            if (valueProvider == null) yield break;

            T lastValue = valueProvider();
            while (true)
            {
                yield return new WaitForSeconds(0.5f); // Verifică schimbarea la fiecare 0.5 secunde
                T currentValue = valueProvider();
                if (!EqualityComparer<T>.Default.Equals(lastValue, currentValue))
                {
                    onChange?.Invoke(currentValue);
                    lastValue = currentValue;
                }
            }
        }
        
        
    }
}
