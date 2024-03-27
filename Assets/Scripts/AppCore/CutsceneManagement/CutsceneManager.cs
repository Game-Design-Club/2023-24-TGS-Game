using System.Collections;

using UnityEngine;

namespace AppCore.CutsceneManagement {
    public class CutsceneManager : MonoBehaviour {
        private Cutscene _currentCutscene;
        
        // Private functions
        private IEnumerator PlayCutscene() {
            foreach (CutsceneChunk currentChunk in _currentCutscene) {
                CutsceneChunkType currentChunkType = currentChunk.chunkType;

                switch (currentChunkType) {
                    case (CutsceneChunkType.Dialogue):
                        break;
                    case (CutsceneChunkType.Animation):
                        break;
                }
            }
            yield return null;
        }
        
        // Public functions
        public void StartCutscene(Cutscene cutscene) {
            if (_currentCutscene != null) {
                Debug.LogWarning(_currentCutscene == cutscene
                    ? "Tried to play cutscene while already playing that cutscene"
                    : "Tried to play a cutscene while playing another cutscene");
            }
            
            _currentCutscene = cutscene;
            StartCoroutine(PlayCutscene());
        }
    }
}