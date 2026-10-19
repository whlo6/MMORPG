using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour {

    public Collider miniMapBordingBox;

    void Start () {
        MiniMapManager.Instance.miniMapBordingBox = this.miniMapBordingBox;
        MiniMapManager.Instance.UpdateCurrentMiniMap();//更新小地图资源
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
