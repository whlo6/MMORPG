using Assets.Scripts.Managers;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour, IPointerClickHandler
{

    public int id=-1;
    public Text TNum;
    public int num;
    public ItemType type;
    public int listId;
    public GameObject check;

    Image image;
    string Name;
    string Category;
    string Describe;

    void Start () {
        if (image == null) image = this.gameObject.GetComponent<Image>();
    }
	
	public void LoadMessag()
    {
        if(image==null) image = this.gameObject.GetComponent<Image>();
        if(type==ItemType.Equip)
        {
            this.image.sprite = EquipmentManager.Instance.Icon[id];
            this.Name = DataManager.Instance.Equipments[id].ProName;
            this.Category = DataManager.Instance.Equipments[id].Category;
            this.Describe = DataManager.Instance.Equipments[id].Description;
        }
        else
        {
            this.image.sprite = ItemManager.Instance.ItemIcon[id];
            this.Name = DataManager.Instance.Items[id].proName;
            this.Category = DataManager.Instance.Items[id].Category;
            this.Describe = DataManager.Instance.Items[id].Description;
        }
        this.TNum.text = num.ToString();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BagManager.Instance.uiBag.SetDescribe(this.Name, this.Category, this.Describe);

        //更改显示高亮
        UIBagItem obj = BagManager.Instance.checkObj;
        if (obj != null)
            obj.check.SetActive(false);
        this.check.SetActive(true);
        BagManager.Instance.checkObj = this;
    }
}
