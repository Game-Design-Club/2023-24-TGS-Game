using System;

using AppCore;

using TMPro;

using Tools.Constants;

using UnityEngine;

namespace Credits {
    public class CreditsScroller : MonoBehaviour {
        // Manages the creation and scrolling of the credits, based on the CreditsAsset
        // Automatically scrolls the credits up the screen, and displays a thank you message when the credits are done
        
        [SerializeField] private CreditsAsset creditsAsset;
        [SerializeField] private GameObject sectionTitlePrefab;
        [SerializeField] private GameObject personNamePrefab;
        [SerializeField] private GameObject creditsParentObject;
        [SerializeField] private float scrollSpeed = 1f;
        [SerializeField] private int characterCountWrap = 15;
        
        [SerializeField] private GameObject thankYouObject;

        private float _endY;
        private bool _scrolling = true;
        private bool _freeze = false;

        // Unity functions
        private void OnEnable() {
            App.Instance.inputManager.OnCancel += OnCancelPressed;
        }

        private void OnDisable() {
            App.Instance.inputManager.OnCancel -= OnCancelPressed;
        }

        private void Start() {
            thankYouObject.SetActive(false);
            creditsParentObject.SetActive(true);
            SetupCredits();
        }

        private void Update() {
            if (!_scrolling) return;
            creditsParentObject.transform.position += Vector3.up * (scrollSpeed * Time.unscaledDeltaTime);
            if (creditsParentObject.transform.localPosition.y > _endY) {
                _scrolling = false;
                ShowThankYou();
            }
        }

        // Private functions
        private void SetupCredits() {
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
                if (section.names.Length <= 1 && section.title.Length > characterCountWrap) {
                    currentY -= creditsAsset.spaceBetweenSections;
                    creditsHeight += creditsAsset.spaceBetweenSections;
                }
                currentY -= creditsAsset.spaceBetweenSections;
                creditsHeight += creditsAsset.spaceBetweenSections;
            }
            
            _endY = creditsHeight;
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