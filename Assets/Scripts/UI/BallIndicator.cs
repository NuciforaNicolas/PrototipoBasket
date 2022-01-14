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
            var cappedTargetScreenPos = targetPosScreenPoint;
            if (cappedTargetScreenPos.x <= screenBorderOffset) cappedTargetScreenPos.x = screenBorderOffset;
            if (cappedTargetScreenPos.x >= Screen.width - screenBorderOffset) cappedTargetScreenPos.x = Screen.width - screenBorderOffset;
            if (cappedTargetScreenPos.y <= screenBorderOffset) cappedTargetScreenPos.y = screenBorderOffset;
            if (cappedTargetScreenPos.y >= Screen.height - screenBorderOffset) cappedTargetScreenPos.y = Screen.height - screenBorderOffset;

            ballIndicator.position = cappedTargetScreenPos;
            ballIndicator.localPosition = new Vector3(ballIndicator.localPosition.x, ballIndicator.localPosition.y, 0f);
        }
        else
            ballIndicatorSprite.enabled = false;
    }
}
