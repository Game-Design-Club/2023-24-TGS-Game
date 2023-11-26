using System;
using System.Collections;
using System.Collections.Generic;

using Cinemachine;

using Game.GameManagement;
using Game.Player;

using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour {
    private CinemachineVirtualCamera _virtualCamera;
    
    private void Awake() {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable() {
        GameManager.Instance.OnLevelStart += OnLevelStart;
        GameManager.Instance.OnLevelOver += OnLevelOver;
    }
    
    private void OnDisable() {
        GameManager.Instance.OnLevelStart -= OnLevelStart;
        GameManager.Instance.OnLevelOver -= OnLevelOver;
    }
    
    private void OnLevelStart() {
        _virtualCamera.Follow = Player.Instance.transform;
    }
    
    private void OnLevelOver() {
        _virtualCamera.Follow = null;
    }
}
