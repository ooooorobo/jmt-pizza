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
    public Image background;
    public Image left;
    public Image right;
    
    
    private void Awake()
    {
        dialogList = CSVReader.Read("Datas/Dialog/kingdom_script_stage_" + stageNumber);
        dialogLength = dialogList.Count;
    }

    private void Start()
    {
        // for (int i = 0; i < dialogLength; i++)
        // {
        //     Dictionary<string, object> dialog = dialogList[dialogIndex++];
        //     
        // }
        
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
        
        charName.text = dialog["name"].ToString();;
        originalText = dialog["contents"].ToString();
        
        // TODO:: 리소스 미리 불러오는 방식으로 변경 필요
        background.sprite = Resources.Load<Sprite>("Datas/Sprite/" + dialog["background"]);
        if (dialog["left"].ToString().Length > 0)
        {
            left.sprite = Resources.Load<Sprite>("Datas/Sprite/" + dialog["left"]);
            left.enabled = true;
        }
        else left.enabled = false;
        if (dialog["right"].ToString().Length > 0)
        {
            right.sprite = Resources.Load<Sprite>("Datas/Sprite/" + dialog["right"]);
            right.enabled = true;
        }
        else right.enabled = false;

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
