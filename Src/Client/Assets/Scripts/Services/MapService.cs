using System;
using Network;
using UnityEngine;

using Common.Data;
using SkillBridge.Message;
using Models;
using Managers;
using Assets.Scripts.Managers;

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {

        public int CurrentMapId = 0;//记录当前的地图id
        public MapService()
        {
            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);

            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(this.OnMapEntitySync);

        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(this.OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(this.OnMapCharacterLeave);

            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(this.OnMapEntitySync);
        }

        public void Init()
        {
            Debug.Log("MapServiceInit");
        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.LogFormat("OnMapCharacterEnter:Map:{0} Count:{1}", response.mapId, response.Characters.Count);
            //当前角色切换地图，将已经存在于该地图的其他玩家加入进来
            foreach (var cha in response.Characters)
            {
                if (User.Instance.CurrentCharacter == null || User.Instance.CurrentCharacter.Id == cha.Id)
                {
                    User.Instance.CurrentCharacter = cha;//对玩家进来前的数据和进来后的数据进行二次同步
                }
                CharacterManager.Instance.AddCharacter(cha);
            }
            if (CurrentMapId != response.mapId)//如果记录下的地图id与即将进入的地图的id不同，则表示并非首次进入游戏
            {
                this.EnterMap(response.mapId);
                this.CurrentMapId = response.mapId;//更新地图id
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.LogFormat("OnMapCharacterLeave:CharID:{0}", response.characterId);

            if (response.characterId != User.Instance.CurrentCharacter.Id)//判断即将离开的角色是不是自己
                CharacterManager.Instance.RemoveCharacter(response.characterId);
            else
            {
                CharacterManager.Instance.Clear();
            }
                
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))//如果该地图id存在，则载入地图资源
            {
                //保存当前地图资源并载入地图
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
                Debug.LogErrorFormat("EnterMap: Map {0} not existed", mapId);
        }

        public void SendMapEntitySync(EntityEvent entityEvent,NEntity entity)
        {
            Debug.LogFormat("SendMapEntitySyncRequest：ID:{0} POS:{1} DIR:{2} SPD:{3} ",entity.Id,entity.Position.String(),entity.Direction.String(),entity.Speed);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity
            };
            NetClient.Instance.SendMessage(message);
        }

        //同步地图内的角色
        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("MapEntityUpdateResponse: Entity{0}", response.entitySyncs.Count);
            sb.AppendLine();//追加一个换行符
            foreach(var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);//对地图内所有角色同步
                sb.AppendFormat("    [{0}]evt:{1} entity:{2}", entity.Id, entity.Event, entity.Entity.String());
                sb.AppendLine();
            }
            Debug.Log(sb.ToString());
        }

        public void SendMapTeleport(int teleporterID)
        {
            UpdateLastTransform(teleporterID);//在传送地图时，更新至新地图的位置
            Debug.LogFormat("SendMapTeleport :teleporterID :{0}", teleporterID);
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }


        public void UpdateLastTransform(int teleporterID)//在传送地图时，更新至新地图的位置
        {
            int linkTo = DataManager.Instance.Teleporters[teleporterID].LinkTo;
        }
    }
}
