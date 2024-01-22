using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Tricks Data", fileName = "TricksData")]
public class TricksData : ScriptableObject
{
    [SerializeField] private TrickData[] _tricks;

    public Sprite GetSprite(int index) => _tricks[index].Icon;
    public string GetName(int index) => _tricks[index].Name;
    public int GetExperience(int index) => _tricks[index].Experience;

    [Serializable]
    private class TrickData
    {
        public string Name;
        public Sprite Icon;
        [Range(0, 500)] public int Experience = 60;
    }
}