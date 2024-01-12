using System;
using System.Collections;
using System.Collections.Generic;

using AppCore;

using Constants;

using TMPro;

using UnityEngine;
using UnityEngine.Serialization;

namespace Credits {
    public class CreditsScroller : MonoBehaviour {
        [FormerlySerializedAs("creditsInfo")] [SerializeField] private CreditsAsset creditsAsset;
        [SerializeField] private GameObject parentObject;
        [SerializeField] private float scrollSpeed = 1f;

        [SerializeField] private float endY;
        
        [SerializeField] private GameObject thankYouObject;
        [SerializeField] private float thankYouDelay = 1f;

        private bool _scrolling = true;

        // Unity functions
        private void OnEnable() {
            App.Instance.inputManager.OnCancelPressed += OnCancelPressed;
        }

        private void OnDisable() {
            App.Instance.inputManager.OnCancelPressed -= OnCancelPressed;
        }

        private void Update() {
            if (!_scrolling) return;
            parentObject.transform.position += Vector3.up * (scrollSpeed * Time.deltaTime);
            if (parentObject.transform.position.y > endY) {
                _scrolling = false;
                StartCoroutine(ShowThankYou());
            }
        }

        // Private functions
        private void OnCancelPressed() {
            App.Instance.sceneManager.LoadScene(SceneConstants.MainMenu, true);
        }

        private IEnumerator ShowThankYou() {
            App.Instance.transitionManager.FadeOut();
            yield return new WaitForSeconds(thankYouDelay);
        }
        
    }
}