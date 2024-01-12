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
        
        [SerializeField] private GameObject thankYouObject;

        private float _endY;
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
            if (creditsParentObject.transform.position.y > _endY) {
                _scrolling = false;
                ShowThankYou();
            }
        }

        // Private functions
        private void SetupCredits() {
            _endY = 0f;
            float currentY = 0f;
            foreach (CreditsSection section in creditsAsset.creditsSections) {
                GameObject sectionObject = Instantiate(sectionTitlePrefab, creditsParentObject.transform);
                sectionObject.GetComponent<TextMeshProUGUI>().SetText(section.title);
                RectTransform sectionTransform = sectionObject.GetComponent<RectTransform>();
                sectionTransform.anchoredPosition = new Vector2(sectionTransform.anchoredPosition.x, currentY);
                foreach (String personName in section.names) {
                    GameObject nameObject = Instantiate(personNamePrefab, creditsParentObject.transform);
                    nameObject.GetComponent<TextMeshProUGUI>().SetText(personName);
                    RectTransform nameTransform = nameObject.GetComponent<RectTransform>();
                    nameTransform.anchoredPosition = new Vector2(nameTransform.anchoredPosition.x, currentY);
                    
                    float nameHeight = nameObject.GetComponent<RectTransform>().rect.height;
                    currentY -= nameHeight;
                    _endY += nameHeight;
                }
                currentY -= creditsAsset.spaceBetweenSections;
                _endY += creditsAsset.spaceBetweenSections;
            }
            Debug.Log(_endY);
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