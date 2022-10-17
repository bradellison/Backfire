using TMPro;
using UnityEngine;

namespace CanvasScripts
{
    public class DebugCanvas : MonoBehaviour
    {

        private float _elapsedTime;
        public TMP_Text elapsedTimeText;
        public TMP_Text memoryUsedText;
        private float _fps;
        public TMP_Text fpsText;

        private void Start()
        {
            _fps = 30f;
        }

        private void DisplayFPS() {
            _fps = 1.0f / Time.smoothDeltaTime;
            fpsText.text = ("FPS: " + ((int)_fps).ToString());
        }

        private void Update()
        {
            _elapsedTime += Time.deltaTime;
            elapsedTimeText.text = ("Time: " + _elapsedTime.ToString("F2"));
            memoryUsedText.text = ("Mem: " + (System.GC.GetTotalMemory(false) / 1000000).ToString("F0") + "MB");
            DisplayFPS();
        }
    }
}
