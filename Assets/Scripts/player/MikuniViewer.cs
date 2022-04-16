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

        public void DisplayMikuni(Mikuni mikuni)
        {
            if(mikuni.State == Mikuni.STATE_CAPTURED) return;
            mikuni.SetCaptured(true);
            _mikunis.Add(mikuni);
            Rerender();
        }

        public void HideMikuni(Mikuni mikuni)
        {
            if(mikuni.State != Mikuni.STATE_CAPTURED) return;
            mikuni.transform.parent = null;
            mikuni.SetCaptured(false);
            _mikunis.Remove(mikuni);
            Rerender();
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
                    var tr = mikuni.transform;
                    tr.parent = positions[i];
                    tr.localPosition = Vector3.zero;
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
                    if (mikuni != null)
                    {
                        mikuni.SetCaptured(false);
                    }
                }
            }

            _mikunis.Clear();
        }
    }
}
