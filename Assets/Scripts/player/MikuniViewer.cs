using System;
using System.Collections.Generic;
using mikunis;
using UnityEngine;

namespace player
{
    public class MikuniViewer : MonoBehaviour
    {
        public List<Transform> positions;
        private readonly List<Mikuni> _mikunis = new List<Mikuni>();

        public void DisplayMikuni(Mikuni mikuni, bool remote)
        {
            if (!remote)
            {
                mikuni.SetCaptured(true);
            }
            _mikunis.Add(mikuni);
            Rerender();
        }

        public void ReleaseMikuni(Mikuni mikuni, bool remote)
        {
            if(mikuni.State != Mikuni.STATE_CAPTURED) return;
            mikuni.transform.parent = null;
            mikuni.gameObject.SetActive(true);
            _mikunis.Remove(mikuni);
            Rerender();
            if (!remote)
            {
                mikuni.transform.localScale = Vector3.one;
                mikuni.SetCaptured(false);
            }
        }

        public void Rerender()
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (i >= _mikunis.Count)
                {
                    for (int j = 0; j < positions[i].childCount; j++)
                    {
                        positions[i].GetChild(j).parent = null;
                    }
                }
                else
                {
                    Mikuni mikuni = _mikunis[i];
                    mikuni.gameObject.SetActive(true);
                    var tr = mikuni.transform;
                    tr.parent = positions[i];
                    tr.localScale = Vector3.one * 0.005f;
                    tr.localPosition = Vector3.zero;
                }
            }

            if (_mikunis.Count > positions.Count)
            {
                for (int i = positions.Count; i < _mikunis.Count; i++)
                {
                    Mikuni mikuni = _mikunis[i];
                    mikuni.gameObject.SetActive(false);
                }
            }
        }

        public void Clear(Transform newParent)
        {
            foreach (Transform tr in positions)
            {
                for (int j = 0; j < tr.childCount; j++)
                {
                    Transform child = tr.GetChild(j);
                    child.parent = newParent;
                    Mikuni mikuni = child.GetComponent<Mikuni>();
                    mikuni.gameObject.SetActive(true);
                    if (mikuni != null)
                    {
                        mikuni.SetCaptured(false);
                    }
                }
            }

            _mikunis.Clear();
        }

        public void Destroy()
        {
            _mikunis.Clear();
        }
    }
}
