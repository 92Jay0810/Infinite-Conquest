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
        //UI Stage���Y���p�C��3D�V�E��r�L�p�C�������Ѝ�canva��
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
        //��Q���b�Icanva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // �� Canvas �I�q�����ʒu�n������ prefab
        GameObject spawnedText = Instantiate(openingText_1, canvas.transform);
        // �ݒu���n�ʒu
        spawnedText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -400);
    }
    private void CreateOpeningText_2(List<string> properties)
    {
        //��Q���b�Icanva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // �� Canvas �I�q�����ʒu�n������ prefab
        GameObject spawnedText = Instantiate(openingText_2, canvas.transform);
        // �ݒu���n�ʒu
        spawnedText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -450);
    }
    private void InputPlayerText(List<string> properties)
    {
        //��Q���b�Icanva
        GameObject canvas = GameObject.Find("DefaultDialogPrefab(Clone)");
        // �� Canvas �I�q�����ʒu�n������ prefab
        GameObject InputPlayerField_prefab = Instantiate(InputPlayerField, canvas.transform);
        //�ݒu���n�ʒu
        InputPlayerField_prefab.GetComponent<Transform>().position = new Vector2(-50, -100);
        //�����������
        InputField inputField = InputPlayerField_prefab.GetComponent<InputField>();
        inputField.onEndEdit.AddListener(OnSubmit);

    }
    //�߉Ɩ��i����
    void OnSubmit(string input)
    {
       // myString = input;  �X�V����
       //  UpdateDisplayText(); �X�V��������
    }

}
