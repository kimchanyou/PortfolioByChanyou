using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    // delegate
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MounseAction = null;

    bool _pressed = false;
    float _pressedTime = 0;

    public void OnUpdate() // Ű���� üũ�� �����ϰ� �ϱ�. ����׿��� ���� Ű���带 ����ϴ��� �� �� �־ ������
    {
        if (EventSystem.current.IsPointerOverGameObject()) // UI�� Ŭ���� ��Ȳ�̶��
            return;

        if (Input.anyKey && KeyAction != null)
            KeyAction.Invoke();

        if (MounseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                    MounseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MounseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if (Time.time < _pressedTime + 0.2f)
                        MounseAction.Invoke(Define.MouseEvent.Click);
                    MounseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    public void Clear()
    {
        KeyAction = null;
        MounseAction = null;
    }
}
