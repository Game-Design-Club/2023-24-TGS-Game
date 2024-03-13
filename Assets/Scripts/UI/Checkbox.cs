using UnityEngine;
using UnityEngine.Events;

namespace UI {
    public class Checkbox : MonoBehaviour{
        [SerializeField] public bool state = false;
        [SerializeField] private GameObject checkmark;
        [SerializeField] private UnityEvent<bool> OnToggle;
        
        public void Toggle() {
            state = !state;
            checkmark.SetActive(state);
        }
    }
}