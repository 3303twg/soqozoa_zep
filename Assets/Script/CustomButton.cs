using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public override void OnSubmit(BaseEventData eventData)
    {
        // 엔터 키 입력에 대해 아무 동작도 하지 않음
        if (eventData is PointerEventData pointerEventData)
        {
            return;
        }

    }
}
