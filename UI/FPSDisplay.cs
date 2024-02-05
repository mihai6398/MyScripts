using UnityEngine;


public class FPSDisplay : MonoBehaviour
{
    public int FPSLemit = 60;
    public bool finalBuildld;
    
    private float _deltaTime;
    private void Awake()
    {
        Application.targetFrameRate = FPSLemit;
    }

    private void  Start()
    {
        QualitySettings.vSyncCount = 0; 
        Application.targetFrameRate = FPSLemit; 
    }
    private void Update()
    {
        if (finalBuildld) return;
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        if(finalBuildld) return;
        int width = Screen.width, height = Screen.height;

        GUIStyle style = new GUIStyle();

        // ReSharper disable once PossibleLossOfFraction
        Rect rect = new Rect(0, 0, width, height * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = height * 2 / 100;
        style.normal.textColor = new Color(1.0f, 0.0f, 0.5f, 1.0f);
        float msec = _deltaTime * 1000.0f;
        float fps = 1.0f / _deltaTime;
        string text = $"{msec:0.0} ms ({fps:0.} fps)";
        GUI.Label(rect, text, style);
    }
}