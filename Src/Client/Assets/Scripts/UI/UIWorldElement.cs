using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldElement : MonoBehaviour {

    public Transform owner;
    public GameObject hasQuest;

    public float height =2f;

    NpcController npcController = null;

    // Use this for initialization
    void Start () {
        height = 2.2f;
        npcController = this.owner.gameObject.GetComponent<NpcController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (owner != null)
        {
            this.transform.position = owner.position + Vector3.up * height;
        }
        if(npcController!=null && npcController.hasQuest)
        {
            this.hasQuest.SetActive(true);
        }
        else
            this.hasQuest.SetActive(false);
    }
}
