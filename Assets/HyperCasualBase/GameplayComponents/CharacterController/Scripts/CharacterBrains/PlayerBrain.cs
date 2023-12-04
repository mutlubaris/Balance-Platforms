using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : CharacterBrainBase
{
    private Platform currentPlatform = null;
    private bool isFinished;
    
    private Rigidbody _rigid;
    private Rigidbody rigid { get { return (_rigid == null) ? _rigid = GetComponent<Rigidbody>() : _rigid; } }

    private CameraController _cameraController;
    private CameraController cameraController { get { return (_cameraController == null) ? 
                _cameraController = Camera.main.gameObject.GetComponent<CameraController>() : _cameraController; } }

    private void OnEnable()
    {
        isFinished = false;
        transform.localScale = Vector3.one;
        rigid.mass = 1f;
    }

    public override void Logic()
    {
        CharacterController.Move(new Vector3(InputManager.Instance.Joystick.Direction.x, 0f, InputManager.Instance.Joystick.Direction.y));

        if (isFinished) return;

        if (rigid.velocity.y < -30)
        {
            Character.KillCharacter();
            GameManager.Instance.CompilateStage(false);
            isFinished = true;
            return;
        }

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 1.1f))
        {
            var platform = hit.transform.gameObject.GetComponentInChildren<Platform>();

            if (platform == null) return;

            if (currentPlatform == null)
            {
                currentPlatform = platform;
                return;
            }

            if (platform.PlatformID != currentPlatform.PlatformID)
            {
                cameraController.ShakeCamera(8, 2f, 6, .1f);
                
                if (!platform.IsSafe)
                {
                    Character.KillCharacter();
                    Invoke("FinishLevel", 2f);
                    isFinished = true;
                    return;
                }

                if (platform.isLast)
                {
                    GameManager.Instance.CompilateStage(true);
                    isFinished = true;
                    Character.IsControlable = false;
                    
                    return;
                }

                currentPlatform = platform;
                currentPlatform.SetMaterial(true);
                Invoke("ZoomOut", .5f);
            }
        }
    }

    private void FinishLevel()
    {
        GameManager.Instance.CompilateStage(false);
    }

    private void ZoomOut()
    {
        DOTween.To(() => cameraController.distance, x => cameraController.distance = x, cameraController.distance + 12f, .6f).SetEase(Ease.Linear);
    }

    public void Collect(float massIncrease, float sizeIncrease)
    {
        rigid.mass += massIncrease;

        Character.MoveSpeed *= ((transform.localScale.x + sizeIncrease) / transform.localScale.x);

        transform.localScale = transform.localScale + Vector3.one * sizeIncrease;
    }
}
