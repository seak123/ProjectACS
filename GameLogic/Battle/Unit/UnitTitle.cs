using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTitle : MonoBehaviour
{
    private UnitAvatar _master;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_master)
        {
            var worldPos = _master.transform.position + new Vector3(0, 1, 0);
            transform.position = Camera.main.WorldToScreenPoint(worldPos);
        }
    }

    public void BindUnit(UnitAvatar unit)
    {
        _master = unit;
    }
}
