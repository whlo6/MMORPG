using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;
using Services;

public class PlayerInputController : MonoBehaviour {

    public Rigidbody rb;
    SkillBridge.Message.CharacterState state;//角色状态
    public Character character;
    public float rotateSpeed = 2.0f;//旋转速度
    public float turnAngle = 10;//转角
    public int speed;
    public EntityController entityController;
    public bool onAir = false;//广播

    int isJump = 80;//跳跃内置cd

    // Use this for initialization
    void Start () {
        state = SkillBridge.Message.CharacterState.Idle;
        if(this.character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            this.character = new Character(cinfo);

            if (entityController != null) entityController.entity = this.character;
        }
    }


    void FixedUpdate()
    {
        if (character == null)
            return;

        
        //角色移动控制
        float v = Input.GetAxis("Vertical");
        if (v > 0.01)//前进
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();//获取角色该状态下的移动速度
                this.SendEntityEvent(EntityEvent.MoveFwd);//设置角色移动状态，同步
            }
            //保持角色当前竖直速度（下落、跳跃）不变  GameObjectTool.LogicToWorld（）//逻辑位置转换为世界位置
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else if (v < -0.01)//后退
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else
        {
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = SkillBridge.Message.CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        //角色跳跃控制
        if (isJump++ > 80 && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("角色跳跃");
            isJump = 0;
            this.SendEntityEvent(EntityEvent.Jump);
        }

        //角色旋转控制
        float h = Input.GetAxis("Horizontal");
        if (h < -0.1 || h > 0.1)
        {
            this.transform.Rotate(0, h * rotateSpeed, 0);
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);//设置实体物体的转向

            if (rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));//设置网络数据转向
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }

        }
    }

    Vector3 lastPos;
    private void LateUpdate()
    {
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        this.lastPos = this.rb.transform.position;

        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 5)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        this.transform.position = this.rb.transform.position; 
    }

    void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);//设置实体动画状态
        MapService.Instance.SendMapEntitySync(entityEvent, this.character.EntityData);//发送角色位置同步消息
    }
}
