using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opening_text : MonoBehaviour
{
    public float speed = 50f; // �ړ����x
    public float destroyHeight = 1000f; // �����I���x

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // ��㔉���ړ����� prefab
        rectTransform.Translate(Vector3.up * speed * Time.deltaTime);

        // �c���� prefab ���B��荂�x��������
        if (rectTransform.anchoredPosition.y >= destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
