using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.Managers;

public class UIEquipItem : MonoBehaviour, IPointerClickHandler
{
    public Image Icon;

    public void Init(int id)
    {
        this.Icon.sprite = EquipmentManager.Instance.Icon[id];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
