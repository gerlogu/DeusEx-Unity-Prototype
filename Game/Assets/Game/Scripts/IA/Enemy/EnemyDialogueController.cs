using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Febucci.UI;
using System;

public class EnemyDialogueController : MonoBehaviour
{
    public GameObject container;

    private TextMeshPro dialogueText;
    private TextAnimatorPlayer textAnimatorPlayer;
    private GameObject playerCamera;
    private float timeToHideQuote = 1f;
    private float textFadeSmooth = 8f;
    private bool hideText = false;
    private Color hideColor;
    private Color originalColor;
    private string sentence = "";
    private Enemy enemy;
    

    // Load the data from the database
    string sentecesDBcontent;
    string[] sentencesData;


    List<string[]> values;

    // Start is called before the first frame update
    void Start()
    {
        dialogueText = GetComponentInChildren<TextMeshPro>();
        textAnimatorPlayer = GetComponentInChildren<TextAnimatorPlayer>();
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if (!dialogueText)
            return;

        hideColor = new Color(dialogueText.color.r, dialogueText.color.g, dialogueText.color.b, 0);
        originalColor = dialogueText.color;

        sentecesDBcontent = Resources.Load<TextAsset>("Sentences/Enemies/EnemySentencesDB").text;
        sentencesData = sentecesDBcontent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        values = new List<string[]>();
        enemy = GetComponent<Enemy>();

        for(int i = 1; i< sentencesData.Length - 1; i++)
        {
            values.Add(sentencesData[i].Split(';'));
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!dialogueText)
            return;

        dialogueText.gameObject.transform.LookAt(2 * dialogueText.gameObject.transform.position - playerCamera.transform.position);

        if (container.transform.parent != null && enemy.isDead)
        {
            container.transform.parent = null;
        }

        if (hideText || (enemy.isDead && enemy.collisionsDisabled))
        {
            if(dialogueText.color.a != 0 || (enemy.isDead && enemy.collisionsDisabled))
            {
                dialogueText.color = Color.Lerp(dialogueText.color, hideColor, textFadeSmooth * Time.deltaTime);
                if (dialogueText.color.a < .01f)
                {
                    dialogueText.color = hideColor;
                }
            } 
            else
            {
                hideText = false;
                dialogueText.color = originalColor;
                int random = UnityEngine.Random.Range(0, values.Count);
                sentence = values[random][1];
                textAnimatorPlayer.ShowText(sentence);
            }
        }
        
    }

    public void HideQuote()
    {
        Invoke("FinallyHideQuote", 2.5f);
    }



    void FinallyHideQuote()
    {
        hideText = true;
    }
}
