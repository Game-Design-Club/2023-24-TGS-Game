using UnityEngine;
using UnityEngine.Events;

namespace UI {
    public class Checkbox : MonoBehaviour { // Simple checkbox UI element that can toggle a boolean state
        [SerializeField] public bool state = false;
        [SerializeField] private GameObject checkmark;
        [SerializeField] private UnityEvent<bool> OnToggle;
        
        public void Toggle() {
            state = !state;
            checkmark.SetActive(state);
            OnToggle?.Invoke(state);
        }
        
        public void SetState(bool value) {
            state = value;
            checkmark.SetActive(state);
        }
    }
}