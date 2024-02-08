using UnityEngine;

namespace Services
{
    public class InputService
    {
        public Vector2 GetMovement() =>
            new Vector2(SimpleInput.GetAxis("Horizontal"), SimpleInput.GetAxis("Vertical"));

        public bool IsFirePressed() =>
            SimpleInput.GetButton("Fire");

        public bool IsTouched() =>
            Input.touchCount > 0;

        public Touch GetTouch() =>
            Input.GetTouch(0);
    }
}