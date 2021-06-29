using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Puzzle15
{
    public class TouchDetector : MonoBehaviour, IPointerClickHandler
    {
        public event Action<PointerEventData> EventClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            EventClick?.Invoke(eventData);
        }
    }
}
