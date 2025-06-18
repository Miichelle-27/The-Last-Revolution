using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] public int levelIndex; // Índice del nivel (-1 para lobby)
    private GameObject[] levels;
    private CinemachineVirtualCamera[] vcams;

    private void Awake()
    {
        levels = new[] { GameObject.Find("Lobby"), GameObject.Find("Level1"), GameObject.Find("Level2"), 
                        GameObject.Find("Level3"), GameObject.Find("Level4"), GameObject.Find("Level5") };
        vcams = FindObjectsOfType<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && (levelIndex == -1 || GameManager.Instance.IsLevelUnlocked(levelIndex)))
        {
            GameManager.Instance.SetCurrentLevel(levelIndex);
            
            switch (levelIndex)
            {
                case -1: // Lobby
                    other.transform.position = new Vector3(0f, 0f, 0f);
                    levels[0]?.SetActive(true);
                    for (int i = 1; i < levels.Length; i++) levels[i]?.SetActive(false);
                    SetActiveCamera("VCam_Lobby");
                    break;
                case 0: // Nivel 1
                    other.transform.position = new Vector3(20f, 0f, 0f);
                    levels[1]?.SetActive(true);
                    levels[0]?.SetActive(false);
                    for (int i = 2; i < levels.Length; i++) levels[i]?.SetActive(false);
                    SetActiveCamera("VCam_Level1");
                    break;
                case 1: // Nivel 2
                    other.transform.position = new Vector3(40f, 0f, 0f);
                    levels[2]?.SetActive(true);
                    for (int i = 0; i < levels.Length; i++) if (i != 2) levels[i]?.SetActive(false);
                    SetActiveCamera("VCam_Level2");
                    break;
                case 2: // Nivel 3
                    other.transform.position = new Vector3(60f, 0f, 0f);
                    levels[3]?.SetActive(true);
                    for (int i = 0; i < levels.Length; i++) if (i != 3) levels[i]?.SetActive(false);
                    SetActiveCamera("VCam_Level3");
                    break;
                case 3: // Nivel 4
                    other.transform.position = new Vector3(80f, 0f, 0f);
                    levels[4]?.SetActive(true);
                    for (int i = 0; i < levels.Length; i++) if (i != 4) levels[i]?.SetActive(false);
                    SetActiveCamera("VCam_Level4");
                    break;
                case 4: // Nivel 5
                    other.transform.position = new Vector3(100f, 0f, 0f);
                    levels[5]?.SetActive(true);
                    for (int i = 0; i < 5; i++) levels[i]?.SetActive(false);
                    SetActiveCamera("VCam_Level5");
                    break;
            }

            // Asegura que la cámara siga al jugador
            CinemachineVirtualCamera activeCam = Array.Find(vcams, v => v.name == GetCameraName(levelIndex));
            if (activeCam != null) activeCam.Follow = other.transform;
        }
    }

    private string GetCameraName(int index)
    {
        return index == -1 ? "VCam_Lobby" : $"VCam_Level{index}";
    }

    private void SetActiveCamera(string cameraName)
    {
        foreach (var vcam in vcams)
        {
            vcam.Priority = (vcam.name == cameraName) ? 10 : 0;
        }
    }
}
