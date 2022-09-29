using UnityEngine;

namespace PlayerNotifications
{
    /// Attach this script to ny object to call a player notification to the attached controller on wake.
    public class MessageOnCollide : MonoBehaviour
    {
        public NotificationController notificationController;

        public string messageToPlay = "default";

        [Tooltip("The instance of a game object to check collision against.")]
        public GameObject collisionObject;

        [Tooltip("Priority level of the message - 1 is highest, increasing is lower priority.")]
        public int priority = 1;

        public float timeToDisplayMessage = 0f;
        public bool canMessageBeReplayed = false;

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject == collisionObject)
            {
                if (notificationController != default)
                {
                    if (timeToDisplayMessage == 0f)
                    {
                        notificationController.DisplayNotificationMessage(messageToPlay, priority,
                            canMessageBeReplayed);
                    }
                    else
                    {
                        notificationController.DisplayNotificationMessage(messageToPlay, priority, canMessageBeReplayed,
                            timeToDisplayMessage);
                    }
                }
                else
                {
                    Debug.Log("No notification controller set on " + name);
                }
            }
        }
    }
}