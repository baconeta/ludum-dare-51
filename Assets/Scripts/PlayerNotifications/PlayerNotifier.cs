using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerNotifications
{
    public class PlayerNotifier : MonoBehaviour
    {
        //Components and base setup
        private Text _notificationTextBase;
        private Canvas _displayCanvas;
        private RectTransform _rectTransform;
        private float _defaultAlphaValue = 1f;

        private void Start()
        {
            ComponentSetup();
            SetupCanvas();
            SetTextDefaults();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void ComponentSetup()
        {
            // Ensure component is set up to display text on the screen
            _notificationTextBase = gameObject.AddComponent<Text>();
            _displayCanvas = gameObject.AddComponent<Canvas>();
            _rectTransform = gameObject.GetComponent<RectTransform>();
        }

        private void SetupCanvas()
        {
            // Should this be enabled to be set manually as well? TODO
            _displayCanvas.worldCamera = FindObjectOfType<Camera>();

            _displayCanvas.renderMode = RenderMode.WorldSpace;
            _displayCanvas.sortingOrder = 5; // UI mode
        }

        private void SetTextDefaults()
        {
            _notificationTextBase.alignment = TextAnchor.MiddleCenter;
            _notificationTextBase.font = Font.CreateDynamicFontFromOSFont("LiberationSans", 14); // TODO generify
            _notificationTextBase.fontSize = 28;
        }

        /// <summary>
        /// Set the size of the font in px.
        /// </summary>
        /// <param name="size">Size of the font in pixels</param>
        public void SetFontSize(int size)
        {
            _notificationTextBase.fontSize = size;
        }

        /// <summary>
        /// Set a font family with default px size.
        /// </summary>
        /// <param name="font">Font to set</param>
        public void SetFontFamily(Font font)
        {
            try
            {
                _notificationTextBase.font = font;
            }
            catch (Exception e)
            {
                Debug.Log("Font not valid or error setting font " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// Sets the color and/or alpha values for the font.
        /// </summary>
        /// <param name="color"></param>
        public void SetFontColor(Color color)
        {
            try
            {
                _defaultAlphaValue = color.a;
                _notificationTextBase.color = color;
            }
            catch (Exception e)
            {
                Debug.Log("Error setting color or alpha value." + e.Message);
                throw;
            }
        }

        /// <summary>
        /// Set and display a message on the Text object of this object.
        /// Call SetNotifierLocation() to set its world location.
        /// </summary>
        /// <param name="m">Message to display on-screen</param>
        public void DisplayNotificationMessage(string m)
        {
            SetMessageAlpha(_defaultAlphaValue);
            _notificationTextBase.text = m;
        }

        // This function is used to set the initial location of the notification message and
        // can be called on update functions to pin it to a target.
        public void SetNotifierLocation(float x, float y)
        {
            Vector3 transformPosition = _rectTransform.position;
            transformPosition.x = x;
            transformPosition.y = y;
            _rectTransform.position = transformPosition;
        }

        public void ClearMessage(bool fadeMessageOut, float fadeMessageTime = 0.0f)
        {
            if (fadeMessageOut)
            {
                StartCoroutine(FadeOutMessage(fadeMessageTime));
            }
            else
            {
                _notificationTextBase.text = "";
            }
        }

        private void SetMessageAlpha(float alpha)
        {
            Color color = _notificationTextBase.color;
            color.a = alpha;
            _notificationTextBase.color = color;
        }

        private IEnumerator FadeOutMessage(float duration)
        {
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration;
                SetMessageAlpha(Mathf.Lerp(1, 0, normalizedTime));

                yield return null;
            }

            _notificationTextBase.text = "";
        }

        public void CancelFade()
        {
            StopAllCoroutines();
        }
    }
}