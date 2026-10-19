using Assets.Scripts.Managers;
using Models;
using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMessagManager : Singleton<PlayerMessagManager>
{
    public int maxHp;
    public int maxMp;
    public int hp = 0;//生命
    public int mp = 0;//法力
    public int atk;//攻击
    public int sta;//防御
    public int wit;//智力

    public CharacterClass characterClass;
    public int level;

    public void Init()
    {
        this.characterClass = User.Instance.CurrentCharacter.Class;
        this.level = User.Instance.CurrentCharacter.Level;

        //获取职业及等级基础三维
        switch (this.characterClass)
        {
            case CharacterClass.None:
                break;
            case CharacterClass.Warrior://战士
                atk = 25 * level;
                sta = 25 * level;
                wit = 10 * level;
                break;
            case CharacterClass.Wizard:
                atk = 25 * level;
                sta = 5 * level;
                wit = 30 * level;
                break;
            case CharacterClass.Archer:
                atk = 30 * level;
                sta = 15 * level;
                wit = 15 * level;
                break;
            default:
                break;
        }

        //获取装备加成三维
        for(int i=0;i<4;i++)
        {
            int n = EquipmentManager.Instance.equipId[i];
            if (n == -1) continue;
            this.atk += DataManager.Instance.Equipments[n].ATK;
            this.sta += DataManager.Instance.Equipments[n].STA;
            this.wit += DataManager.Instance.Equipments[n].WIT;
        }
        //计算生命和法力
        this.maxHp = this.sta * 10;
        this.maxMp = this.wit * 10;
        if(this.hp==0)
            this.hp = this.maxHp;
        if(this.mp==0)
            this.mp = this.maxMp;
    }
}
