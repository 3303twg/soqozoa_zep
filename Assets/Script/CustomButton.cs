using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public override void OnSubmit(BaseEventData eventData)
    {
        // ���� Ű �Է¿� ���� �ƹ� ���۵� ���� ����
        if (eventData is PointerEventData pointerEventData)
        {
            return;
        }

    }
}
