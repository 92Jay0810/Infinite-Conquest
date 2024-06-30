using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flower;
using System;
using UnityEngine.UI;

public class opening : MonoBehaviour
{
    FlowerSystem fs;
    private int progress = 0;
    private bool gameEnd = false;
    [SerializeField] GameObject openingText_1;
    [SerializeField] GameObject openingText_2;
    private string playername = "";
    [SerializeField] GameObject InputPlayerField;
    [SerializeField] GameObject playerbutton;
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem("default", false);
        fs.SetupDialog();
        //UI Stage沒什麼用，在3D遊戲比較有用，顯示圖片在canva中
        //fs.SetupUIStage("default", "DefaultUIStagePrefab", 10);
        fs.RegisterCommand("CreateOpeningText_1", CreateOpeningText_1);
        fs.RegisterCommand("CreateOpeningText_2", CreateOpeningText_2);
        fs.RegisterCommand("InputPlayerText", InputPlayerText);
        fs.SetVariable("playername", playername);
    }


    void Update()
    {
        if (fs.isCompleted && !gameEnd)
        {
            switch (progress)
            {
                case 0:
                    fs.ReadTextFromResource("SingleMode/opening_1");
                    progress = 1;
                    break;
                case 1:
                    fs.SetupButtonGroup();
                        fs.SetupButton("同意.", () =>
                        {
                            gameEnd = false;
                            fs.RemoveButtonGroup();
                            fs.SetTextList(new List<string> { "那你先去訓練場找教官吧。[w]" });
                            progress = 2;
                        });
                    fs.SetupButton("不同意", () =>
                    {
                        gameEnd = false;
                        fs.RemoveButtonGroup();
                        fs.SetTextList(new List<string> { "真的不願意嗎?[w]" });
                    });
                    gameEnd = true;
                    break;
                case 2:
                    fs.ReadTextFromResource("SingleMode/opening_2");
                    progress = 3;
                    break;
                case 3:

                    break;
            }
        }
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
        InputPlayerField_prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -100);
        GameObject Playerbutton_prefab = Instantiate(playerbutton, canvas.transform);
        Playerbutton_prefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(250, -100);
        //增加提交事件
        Button buttonComponent = Playerbutton_prefab.GetComponent<Button>();
        buttonComponent.onClick.AddListener(() =>
        {
            // 更新字串
            playername = InputPlayerField_prefab.GetComponent<InputField>().text;
            fs.SetVariable("playername", playername);
            fs.Resume();
            fs.Next();
            // 刪除 InputField 和按鈕
            Destroy(InputPlayerField_prefab);
            Destroy(Playerbutton_prefab);
        });
    }
}
