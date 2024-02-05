using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryStyle : Singleton<InventoryStyle>
{
    
    [System.Serializable] // This attribute is needed for Unity to serialize the class
    public class InventoryImage
    {
        public CardUIStyle actionUIStyle;
        public Sprite icon;
        public Sprite backImage;
    }

    [SerializeField] private List<InventoryImage> actionStyle;

    public enum CardUIStyle
    {
        Rifler,
        Tank,
        Sniper,
        Spion,
    }
    
    public Sprite GetIcon(CardUIStyle actionUIStyle)
    {
        foreach (InventoryImage actionImage in actionStyle)
        {
            if (actionImage.actionUIStyle == actionUIStyle)
            {
                return actionImage.icon;
            }
        }

        return null;
    }
    public Sprite GetBackFoto(CardUIStyle actionUIStyle)
    {
        foreach (InventoryImage actionImage in actionStyle)
        {
            if (actionImage.actionUIStyle == actionUIStyle)
            {
                return actionImage.backImage;
            }
        }

        return null;
    }
}
