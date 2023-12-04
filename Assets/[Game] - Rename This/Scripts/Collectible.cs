using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private float massIncrease = 0.1f;
    [SerializeField] private float sizeIncrease = 0.1f;
    [SerializeField] private ParticleSystem collectionEffect;

    private void OnTriggerEnter(Collider other)
    {
        var playerBrain = other.GetComponent<PlayerBrain>();
        if (playerBrain == null) return;

        playerBrain.Collect(massIncrease, sizeIncrease);
        HapticManager.Haptic(HapticTypes.SoftImpact);

        var effect = Instantiate(collectionEffect, transform.position, Quaternion.identity);
        effect.gameObject.transform.localScale = transform.lossyScale;
        ParticleSystem.MainModule settings = effect.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(GetComponent<MeshRenderer>().material.color);
        effect.Play();

        Destroy(gameObject);
    }
}
