using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace menus
{
    public class MainMenu : MonoBehaviour
    {
        public Camera mainCamera;

        [FormerlySerializedAs("menuContainer")]
        public GameObject canvasGroupObject;

        public GameObject playGroupObject;

        public GameObject mainMenu;

        private readonly Vector3 _cauldronTitlePos = new Vector3(0, 111.6f, 80);
        private readonly Vector3 _cauldronPlayPos = new Vector3(0, 120, 100);
        private Quaternion _cauldronPlayRot;
        private Quaternion _cauldronTitleRot;
        private CanvasGroup _canvasGroup;

        private bool _playMenu;
        private float _camProgression = 0;
        private float _alphaProgression = 0;

        public void PlayGame()
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            _playMenu = true;
            playGroupObject.SetActive(true);
        }

        public void Start()
        {
            _cauldronPlayRot = Quaternion.Euler(90, 0, 0);
            _cauldronTitleRot = Quaternion.Euler(15, 0, 0);
            _playMenu = false;
            _camProgression = 0;
            _alphaProgression = 1;
            _canvasGroup = canvasGroupObject.GetComponent<CanvasGroup>();
            mainCamera.transform.position = _cauldronTitlePos;
        }

        public void Update()
        {
            if (_playMenu)
            {
                if (_camProgression < 1)
                    _camProgression += 0.01f;
                else
                {
                    _playMenu = false;
                    canvasGroupObject.SetActive(false);
                }

                if (_alphaProgression < 1)
                    _alphaProgression += 0.05f;
            }
            else
            {
                if (_camProgression > 0)
                    _camProgression -= 0.01f;
                else
                {
                    mainMenu.SetActive(true);
                    canvasGroupObject.SetActive(true);
                }

                if (_alphaProgression > 0)
                    _alphaProgression -= 0.05f;
            }

            Debug.Log(_alphaProgression);

            _canvasGroup.alpha = EaseInOutSine(1 - _alphaProgression);
            mainCamera.transform.position =
                Vector3.Slerp(_cauldronTitlePos, _cauldronPlayPos, EaseInOutSine(_camProgression));
            mainCamera.transform.rotation =
                Quaternion.Lerp(_cauldronTitleRot, _cauldronPlayRot, EaseInOutSine(_camProgression));
        }

        public void QuitGame()
        {
            Debug.Log("=> Quit");
            Application.Quit();
        }

        public void ResetMenu()
        {
            _playMenu = false;
        }

        private float EaseInOutSine(float x) => (float) (-(Math.Cos(Math.PI * x) - 1) / 2);
    }
}