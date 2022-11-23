﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Joystick_Pack.Scripts.Joysticks
{
    public class DynamicJoystick : Joystick
    {
        public float MoveThreshold
        {
            get => moveThreshold;
            set => moveThreshold = Mathf.Abs(value);
        }

        [SerializeField] private float moveThreshold = 1;

        public static DynamicJoystick Instance;

        protected override void Start()
        {
            MoveThreshold = moveThreshold;
            base.Start();
            background.gameObject.SetActive(false);

            if (Instance == null)
                Instance = this;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
        }

        protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > moveThreshold)
            {
                Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
                background.anchoredPosition += difference;
            }

            base.HandleInput(magnitude, normalised, radius, cam);
        }
    }
}