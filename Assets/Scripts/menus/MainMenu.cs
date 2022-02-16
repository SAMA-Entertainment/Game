using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace menus
{
    public class MainMenu : MonoBehaviour
    {
        public Camera mainCamera;
        [FormerlySerializedAs("menuContainer")] public GameObject canvasGroupObject;
        public GameObject mainMenu;

        private readonly Vector3 _cauldronTitlePos = new Vector3(0, 111.6f, 80);
        private readonly Vector3 _cauldronPlayPos = new Vector3(0, 120, 100);
        private Quaternion _cauldronPlayRot;
        private Quaternion _cauldronTitleRot;
        private CanvasGroup _canvasGroup;

        private bool _playMenu;
        private float _animProgression = 0;

        public void PlayGame()
        {
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            _playMenu = true;
        }

        public void Start()
        {
            _cauldronPlayRot = Quaternion.Euler(90, 0, 0);
            _cauldronTitleRot = Quaternion.Euler(15, 0, 0);
            _playMenu = false;
            _animProgression = 0;
            _canvasGroup = canvasGroupObject.GetComponent<CanvasGroup>();
            mainCamera.transform.position = _cauldronTitlePos;
        }

        public void Update()
        {
            if (_playMenu)
            {
                if (_animProgression < 1)
                    _animProgression += 0.01f;
                else
                    canvasGroupObject.SetActive(false);
            }
            else
            {
                if (_animProgression > 0)
                    _animProgression -= 0.01f;
                mainMenu.SetActive(true);
            }

            _canvasGroup.alpha = EaseInOutSine(1-_animProgression);
            mainCamera.transform.position =
                Vector3.Slerp(_cauldronTitlePos, _cauldronPlayPos, EaseInOutSine(_animProgression));
            mainCamera.transform.rotation =
                Quaternion.Lerp(_cauldronTitleRot, _cauldronPlayRot, EaseInOutSine(_animProgression));
        }

        public void QuitGame()
        {
            Debug.Log("=> Quit");
            Application.Quit();
        }

        private float EaseInOutSine(float x) => (float) (-(Math.Cos(Math.PI * x) - 1) / 2);
    }
}