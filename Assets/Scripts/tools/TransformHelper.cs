using UnityEngine;

namespace tools
{
    public class TransformHelper
    {
        public static GameObject FindComponentInChildWithTag(GameObject parent, string tag) {
            Transform t = parent.transform;
            foreach(Transform tr in t)
            {
                if(tr.CompareTag(tag))
                {
                    return tr.gameObject;
                }
            }
            return null;
        } 
    }
}