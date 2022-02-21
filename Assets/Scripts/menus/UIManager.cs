using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace menus
{
    public class UIManager : MonoBehaviour
    {public Camera mainCamera;

             public GameObject titleGroupObject;

             public GameObject playGroupObject;

             public GameObject loadingGroupObject;

             private readonly Vector3 _cauldronTitlePos = new Vector3(0, 111.6f, 80);
             private readonly Vector3 _cauldronPlayPos = new Vector3(0, 120, 100);
             private Quaternion _cauldronPlayRot;
             private Quaternion _cauldronTitleRot;
             private CanvasGroup _titleCanvasGroup;
             private CanvasGroup _playCanvasGroup;
             private CanvasGroup _loadingCanvasGroup;

             private AsyncOperation _sceneLoading;

             private bool _playMenu;
             private float _camProgression = 0;
             private float _titleAlphaProgression = 0;
             private float _playAlphaProgression = 0;
             private float _loadingScreenAlphaProgression = 0;

             public void PlayGame()
             {
                 // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
                 _playMenu = true;
             }

             public void JoinGame()
             {
                 _sceneLoading = SceneManager.LoadSceneAsync("Game");
                 _sceneLoading.allowSceneActivation = false;
             }

             public void ReturnBack()
             {
                 _playMenu = false;
             }

             public void Start()
             {
                 _cauldronPlayRot = Quaternion.Euler(90, 0, 0);
                 _cauldronTitleRot = Quaternion.Euler(15, 0, 0);
                 _playMenu = false;
                 _camProgression = 0;
                 _titleAlphaProgression = 1;
                 _playAlphaProgression = 0;
                 _loadingScreenAlphaProgression = 0;
                 _titleCanvasGroup = titleGroupObject.GetComponent<CanvasGroup>();
                 _playCanvasGroup = playGroupObject.GetComponent<CanvasGroup>();
                 _loadingCanvasGroup = loadingGroupObject.GetComponent<CanvasGroup>();
                 mainCamera.transform.position = _cauldronTitlePos;
             }

             public void Update()
             {
                 if (_playMenu)
                 {
                     _titleCanvasGroup.interactable = false;
                     playGroupObject.SetActive(true);
                     if (_camProgression < 1)
                         _camProgression += 0.01f;
                     else
                     {
                         titleGroupObject.SetActive(false);
                         _playCanvasGroup.interactable = true;
                     }

                     if (_playAlphaProgression < 1)
                         _playAlphaProgression += 0.01f;

                     if (_titleAlphaProgression < 1)
                         _titleAlphaProgression += 0.05f;
                 }
                 else
                 {
                     _playCanvasGroup.interactable = false;
                     if (_camProgression > 0)
                         _camProgression -= 0.01f;
                     else
                     {
                         playGroupObject.SetActive(false);
                         titleGroupObject.SetActive(true);
                         _titleCanvasGroup.interactable = true;
                         if (_titleAlphaProgression > 0)
                             _titleAlphaProgression -= 0.05f;
                     }

                     if (_playAlphaProgression > 0)
                         _playAlphaProgression -= 0.01f;
                 }

                 if (_sceneLoading != null)
                 {
                     loadingGroupObject.SetActive(true);
                     if (_loadingScreenAlphaProgression < 1)
                         _loadingScreenAlphaProgression += 0.1f;

                     loadingGroupObject.GetComponentInChildren<TextMeshProUGUI>().text =
                         $"Chargement...\n{Math.Ceiling(_sceneLoading.progress / 0.9):P}";

                 }
                 else
                 {
                     if (_loadingScreenAlphaProgression > 0)
                         _loadingScreenAlphaProgression -= 0.1f;
                     else
                         loadingGroupObject.SetActive(false);
                 }
             }

             public void LateUpdate()
             {
                 _titleCanvasGroup.alpha = EaseInOutSine(1 - _titleAlphaProgression);
                 _playCanvasGroup.alpha = EaseInOutSine(_playAlphaProgression);
                 _loadingCanvasGroup.alpha = _loadingScreenAlphaProgression;
                 mainCamera.transform.position =
                     Vector3.Slerp(_cauldronTitlePos, _cauldronPlayPos, EaseInOutSine(_camProgression));
                 mainCamera.transform.rotation =
                     Quaternion.Lerp(_cauldronTitleRot, _cauldronPlayRot, EaseInOutSine(_camProgression));
                 if (_sceneLoading != null && _sceneLoading.progress >= 0.9f)
                 {
                     _sceneLoading.allowSceneActivation = true;
                 }
             }

             public void QuitGame()
             {
                 Debug.Log("=> Quit");
                 Application.Quit();
             }

             private float EaseInOutSine(float x) => (float) (-(Math.Cos(Math.PI * x) - 1) / 2);
    }
}