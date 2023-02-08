using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawManager : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler
{
    public static GameObject beginDraggedIcon;

    Vector3 startPosition;

    [SerializeField] Transform onDragParent;
    [HideInInspector] public Transform startParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginDraggedIcon = gameObject;

        startPosition = transform.position;
        startParent = transform.parent;

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        transform.SetParent(onDragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        beginDraggedIcon = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if(transform.parent == onDragParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }

}
