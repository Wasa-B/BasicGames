using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
namespace SweetRoad
{
    public interface ISwap
    {
        public void OnSwap(Vector2 dir);
    }
    public class SwapManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        
        public static GameObject beginSwipeObject;
        static bool dragStart = false;
        Vector3 startPosition;
        
        [HideInInspector] public Transform startParent;
        private void Awake()
        {
            
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            dragStart = true;
            beginSwipeObject = gameObject;
            startPosition = transform.position;
            startParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            
            //transform.SetParent(onDragParent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (dragStart)
            {
                Vector2 dir = CalculateDirection(startPosition, eventData.position);
                if (dir != Vector2.zero) {
                    GetComponent<ISwap>().OnSwap(dir);
                    dragStart = false;
                }
                
            }
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            beginSwipeObject = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        Vector2 CalculateDirection(Vector2 start, Vector2 end)
        {
            Vector2 dir = end - start;
            if(dir.magnitude < 30)
            {
                return Vector2.zero;
            }
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (angle < 45 && angle >= -45) return Vector2.right;
            else if (angle < 135 && angle >= 45) return Vector2.down;
            else if (angle < -45 && angle >= -135) return Vector2.up;
            else return Vector2.left;

        }
    }
}