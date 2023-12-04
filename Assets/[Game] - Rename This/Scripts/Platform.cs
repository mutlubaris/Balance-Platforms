using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Material safeMaterial;
    [SerializeField] private Material dangerMaterial;
    [SerializeField] private Material defaultMaterial;

    private SpringJoint[] springJoints;
    private Rigidbody rigid;
    private Character player = null;
    private MeshRenderer meshRenderer;
    private bool playerInRange;
    private bool isSafe;

    public bool IsSafe { get { return isSafe; } }

    public bool isLast;

    public int PlatformID;

    private void OnEnable()
    {
        if (Managers.Instance == null) return;

        LevelManager.Instance.OnLevelStart.AddListener(GetPlayer);
        
        springJoints = GetComponents<SpringJoint>();
        rigid = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = defaultMaterial;
        isSafe = true;
        playerInRange = false;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;

        LevelManager.Instance.OnLevelStart.RemoveListener(GetPlayer);
    }

    private void GetPlayer()
    {
        player = CharacterManager.Instance.Player;
    }

    public void DisableSprings()
    {
        foreach (var springJoint in springJoints)
        {
            Destroy(springJoint);
        }
        Destroy(rigid);
    }

    private void Update()
    {
        if (playerInRange || player == null) return;
        if (player.transform.position.y - 2f < transform.position.y)
        {
            playerInRange = true;
            return;
        }
        if (!player.IsGrounded) return;
        SetMaterial(false);
    }

    public void SetMaterial(bool toDefault)
    {
        if (toDefault)
        {
            meshRenderer.material = defaultMaterial;
            isSafe = true;
        }
        else
        {
            if (player.transform.position.y - player.transform.localScale.magnitude > transform.position.y)
            {
                meshRenderer.material = dangerMaterial;
                isSafe = false;
            }
            else
            {
                meshRenderer.material = safeMaterial;
                isSafe = true;
            }
        }
    }
}
