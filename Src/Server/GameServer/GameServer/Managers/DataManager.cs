using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;

using Common;
using Common.Data;

using Newtonsoft.Json;
namespace GameServer.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        internal string DataPath;
        internal Dictionary<int, MapDefine> Maps = null;
        internal Dictionary<int, CharacterDefine> Characters = null;
        internal Dictionary<int, TeleporterDefine> Teleporters = null;
        public Dictionary<int, Dictionary<int, SpawnPointDefine>> SpawnPoints = null;
        public Dictionary<int, Dictionary<int,SpawnRuleDefine>> SpawnRules = null;
        public Dictionary<int, NpcDefine> Npcs = null;
        public Dictionary<int, ItemDefine> Items = null;
        public Dictionary<int, ShopDefine> Shop = null;
        public Dictionary<int, Dictionary<int, ShopItemsDefine>> ShopItems = null;
        public Dictionary<int, EquipmentDefine> Equipments = null;
        public Dictionary<int, QuestDefine> Quests = null;
        
        public DataManager()
        {
            this.DataPath = "Data/";
            Log.Info("DataManager > DataManager()");
        }

        internal void Load()
        {
            string json = File.ReadAllText(this.DataPath + "MapDefine.txt");
            this.Maps = JsonConvert.DeserializeObject<Dictionary<int, MapDefine>>(json);

            json = File.ReadAllText(this.DataPath + "CharacterDefine.txt");
            this.Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "TeleporterDefine.txt");
            this.Teleporters = JsonConvert.DeserializeObject<Dictionary<int, TeleporterDefine>>(json);

            json = File.ReadAllText(this.DataPath + "NpcDefine.txt");
            this.Npcs = JsonConvert.DeserializeObject<Dictionary<int, NpcDefine>>(json);

            json = File.ReadAllText(this.DataPath + "IteamDefine.txt");
            this.Items = JsonConvert.DeserializeObject<Dictionary<int, ItemDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ShopDefine.txt");
            this.Shop = JsonConvert.DeserializeObject<Dictionary<int, ShopDefine>>(json);

            json = File.ReadAllText(this.DataPath + "ShopItemsDefine.txt");
            this.ShopItems = JsonConvert.DeserializeObject<Dictionary<int, Dictionary<int, ShopItemsDefine>>>(json);

            json = File.ReadAllText(this.DataPath + "EquipmentDefine.txt");
            this.Equipments = JsonConvert.DeserializeObject<Dictionary<int, EquipmentDefine>>(json);

            json = File.ReadAllText(this.DataPath + "QuestDefine.txt");
            this.Quests = JsonConvert.DeserializeObject<Dictionary<int, QuestDefine>>(json);
        }
    }
}