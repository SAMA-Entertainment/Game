using mikunis;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace tools
{
    public class CreateMikuni : MonoBehaviour
    {
        [MenuItem("GameObject/Mikuni/New Hiding Mikuni")]
        static void NewHidingMikuni()
        {
            GameObject mikuni = new GameObject("UnnamedMikuni");
            NavMeshAgent agent = mikuni.AddComponent<NavMeshAgent>();
            Mikuni mikuniController = mikuni.AddComponent<HidingMikuni>();
            mikuniController.agent = agent;
            CreateCommon(mikuni);
        }
        
        
        [MenuItem("GameObject/Mikuni/New Rolling Mikuni")]
        static void NewRollingMikuni()
        {
            GameObject mikuni = new GameObject("UnnamedMikuni");
            NavMeshAgent agent = mikuni.AddComponent<NavMeshAgent>();
            Mikuni mikuniController = mikuni.AddComponent<RollingMikuni>();
            mikuniController.agent = agent;
            CreateCommon(mikuni);
        }

        private static void CreateCommon(GameObject mikuni)
        {
            mikuni.AddComponent<CapsuleCollider>();
            mikuni.AddComponent<Rigidbody>();
            mikuni.AddComponent<PhotonView>();
            PhotonTransformView view = mikuni.AddComponent<PhotonTransformView>();
            view.m_SynchronizeScale = true;
        }
    }
}