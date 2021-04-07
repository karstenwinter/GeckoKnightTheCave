﻿using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.InputSystem.OnScreen
{
    /// <summary>
    /// A stick control displayed on screen and moved around by touch or other pointer
    /// input.
    /// </summary>
    [AddComponentMenu("Input/On-Screen Stick")]
    public class OnScreenStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public string mode;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
        }
 
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
            var delta = position - m_PointerDownPos;

            delta = Vector2.ClampMagnitude(delta, movementRange);
            ((RectTransform)transform).anchoredPosition = m_StartPos + (Vector3)delta;

            var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);
            SendValueToControl(newPos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ((RectTransform)transform).anchoredPosition = m_StartPos;
            SendValueToControl(Vector2.zero);
        }

        private void Start()
        {
            m_StartPos = ((RectTransform)transform).anchoredPosition;
        }

        void SendValueToControl(Vector2 arg) {
            if(mode == "Move") {
                Input2.hor = arg.x;
                Input2.vert = arg.y;
            } else if(mode == "Hit") {
                Input2.jump = arg.y > 0.1 ? (bool?) true : null;
                Input2.hit = arg.x < -0.1 ? (bool?) true : null;
                Input2.overrideFlip = arg.x < -0.1 ? (bool?) true : null;
                Input2.crouch = arg.y < -0.1 ? (bool?) true : null;
            }
        }

        public float movementRange
        {
            get => m_MovementRange;
            set => m_MovementRange = value;
        }

        [FormerlySerializedAs("movementRange")]
        [SerializeField]
        private float m_MovementRange = 50;

        //[SerializeField]
        //private string m_ControlPath;

        private Vector3 m_StartPos;
        private Vector2 m_PointerDownPos;
    }
}