using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    // delegate
    public Action KeyAction = null;

    public void OnUpdate() // Ű���� üũ�� �����ϰ� �ϱ�. ����׿��� ���� Ű���带 ����ϴ��� �� �� �־ ������
    {
        if (Input.anyKey == false)
            return;

        if (KeyAction != null)
            KeyAction.Invoke();
    }
}
