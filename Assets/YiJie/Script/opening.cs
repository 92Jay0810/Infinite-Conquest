using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using System;
using UnityEngine.UI;

public class opening : MonoBehaviour
{
    FlowerSystem fs;
    [SerializeField] GameObject openingText_1;
    [SerializeField] GameObject openingText_2;
    [SerializeField] GameObject InputPlayerField;
    // Start is called before the first frame update
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem("default", false);
        fs.SetupDialog();
        //UI Stage沒什麼用，在3D遊戲比較有用，顯示圖片在canva中
        //fs.SetupUIStage("default", "DefaultUIStagePrefab", 10);
        fs.ReadTextFromResource("SingleMode/test");
        fs.RegisterCommand("CreateOpeningText_1", CreateOpeningText_1);
        fs.RegisterCommand("CreateOpeningText_2", CreateOpeningText_2);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fs.Next();
        }
    }
    private void CreateOpeningText_1(List<string> properties)
    {
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建文字 prefab
        GameObject spawnedText = Instantiate(openingText_1, canvas.transform);
        // 設置初始位置
        spawnedText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
    }
    private void CreateOpeningText_2(List<string> properties)
    {
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建文字 prefab
        GameObject spawnedText = Instantiate(openingText_2, canvas.transform);
        // 設置初始位置
        spawnedText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -450);
    }
    private void InputPlayerText(List<string> properties)
    {
        //先找對話的canva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // 在 Canvas 的子物件位置創建文字 prefab
        GameObject InputPlayerField_prefab = Instantiate(InputPlayerField, canvas.transform);
        //設置初始位置
        InputPlayerField_prefab.GetComponent<Transform>().position = new Vector2(-50, -100);
        //增加提交事件
        InputField inputField = InputPlayerField_prefab.GetComponent<InputField>();
        inputField.onEndEdit.AddListener(OnSubmit);

    }
    //玩家名稱未做完
    void OnSubmit(string input)
    {
       // myString = input;  更新字串
       //  UpdateDisplayText(); 更新顯示文字
    }

}
