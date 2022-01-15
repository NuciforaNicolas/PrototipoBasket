using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallIndicator : MonoBehaviour
{
    [SerializeField] RectTransform ballIndicator;
    [SerializeField] Transform target;
    [SerializeField] float screenBorderOffset;
    Image ballIndicatorSprite;
    bool isStartGame;

    private void Awake()
    {
        ballIndicatorSprite = ballIndicator.GetComponent<Image>();
        ballIndicatorSprite.enabled = false;
        isStartGame = true;
    }

    private void Update()
    {
        if (!GameManager.instance.CanPlay() && isStartGame) return;
        isStartGame = false;

        var targetPosScreenPoint = Camera.main.WorldToScreenPoint(target.position);
        if (targetPosScreenPoint.x <= screenBorderOffset || targetPosScreenPoint.x >= Screen.width - screenBorderOffset ||
            targetPosScreenPoint.y <= screenBorderOffset || targetPosScreenPoint.y >= Screen.height - screenBorderOffset)
        {
            ballIndicatorSprite.enabled = true;
            var targetInsideScreenPos = targetPosScreenPoint;
            if (targetInsideScreenPos.x <= screenBorderOffset) targetInsideScreenPos.x = screenBorderOffset;
            if (targetInsideScreenPos.x >= Screen.width - screenBorderOffset) targetInsideScreenPos.x = Screen.width - screenBorderOffset;
            if (targetInsideScreenPos.y <= screenBorderOffset) targetInsideScreenPos.y = screenBorderOffset;
            if (targetInsideScreenPos.y >= Screen.height - screenBorderOffset) targetInsideScreenPos.y = Screen.height - screenBorderOffset;

            ballIndicator.position = targetInsideScreenPos;
            ballIndicator.localPosition = new Vector3(ballIndicator.localPosition.x, ballIndicator.localPosition.y, 0f);
        }
        else
            ballIndicatorSprite.enabled = false;
    }
}
