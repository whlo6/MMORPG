using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {

	class UIElement//UI资源类型
    {
        public string Resources;
        public bool Cache;
        public GameObject Instance;
    }

    private Dictionary<Type, UIElement> UIResources = new Dictionary<Type, UIElement>();//管理所有UI资源

    public UIManager()//添加初始UI资源
    {
        Debug.Log("UIManager::Start Add UIResource");
        this.UIResources.Add(typeof(UIBag), new UIElement() { Resources = "UI/UIBag", Cache = true });
        this.UIResources.Add(typeof(UIShop), new UIElement() { Resources = "UI/UIShop", Cache = true });
        this.UIResources.Add(typeof(cnfirmBuy), new UIElement() { Resources = "UI/CnfirmBuy", Cache = false });
        this.UIResources.Add(typeof(UIQuest), new UIElement() { Resources = "UI/UIQuest", Cache = true });
        this.UIResources.Add(typeof(UIDialogBox), new UIElement() { Resources = "UI/UIDialogBox", Cache = false });
    }

    ~UIManager() { }
    
    public T Show<T>()
    {
        Type type = typeof(T);
        Debug.Log("UI_Show::" + type);

        if (this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if(info.Instance!=null)
            {
                info.Instance.SetActive(true);//设置该ui是否缓存，true：缓存，关闭窗口不删除实体
                                              //                false：不缓存，一次性窗口，关闭则直接删除实体
            }
            else
            {
                UnityEngine.Object prefab = Resources.Load(info.Resources);
                if(prefab==null)
                {
                    return default(T);
                }
                info.Instance = (GameObject)GameObject.Instantiate(prefab, UIMain.Instance.gameObject.transform);
            }
            info.Instance.SetActive(true);//显示实体ui
            return info.Instance.GetComponent<T>();
        }
        return default(T);
    }

    public void Close(Type type)
    {
        if(this.UIResources.ContainsKey(type))
        {
            UIElement info = this.UIResources[type];
            if(info.Cache)
            {
                info.Instance.SetActive(false);
            }
            else
            {
                GameObject.Destroy(info.Instance);
                info.Instance = null;
            }
        }
    }
}
