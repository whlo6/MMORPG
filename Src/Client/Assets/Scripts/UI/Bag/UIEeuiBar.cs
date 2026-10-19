using Assets.Scripts.Managers;
using Assets.Scripts.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEeuiBar : MonoBehaviour
{
    public Transform[] equipFields;
    public GameObject equipItem;
    public Slider hp;
    public Slider mp;
    public Text THp;
    public Text TMp;
    public Text TMaxHp;
    public Text TMaxMp;
    public Text TAtk;
    public Text TSta;
    public Text TWit;

    GameObject[] equipObj = new GameObject[4];

    public void Init()
    {
        //更新已经装备的物品
        for (int i = 0; i < 4; i++)
        {
            int n = EquipmentManager.Instance.equipId[i];
            if (n == -1) continue;
            if (equipObj[i] != null)
                Destroy(equipObj[i]);
            equipObj[i] = Instantiate(equipItem, equipFields[i]);
            equipObj[i].transform.localPosition = Vector3.zero;
            equipObj[i].GetComponent<UIEquipItem>().Init(n);
        }
        //获取并显示基础数值
        this.hp.maxValue = PlayerMessagManager.Instance.maxHp;
        this.hp.value = PlayerMessagManager.Instance.hp;
        this.mp.maxValue = PlayerMessagManager.Instance.maxMp;
        this.mp.value = PlayerMessagManager.Instance.mp;
        this.THp.text = this.hp.value.ToString();
        this.TMp.text = this.mp.value.ToString();
        this.TMaxHp.text = "最大生命：\t" + PlayerMessagManager.Instance.maxHp.ToString();
        this.TMaxMp.text = "最大法力：\t" + PlayerMessagManager.Instance.maxMp.ToString();
        this.TAtk.text = "力量：\t" + PlayerMessagManager.Instance.atk.ToString();
        this.TSta.text = "防御：\t" + PlayerMessagManager.Instance.sta.ToString();
        this.TWit.text = "智力：\t" + PlayerMessagManager.Instance.wit.ToString();
    }

    public void OnClickEquip()
    {
        if (BagManager.Instance.checkObj == null) return;
        Equipment equipDefin = EquipmentManager.Instance.Equipments[BagManager.Instance.checkObj.listId];
        int number = equipDefin.number;

        //判断装备职业
        int n = number / 1000 % 40;
        if(n!=(int)PlayerMessagManager.Instance.characterClass)
        {
            MessageBox.Show("职业不符，你无法使用这个装备");
            return;
        }
        //判断装备部位
        number = number % (number / 10 * 10);
        EquipmentManager.Instance.equipId[number - 1] = equipDefin.id;
        PlayerMessagManager.Instance.Init();
        this.Init();
    }
}
