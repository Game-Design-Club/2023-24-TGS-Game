
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace AppCore.CutsceneManagement {
    public class Cutscene : MonoBehaviour, IEnumerable<CutsceneChunk> {
        [SerializeField] private CutsceneChunk[] _cutsceneChunks;
        private int _currentChunkIndex;


        public IEnumerator<CutsceneChunk> GetEnumerator() {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}