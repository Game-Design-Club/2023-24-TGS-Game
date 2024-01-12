using System;

using AppCore;

using Constants;

using TMPro;

using UnityEngine;

namespace Credits {
    public class CreditsScroller : MonoBehaviour {
        [SerializeField] private CreditsAsset creditsAsset;
        [SerializeField] private GameObject sectionTitlePrefab;
        [SerializeField] private GameObject personNamePrefab;
        [SerializeField] private GameObject creditsParentObject;
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

        private void Start() {
            thankYouObject.SetActive(false);
            creditsParentObject.SetActive(true);
            SetupCredits();
        }

        private void Update() {
            if (!_scrolling) return;
            creditsParentObject.transform.position += Vector3.up * (scrollSpeed * Time.deltaTime);
            if (creditsParentObject.transform.position.y < endY) {
                _scrolling = false;
                ShowThankYou();
                Debug.Log("Scroll done");
            }
        }

        // Private functions
        private void SetupCredits() {
            float currentY = 0;
            foreach (CreditsSection section in creditsAsset.creditsSections) {
                GameObject sectionObject = Instantiate(sectionTitlePrefab, creditsParentObject.transform);
                Debug.Log(section.title);
                sectionObject.GetComponent<TextMeshProUGUI>().SetText(section.title);
                // foreach (String personName in section.names) {
                //     GameObject nameObject = Instantiate(personNamePrefab, creditsParentObject.transform);
                //     nameObject.GetComponent<TextMeshProUGUI>().SetText(personName);
                // }
                currentY -= creditsAsset.spaceBetweenSections;
            }
        }
        
        private void OnCancelPressed() {
            App.Instance.sceneManager.LoadScene(SceneConstants.MainMenu, true);
        }

        private void ShowThankYou() {
            App.Instance.transitionManager.FadeOut();
            creditsParentObject.SetActive(false);
            thankYouObject.SetActive(true);
        }
    }
}