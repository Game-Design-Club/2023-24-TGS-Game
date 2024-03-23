using System;
using System.Collections;
using System.Collections.Generic;

using AppCore;

using Tools.Constants;

using UnityEngine;

namespace Main_Menu {
    public class MainMenuManager : MonoBehaviour {
        private bool _freeze = false;

        private void Start() {
            StartCoroutine(PlayMusic());
        }

        private IEnumerator PlayMusic() {
            yield return null;
            App.Instance.audioManager.musicPlayer.PlayMainMenuMusic();
        }
        
        public void StartGame() {
            if (_freeze) {
                return;
            }
            _freeze = true;
            App.Instance.sceneManager.LoadScene(SceneConstants.Game);
        }

        public void ShowCredits() {
            if (!_freeze) {
                _freeze = true;
                App.Instance.sceneManager.LoadScene(SceneConstants.Credits);
            }
        }

        public void QuitGame() {
            if (_freeze) {
                return;
            }
            _freeze = true;
            if (Application.isEditor) {
                UnityEditor.EditorApplication.isPlaying = false;
                return;
            }
            Application.Quit();
        }
    }
}