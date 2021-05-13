using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ValueNoticeType
{
    Damage = 1,
    Heal = 2
}

public class ValueNotice : MonoBehaviour
{
    private Text _value;
    // Start is called before the first frame update
    void Awake()
    {
        _value = transform.Find("Value").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int value,ValueNoticeType noticeType)
    {
        switch(noticeType)
        {
            case ValueNoticeType.Damage:
                _value.text = "-" + value.ToString();
                _value.color = Color.red;
                break;
            case ValueNoticeType.Heal:
                _value.text = "+" + value.ToString();
                _value.color = Color.green;
                break;
        }

        gameObject.transform.DOMoveY(gameObject.transform.position.y + 50, 0.3f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
