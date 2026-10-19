using Managers;
using Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMiniMap : MonoBehaviour {

    public Collider miniMapBordingBox;
    public Transform playerTransform;

    public Image minimap;
    public Image arrow;//小地图中心点，即玩家所在位置
    public Text mapName;

	// Use this for initialization
	void Start () {
        //InitMap();
	}
	
    public void InitMap()
    {
        this.mapName.text = User.Instance.CurrentMapData.Name;
        this.minimap.overrideSprite = MiniMapManager.Instance.LoadCurrentMiniMap();//更新小地图资源

        this.minimap.SetNativeSize();
        this.minimap.transform.localPosition = Vector3.zero;

        if(miniMapBordingBox == null)
        {
            miniMapBordingBox = MiniMapManager.Instance.miniMapBordingBox;
        }
    }

	// Update is called once per frame
	void Update () {
        //角色信息为空时加载
        if(playerTransform==null && MiniMapManager.Instance.PlayerTransform!=null)
        {
            playerTransform = MiniMapManager.Instance.PlayerTransform; 
        }
        //当前小地图盒子为空或者当前地图图片资源对不上，重新初始化小地图
        if (miniMapBordingBox == null || MiniMapManager.Instance.CurrentMiniMap != this.minimap.overrideSprite)
            InitMap();
        //资源残缺时不进行小地图位置处理
        if (miniMapBordingBox == null || playerTransform == null) return;

        float realWidth = miniMapBordingBox.bounds.size.x;
        float realHeight = miniMapBordingBox.bounds.size.z;

        float relaX = playerTransform.position.x - miniMapBordingBox.bounds.min.x;
        float relaY = playerTransform.position.z - miniMapBordingBox.bounds.min.z;

        float pivotX = relaX / realWidth;
        float pivotY = relaY / realHeight;

        minimap.rectTransform.pivot = new Vector2(pivotX, pivotY);
        minimap.transform.localPosition = Vector2.zero;

        this.arrow.transform.eulerAngles = new Vector3(0, 0, -playerTransform.eulerAngles.y); 
    }
}
