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
        private bool _freeze = false;

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
            creditsParentObject.transform.position += Vector3.up * (scrollSpeed * Time.unscaledDeltaTime);
            if (creditsParentObject.transform.position.y > _endY) {
                _scrolling = false;
                ShowThankYou();
            }
        }

        // Private functions
        private void SetupCredits() {
            float canvasHeight = creditsParentObject.GetComponent<RectTransform>().rect.height;
            float currentY = 0f;
            float creditsHeight = 0f;
            
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
                    creditsHeight += nameHeight;
                }
                currentY -= creditsAsset.spaceBetweenSections;
                creditsHeight += creditsAsset.spaceBetweenSections;
            }
            
            float startY = -canvasHeight;
            Vector3 position = creditsParentObject.transform.position;
            creditsParentObject.transform.position = new Vector3(position.x, startY, position.z);
            
            _endY = canvasHeight + creditsHeight;
        }
        
        private void OnCancelPressed() {
            if (_freeze) return;
            App.Instance.sceneManager.LoadScene(SceneConstants.MainMenu, true);
            _freeze = true;
        }

        private void ShowThankYou() {
            App.Instance.transitionManager.FadeOut();
            creditsParentObject.SetActive(false);
            thankYouObject.SetActive(true);
        }
    }
}