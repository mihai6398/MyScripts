using UnityEngine;
using nn.hid;
using UnityEngine.UI;
using System.Collections;

public class HidNintendoInput : MonoBehaviour
{
    private NpadId[] npadIds = { NpadId.Handheld, NpadId.No1 };
    private NpadState npadState = new NpadState();
    private ControllerSupportArg controllerSupportArg = new ControllerSupportArg();
    private nn.Result result = new nn.Result();
    private long[] preButtons;

    private GameObject _CanvasWarning;
    private bool _isActivatePP = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void init()
    {
        Debug.LogError("init -> HidNintendoInput");
        GameObject main = new GameObject("HidNintendoInput");
        main.AddComponent<HidNintendoInput>();
        GameObject.DontDestroyOnLoad(main);
    }

    void Start()
    {
        Npad.Initialize();
        Npad.SetSupportedIdType(npadIds);
        NpadJoy.SetHoldType(NpadJoyHoldType.Horizontal);
        Npad.SetSupportedStyleSet(NpadStyle.FullKey | NpadStyle.Handheld | NpadStyle.JoyDual);
        preButtons = new long[npadIds.Length];

    }

    void Update()
    {
        NpadButton onButtons = 0;

        for (int i = 0; i < npadIds.Length; i++)
        {
            NpadId npadId = npadIds[i];
            NpadStyle npadStyle = Npad.GetStyleSet(npadId);
            if (npadStyle == NpadStyle.None) { continue; }

            Npad.GetState(ref npadState, npadId, npadStyle);

            onButtons |= ((NpadButton)preButtons[i] ^ npadState.buttons) & npadState.buttons;
            preButtons[i] = (long)npadState.buttons;

            if (((npadState.attributes & NpadAttribute.IsLeftConnected) != 0) != ((npadState.attributes & NpadAttribute.IsRightConnected) != 0))
            {
                if (!_isActivatePP)
                    StartCoroutine(ShowPupupGame());

                //if (onButtons != 0)
                //{
                //    ShowControllerSupport();
                //}
            }
        }
    }

    void ShowControllerSupport()
    {
        controllerSupportArg.SetDefault();
        controllerSupportArg.playerCountMax = (byte)(npadIds.Length - 1);
        controllerSupportArg.enableSingleMode = true;

        Debug.Log(controllerSupportArg);
        result = ControllerSupport.Show(controllerSupportArg);
        if (!result.IsSuccess()) { Debug.Log(result); }
        _isActivatePP = false;
        Destroy(_CanvasWarning);
    }

    IEnumerator ShowPupupGame()
    {
        _isActivatePP = true;
        _CanvasWarning = new GameObject("Canvas");
        Canvas c = _CanvasWarning.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        _CanvasWarning.AddComponent<CanvasScaler>();
        _CanvasWarning.AddComponent<GraphicRaycaster>();
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Text i = panel.AddComponent<Text>();
        i.gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        i.gameObject.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
        i.text = "The Joy-Con grip accessory is recommended when playing.";
        i.alignment = TextAnchor.MiddleCenter;
        i.font = Font.CreateDynamicFontFromOSFont("Arial", 20);
        i.fontSize = 20;
        panel.transform.SetParent(_CanvasWarning.transform, false);

        yield return new WaitForSecondsRealtime(3f);

        ShowControllerSupport();
    }
}