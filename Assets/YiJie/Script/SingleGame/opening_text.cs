using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opening_text : MonoBehaviour
{
    public float speed = 50f; // 移動速度
    public float destroyHeight = 1000f; // 刪除的高度

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // 持續往上移動文字 prefab
        rectTransform.Translate(Vector3.up * speed * Time.deltaTime);

        // 當文字 prefab 到達一定高度時刪除它
        if (rectTransform.anchoredPosition.y >= destroyHeight)
        {
            Destroy(gameObject);
        }
    }
}
