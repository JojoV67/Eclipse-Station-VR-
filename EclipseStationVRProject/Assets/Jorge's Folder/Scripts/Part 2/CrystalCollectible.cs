using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class CrystalCollectible : MonoBehaviour
{
    [Header("Audio & Visuals")]
    public AudioClip pickupClip;
    public ParticleSystem pickupEffect;

    private XRGrabInteractable grab;
    private bool collected;
    private Rigidbody rb;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        /*if (collected) return;
        collected = true;

        // Disable further grabs
        grab.enabled = false;

        // Turn off physics so it doesn't float or fall
        rb.isKinematic = true;
        rb.useGravity = false;

        // Hide visual mesh instantly
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
            renderer.enabled = false;

        // Play pickup sound
        if (pickupClip != null)
        {
            var temp = new GameObject("PickupSound");
            var src = temp.AddComponent<AudioSource>();
            src.clip = pickupClip;
            src.spatialBlend = 1f;
            src.Play();
            Destroy(temp, pickupClip.length);
        }

        // Particle burst
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity).Play();

        // Notify manager
        if (CrystalManager.instance)
            CrystalManager.instance.CollectCrystal();

        // Hide mesh and collider
        foreach (var r in GetComponentsInChildren<MeshRenderer>())
            r.enabled = false;
        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        // Destroy after a moment
        Destroy(gameObject, 1f);*/

        if (collected) return;
        collected = true;

        // Stop being grabbable immediately
        grab.enabled = false;

        // Turn off physics so it doesn't float or fall
        rb.isKinematic = true;
        rb.useGravity = false;

        // Hide visual mesh instantly
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
            renderer.enabled = false;

        // Play particle
        if (pickupEffect != null)
        {
            ParticleSystem fx = Instantiate(pickupEffect, transform.position, Quaternion.identity);
            fx.Play();
            Destroy(fx.gameObject, fx.main.duration + 0.5f);
        }

        // Play pickup sound
        if (pickupClip != null)
        {
            GameObject tempAudio = new GameObject("CrystalPickupSound");
            AudioSource audio = tempAudio.AddComponent<AudioSource>();
            audio.clip = pickupClip;
            audio.spatialBlend = 1f;
            audio.Play();
            Destroy(tempAudio, pickupClip.length);
        }

        // Tell manager
        if (CrystalManager.instance != null)
            CrystalManager.instance.CollectCrystal();

        // Destroy the crystal
        Destroy(gameObject, 0.5f);
    }
}
