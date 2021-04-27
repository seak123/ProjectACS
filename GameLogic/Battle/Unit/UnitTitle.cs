using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTitle : MonoBehaviour
{
    private UnitAvatar _master;
    private Text _hpText;
    private Text _energyText;
    private Text _nameText;
    private GameObject _selectFlag;
    private void Awake()
    {
        _hpText = transform.Find("HP").GetComponent<Text>();
        _nameText = transform.Find("Name").GetComponent<Text>();
        _selectFlag = transform.Find("Selected").gameObject;
        _energyText = transform.Find("Name/Energy").GetComponent<Text>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_master)
        {
            var worldPos = _master.transform.position + new Vector3(0, 1, 0);
            transform.position = Camera.main.WorldToScreenPoint(worldPos);
        }
    }

    public void BindUnit(UnitAvatar unit)
    {
        _master = unit;

        _nameText.text = _master.Name;
        RefreshHp();
        RefreshEnergy();
        _selectFlag.SetActive(false);
    }

    public void RefreshHp()
    {
        _hpText.text = _master.Hp.ToString() + "/" + _master.MaxHp.ToString();
    }

    public void RefreshEnergy()
    {
        _energyText.text = _master.Energy.ToString() + "/" + _master.MaxEnergy.ToString();
    }

    public void SetSelectFlag(bool bShow)
    {
        _selectFlag.SetActive(bShow);
    }
}
