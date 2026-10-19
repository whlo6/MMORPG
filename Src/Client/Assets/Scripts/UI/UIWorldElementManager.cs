using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPrefab;

    private Dictionary<Transform, GameObject> elements = new Dictionary<Transform, GameObject>();//所有名字框管理
	

    public void AddCharacterNameBar(Transform owner,Character character)
    {
        Debug.LogFormat("AddCharacterNameBar：ID{0}", character.Info.Id);
        GameObject obj = Instantiate(nameBarPrefab, this.transform);
        obj.name = "NameBar" + character.Info.Id;
        obj.GetComponent<UIWorldElement>().owner = owner;//名字框的位置管理
        obj.GetComponent<UINameBar>().character = character;//名字框的朝向与信息
        obj.SetActive(true);
        elements[owner] = obj;

    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if(elements.ContainsKey(owner))//判断是否存在
        {
            Destroy(elements[owner]);//从地图上删除实体
            elements.Remove(owner);//从管理中删除记录
        }
    }
}
