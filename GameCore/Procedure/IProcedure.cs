using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProcedure
{
    void OnEnter();
    void OnUpdate();
    void OnLeave();
}
