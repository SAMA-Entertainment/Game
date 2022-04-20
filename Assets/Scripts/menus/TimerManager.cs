using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace menus
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager TimerInstance;
        public const int STATE_UNSTARTED = 0;
        public const int STATE_TIMER_COUNT_DOWN = 1;
        public const int STATE_TIMER_COUNT_UP = 2;
        public const int STATE_PAUSED = 3;
        
        private PhotonView _view;
        private int _timerState = STATE_UNSTARTED;
        private long _timestamp;

        public GameObject timerText;
        private TextMeshProUGUI _ugui;

        private void Start()
        {
            TimerInstance = this;
            _view = GetComponent<PhotonView>();
            _ugui = timerText.GetComponent<TextMeshProUGUI>();
            if (_ugui == null) throw new Exception("Could not find TextMeshPro component on timerText");
            //StartCountDown(DateTime.Now + TimeSpan.FromMinutes(5));
            StartStopwatch();
        }

        void LateUpdate()
        {
            if (_timerState == STATE_UNSTARTED)
            {
                _ugui.text = "";
                return;
            }

            TimeSpan elapsed;
            if (_timerState == STATE_TIMER_COUNT_UP)
            {
                elapsed = DateTime.Now - new DateTime(_timestamp * TimeSpan.TicksPerMillisecond);
            } else if (_timerState == STATE_TIMER_COUNT_DOWN)
            {
                elapsed = new DateTime(_timestamp * TimeSpan.TicksPerMillisecond) - DateTime.Now;
            }

            _ugui.text = elapsed.Minutes.ToString().PadLeft(2, '0') + ":" + 
                                         elapsed.Seconds.ToString().PadLeft(2, '0');
        }

        public void StartStopwatch()
        {
            if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) return;
            long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            _view.RPC("RPC_BroadcastTimerUpdate", RpcTarget.AllBuffered, 
                STATE_TIMER_COUNT_UP, startTime);
        }

        public void StartCountDown(DateTime endDate)
        {
            if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) return;
            long endTime = endDate.Ticks / TimeSpan.TicksPerMillisecond;
            _view.RPC("RPC_BroadcastTimerUpdate", RpcTarget.AllBuffered,
                STATE_TIMER_COUNT_DOWN, endTime);
        }

        public void PauseTimer()
        {
            if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) return;
            _view.RPC("RPC_BroadcastTimerUpdate", RpcTarget.AllBuffered,
                STATE_PAUSED, _timestamp);
        }

        public void Reset()
        {
            if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) return;
            _view.RPC("RPC_BroadcastTimerUpdate", RpcTarget.AllBuffered,
                STATE_UNSTARTED, -1);
        }

        [PunRPC]
        public void RPC_BroadcastTimerUpdate(int timerState, long timestamp)
        {
            this._timerState = timerState;
            this._timestamp = timestamp;
            Debug.Log("Timer Update => " + timerState + " | " + timestamp);
        }
    }
}