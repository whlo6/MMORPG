using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnetService : MonoBehaviour {

	// Use this for initialization
	void Start () {

        #region 客户端连接到服务器
        Network.NetClient.Instance.Init("127.0.0.1", 8000);//（服务器地址，端口号）
        Network.NetClient.Instance.Connect();
        #endregion

        //SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage();
        //msg.Request = new SkillBridge.Message.NetMessageRequest();

        //SkillBridge.Message.FirstTestRequest firstRequest = new SkillBridge.Message.FirstTestRequest();
        //firstRequest.Helloworld = "hello world";

        //msg.Request.firstRequest = firstRequest;

        //Network.NetClient.Instance.SendMessage(msg);
    }
	
	// Update is called once per frame
	void Update () {
		 	}
}
