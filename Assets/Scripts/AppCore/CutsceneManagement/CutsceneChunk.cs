using UnityEngine;

namespace AppCore.CutsceneManagement {
    [System.Serializable]
    public class CutsceneChunk {
        [SerializeField] internal CutsceneChunkType chunkType;
        [SerializeField] internal CutsceneChunkContinueType continueAction;
    }
}