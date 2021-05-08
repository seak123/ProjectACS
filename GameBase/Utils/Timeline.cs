using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timeline
{
    private bool bFinish;
    public Timeline()
    {
        bFinish = false;
    }
    public bool Play(float deltaTime)
    {
        return bFinish;
    }
}