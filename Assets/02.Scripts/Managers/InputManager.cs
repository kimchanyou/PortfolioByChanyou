using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // delegate
    public Action KeyAction = null;

    public void OnUpdate() // 키보드 체크를 유일하게 하기. 디버그에도 누가 키보드를 사용하는지 알 수 있어서 용이함
    {
        if (Input.anyKey == false)
            return;

        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
