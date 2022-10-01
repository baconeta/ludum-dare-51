using UnityEngine;
using Controllers;

namespace ModeSwap
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSwapper : MonoBehaviour
    {
        private SpriteRenderer spriteRender;
        [SerializeField] private Sprite lightSprite;
        [SerializeField] private Sprite darkSprite;
        private GameController gameController;
        private void Awake()
        {
            gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.Timer.OnPhaseChange.AddListener(SetPhaseMode);
                gameController.Timer.OnTimerStart.AddListener(SetPhaseMode);
            }

            spriteRender = GetComponent<SpriteRenderer>();
        }

        private void SetPhaseMode(EWorldPhase worldPhase)
        {
            switch (worldPhase)
            {
                case EWorldPhase.LIGHT:
                    if (lightSprite != null)
                        spriteRender.sprite = lightSprite;
                    break;
                case EWorldPhase.DARK:
                    if (darkSprite != null)
                        spriteRender.sprite = darkSprite;
                    break;
            }
        }
    }
}