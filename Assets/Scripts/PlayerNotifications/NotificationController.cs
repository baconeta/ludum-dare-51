using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerNotifications
{
    /// <summary>
    /// This object will handle all functionality involving controlling the text on the screen.
    /// It managers timers related to priority and fade outs on a message.
    /// It should be attached to objects where you want the message to follow a player, and you must also
    /// enable doesMessageFollowController from the editor.
    /// Remember to place a Notification controller in the world where this object is first going to appear if you have
    /// persistBetweenScenes enabled, otherwise place it in every scene where you will use it.
    /// </summary>
    public class NotificationController : MonoBehaviour
    {
        [Tooltip("Put the PlayerNotifier object into the world and reference it here.")]
        public PlayerNotifier playerNotificationObject;

        [Tooltip("Set true if you want this controller to track a gameObject." +
                 "Make sure you set a tagToTrack for this object for scene persist functionality")]
        public bool doesTrackAnObjectOrPlayer;

        private GameObject _objectToFollow;

        [Tooltip("Optional object to track position of, i.e. a character, for message displaying")]
        public string tagToTrack;

        [Tooltip("Offset for messages location from tracked player. Only used if doesMessageFollowController.")]
        public Vector3 messageOffsetLocation;

        [Tooltip(
            "Set whether the message will follow the controller on message shown or controller object moved." +
            "Otherwise, the message will be displayed at the PlayerNotifier world location.")]
        public bool doesMessageFollowController;

        [Tooltip("Enable to save non-repeating messages between levels.")]
        public bool persistBetweenScenes;

        [Tooltip("Enable to have all future messages fade out. Callable from code if you want to change on the fly.")]
        public bool doMessagesFadeOut;

        [Tooltip("Used in conjunction with doMessagesFadeOut, sets the time a message takes to fade to nothing.")]
        public float timeToFadeOutInSeconds;

        [Tooltip("Whether or not an un-played message (due to low priority) is counted as played.")]
        public bool notDisplayedMessagesCountAsPlayed;

        [Tooltip("Default Font Family")] public Font defaultFontForMessages;
        [Tooltip("Default Font Color/Style")] public Color defaultMessageColor;
        [Tooltip("Default Font Size")] public int defaultFontSize;

        private bool _isMessageOnScreen;

        [Tooltip("List of all played messages that should not be played again.")]
        private readonly List<string> _alreadyPlayedMessages = new List<string>();

        [Tooltip("The priority of the message currently being played. 0 if nothing is being played.")]
        private int _currentMessagePriority;

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Call this function (Broadcast or otherwise) to display a message in the world.
        /// </summary>
        /// <param name="m">Message to display</param>
        /// <param name="timeToDisplay">How long should the message display for (seconds).</param>
        /// <param name="messagePriority">The priority level of this message. 1 is highest.</param>
        /// <param name="canMessageBeReplayed">True if this message should only play once ever</param>
        public void DisplayNotificationMessage(string m, int messagePriority, bool canMessageBeReplayed,
            float timeToDisplay = 0.0f)
        {
            if (!playerNotificationObject)
            {
                Debug.Log("No PlayerNotification Object was attached to the controller to handle messages.");
                return;
            }

            if (!canMessageBeReplayed)
            {
                if (HasMessageBeenPlayed(m))
                {
                    return;
                }
            }

            if (!IsMessageHigherPriorityThanCurrent(messagePriority))
            {
                if (notDisplayedMessagesCountAsPlayed)
                {
                    _alreadyPlayedMessages.Add(m);
                }

                return;
            }

            if (_isMessageOnScreen)
            {
                CancelInvoke();
                StopAllCoroutines();
                playerNotificationObject.CancelFade();
                playerNotificationObject.ClearMessage(false);
            }

            if (timeToDisplay == 0.0f)
            {
                timeToDisplay = CalculateTimeToDisplay(m);
            }

            _isMessageOnScreen = true;
            _currentMessagePriority = messagePriority;
            playerNotificationObject.DisplayNotificationMessage(m);
            _alreadyPlayedMessages.Add(m);

            Invoke(nameof(ClearMessageFromScreen), timeToDisplay);
        }

        private float CalculateTimeToDisplay(string s)
        {
            return s.Length / 15.0f + 2.0f;
        }

        private void Update()
        {
            if (!_isMessageOnScreen) return;
            if (!doesMessageFollowController) return;

            Transform currentTransform = transform;

            if (doesTrackAnObjectOrPlayer && _objectToFollow)
            {
                if (_objectToFollow)
                {
                    MoveControllerToObject(currentTransform);
                }
                else
                {
                    // For example, when a new scene loads or an object ref is destroyed
                    _objectToFollow = GameObject.FindGameObjectWithTag(tagToTrack);
                }
            }

            currentTransform.position += messageOffsetLocation;
            playerNotificationObject.SetNotifierLocation(currentTransform.position.x, currentTransform.position.y);
        }

        private void MoveControllerToObject(Transform currentTransform)
        {
            Vector3 transformPosition = currentTransform.position;
            transformPosition.x = _objectToFollow.transform.position.x;
            transformPosition.y = _objectToFollow.transform.position.y;
            transform.position = transformPosition;
        }

        private void Awake()
        {
            if (persistBetweenScenes)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            DefaultFontValues();

            _objectToFollow = GameObject.FindGameObjectWithTag(tagToTrack);
        }

        private void DefaultFontValues()
        {
            if (defaultFontSize != default)
            {
                playerNotificationObject.SetFontSize(defaultFontSize);
            }

            if (defaultMessageColor != default)
            {
                playerNotificationObject.SetFontColor(defaultMessageColor);
            }

            if (defaultFontForMessages != default)
            {
                playerNotificationObject.SetFontFamily(defaultFontForMessages);
            }
        }

        private void ClearMessageFromScreen()
        {
            playerNotificationObject.ClearMessage(doMessagesFadeOut, timeToFadeOutInSeconds);
            if (doMessagesFadeOut)
            {
                StartCoroutine(DelayFade(timeToFadeOutInSeconds));
                return;
            }

            _currentMessagePriority = 0;
            _isMessageOnScreen = false;
        }

        private IEnumerator DelayFade(float delayTime)
        {
            //Wait for the specified delay time before continuing.
            yield return new WaitForSeconds(delayTime);
            _currentMessagePriority = 0;
            _isMessageOnScreen = false;
        }

        /// <summary>
        /// Determines whether or not a message can be played given its priority
        /// level (1 is highest) compared to any already playing messages.
        /// </summary>
        /// <param name="messagePriority">The priority of the message to be checked.</param>
        /// <returns></returns>
        private bool IsMessageHigherPriorityThanCurrent(int messagePriority)
        {
            if (_currentMessagePriority < 0)
            {
                Debug.Log("Message priority should be a positive integer.");
                return false;
            }

            if (_currentMessagePriority == 0)
            {
                // No message is being played
                return true;
            }

            return messagePriority <= _currentMessagePriority;
        }

        /// <summary>
        /// Removes a single message from the played list so it could be played again.
        /// </summary>
        /// <param name="m">The message to be removed from the list.</param>
        public void RemovePlayedMessage(string m)
        {
            _alreadyPlayedMessages.Remove(m);
        }

        /// <summary>
        /// Returns true if the given message has been played before (only for non-repeatable messages).
        /// </summary>
        /// <param name="m">The message to check.</param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public bool HasMessageBeenPlayed(string m)
        {
            return _alreadyPlayedMessages.Contains(m);
        }

        /// <summary>
        /// Removes all played messages so they can all be played again (non-repeatable messages only)
        /// </summary>
        public void RemoveAllPlayedMessages()
        {
            _alreadyPlayedMessages.Clear();
        }
    }
}