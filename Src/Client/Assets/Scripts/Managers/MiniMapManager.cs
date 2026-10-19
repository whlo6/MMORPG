using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Managers
{
    class MiniMapManager : Singleton<MiniMapManager>
    {
        public Sprite CurrentMiniMap;
        public Collider miniMapBordingBox;

        public Transform PlayerTransform
        {
            get
            {
                if (User.Instance.CurrentCharacterObject == null)
                    return null;
                return User.Instance.CurrentCharacterObject.transform;
            }
        }
        public Sprite LoadCurrentMiniMap()
        {
            Debug.Log("当前地图资源：" + User.Instance.CurrentMapData.MiniMap);
            if (CurrentMiniMap == null)
                UpdateCurrentMiniMap();
            return CurrentMiniMap;
        }

        public void UpdateCurrentMiniMap()
        {
            CurrentMiniMap = Resloader.Load<Sprite>("UI/MiniMap/" + User.Instance.CurrentMapData.MiniMap);
        }
    }
}
