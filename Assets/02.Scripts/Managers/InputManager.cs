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

    public void OnUpdate() // 키보드 체크를 유일하게 하기. 디버그에도 누가 키보드를 사용하는지 알 수 있어서 용이함
    {
        if (EventSystem.current.IsPointerOverGameObject()) // UI가 클릭된 상황이라면
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
