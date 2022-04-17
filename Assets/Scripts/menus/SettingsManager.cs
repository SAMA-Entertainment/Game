using System;
using TMPro;
using UnityEngine;

namespace menus
{
    public class SettingsManager : MonoBehaviour
    {
        private Resolution[] _resolutions;

        public GameObject resolutionDisplay;

        private int _resolutionIndex;
        private TextMeshProUGUI _textDisplay;

        // Start is called before the first frame update
        void Start()
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = 75; // TODO
            _resolutions = Screen.resolutions;
            /*foreach (Resolution resolution in _resolutions)
            {
                Debug.Log(resolution);
            }*/
            _textDisplay = resolutionDisplay.GetComponent<TextMeshProUGUI>();
            Resolution prefResolution = GetPreferredResolutionOrDefault();
            _resolutionIndex = BinarySearchIndexOf(prefResolution, _resolutions);
            _textDisplay.text = prefResolution.ToString();
            Screen.SetResolution(prefResolution.width, prefResolution.height, FullScreenMode.ExclusiveFullScreen);
        }

        public void SetNextResolution()
        {
            Debug.Log("Next resolution called");
            _resolutionIndex++;
            if (_resolutionIndex >= _resolutions.Length)
                _resolutionIndex = 0;
            Resolution fetchedRes = _resolutions[_resolutionIndex];
            _textDisplay.text = fetchedRes.ToString();
            Debug.Log(Screen.currentResolution.ToString());
        }

        public void SetPrevResolution()
        {
            Debug.Log("Prev resolution called");
            _resolutionIndex--;
            if (_resolutionIndex < 0)
                _resolutionIndex = _resolutions.Length - 1;
            Resolution fetchedRes = _resolutions[_resolutionIndex];
            _textDisplay.text = fetchedRes.ToString();
            Debug.Log(Screen.currentResolution.ToString());
        }

        public void ApplyResolution()
        {
            Resolution fetchedRes = _resolutions[_resolutionIndex];
            Screen.SetResolution(fetchedRes.width, fetchedRes.height, FullScreenMode.ExclusiveFullScreen);
        }

        private bool resGT(Resolution lh, Resolution rh)
        {
            return lh.width > rh.width || lh.height > rh.height;
        }

        private bool resGEqT(Resolution lh, Resolution rh)
        {
            return lh.width >= rh.width || lh.height >= rh.height;
        }

        private bool resLT(Resolution lh, Resolution rh)
        {
            return lh.width < rh.width || lh.height < rh.height;
        }

        private bool resLEqT(Resolution lh, Resolution rh)
        {
            return lh.width <= rh.width || lh.height <= rh.height;
        }

        private bool resEq(Resolution lh, Resolution rh)
        {
            return lh.width == rh.width && lh.height == rh.height;
        }

        private int BinarySearchIndexOf(Resolution res, Resolution[] array)
        {
            int l = 0;
            int r = array.Length - 1;
            int m;

            do
            {
                m = (l + r) / 2;
                if (resEq(res, array[m]))
                {
                    break;
                }

                if (resLT(res, array[m]))
                {
                    r = m - 1;
                }
                else
                {
                    l = m + 1;
                }
            } while (l <= r);

            return m;
        }

        private Resolution GetPreferredResolutionOrDefault()
        {
            string prefRes = PlayerPrefs.GetString("GameResolution", "");

            Resolution resolution;

            if (prefRes != "")
            {
                string[] res = prefRes.Split('x');

                resolution = new Resolution
                {
                    width = Int32.Parse(res[0]),
                    height = Int32.Parse(res[1])
                };
            }
            else
            {
                resolution = Screen.currentResolution;
            }

            return resolution;
        }

        private void SetPreferredResolution(Resolution res)
        {
            PlayerPrefs.SetString("GameResolution", res.ToString());
        }
    }
}