using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Crafter.UI
{
    public class ButtonTextAnchor : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField]
        private Text textElement;
        [SerializeField]
        private TextAnchor anhorDown;
        [SerializeField]
        private TextAnchor anhorUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            textElement.alignment = anhorDown;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            textElement.alignment = anhorUp;
        }
    }
}