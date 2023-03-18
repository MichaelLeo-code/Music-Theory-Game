using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Scroll_rect : ScrollRect
{
    bool isScrollableByTouch = false;

    public void ScrollOnOff(){
        isScrollableByTouch = !isScrollableByTouch;
        enabled = true;
        // Debug.Log("ScrollOnOff()");
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        base.OnBeginDrag(eventData);
        // Debug.Log("OnBeginDrag");
        if(isScrollableByTouch)
        {
            StopMovement();
            enabled = false;
        }
    }
    public override void OnEndDrag(PointerEventData eventData) {
        base.OnEndDrag(eventData);
        // Debug.Log("OnEndDrag");
        // enabled = true;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        enabled = true;
    }
}
