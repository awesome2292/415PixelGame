using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using VIDE_Data;

public class Conversation : MonoBehaviour
{

    public float playerMorals;
    public float npcFriendliness;
    public float extraVar;

    public bool nice;
    public bool mean;
    public bool neutral;

    public VIDE_Assign assign;
    public SpriteController player;

    public GameObject indicator;

    // Start is called before the first frame update
    void Start()
    {
        assign = GetComponent<VIDE_Assign>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteController>();
        playerMorals = player.moralCompass;

        indicator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region TRIGGERS
    void OnTriggerEnter2D(Collider2D other)
    {
        indicator.gameObject.SetActive(true);


    }

    void OnTriggerExit2D()
    {
        indicator.gameObject.SetActive(false);

    }

    #endregion

    public void MeanNode()
    {
        mean = true;
        playerMorals -= 0.1f;

        if(mean && playerMorals <= 0.3f)
        {
            VD.SetNode(50);
        }
    }

    public void CheckMorals()
    {
        if(playerMorals > 0.6)
        {
            VD.SetNode(VD.currentActionNode.ID + 1);
        }

        if(playerMorals < 0.4)
        {
            VD.SetNode(VD.currentActionNode.ID + 2);
        }
    }

    public void RandomizeNode()
    {
        VD.SetNode(Random.Range(VD.currentActionNode.ID + 1, VD.currentActionNode.ID + 3));
    }

    public void TellMeSomething()
    {
        VD.SetNode(Random.Range(VD.currentActionNode.ID + 1, VD.currentActionNode.ID + 5));
    }

}
