using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private int stageNumber = 1;
    private List<Dictionary<string, object>> dialogList;
    private int dialogIndex = 0;
    private int dialogLength;
    
    private bool isTyping = false;
    private string originalText = "";

    private IEnumerator typingEffect;
    
    [Header("UI")]
    public Text charName;
    public Text charContents;
    
    
    private void Awake()
    {
        dialogList = CSVReader.Read("Datas/Dialog/story_" + stageNumber);
        dialogLength = dialogList.Count;
    }

    private void Start()
    {
        ReadDialogLine();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (isTyping) StopTypingEffect();
            else if (dialogIndex < dialogLength) ReadDialogLine();
            else Debug.Log("끝");
        }
    }
    
    private void ReadDialogLine()
    {
        Dictionary<string, object> dialog = dialogList[dialogIndex++];
        
        charName.text = "";
        originalText = dialog["content"].ToString();

        if (typingEffect != null)
            StopCoroutine(typingEffect);
        
        typingEffect = TypingTextEffect();

        StartCoroutine(typingEffect);
    }

    private void StopTypingEffect()
    {
        isTyping = false;
        charContents.text = originalText;

        if (typingEffect == null) return;
        
        StopCoroutine(typingEffect);
        typingEffect = null;
    }

    IEnumerator TypingTextEffect()
    {
        isTyping = true;
        string tempString = "";
        int charIndex = 0;

        while (isTyping && tempString != originalText)
        {
            tempString += originalText[charIndex++];
            charContents.text = tempString;

            yield return new WaitForSeconds(0.03f);
        }

        isTyping = false;
    } 
}    
