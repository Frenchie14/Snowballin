﻿using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    private float originalScale;
    private float originalDistance;
    private Vector3 originalPlayerScale;
    private Player player;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
    void Update() {
        if (player != null) {
            transform.localScale = Vector3.one * originalScale / transform.parent.localScale.magnitude;
            transform.position = player.transform.position + (transform.position - player.transform.position).normalized * originalDistance;
        }
    }

    public void Attach(Player player) {
        this.player = player;
        originalScale = transform.localScale.magnitude;
        originalDistance = (player.transform.position - transform.position).magnitude * 0.8f;
        transform.parent = player.transform;
        originalPlayerScale = player.targetScale;
    }


    // Detach if the player lost more scale then when we attached
    public void CheckDetach() {
        if (player.targetScale.magnitude < originalPlayerScale.magnitude) {
            Blast(player.GetComponent<Rigidbody>().velocity.magnitude);
            transform.parent = null;
            player = null;
        }
    }

    // We've detached or been hit by something smaller
    public void Blast(float playerVel) {
        Invoke("Die", 5);
        GetComponent<Collider>().enabled = false;
        Vector3 detachedVel = Random.onUnitSphere;
        rb.angularVelocity = detachedVel * 2;
        if (detachedVel.y < 0) {
            detachedVel.y = -detachedVel.y;
        }
        if (detachedVel.z < 0) {
            detachedVel.z = -detachedVel.z;
        }
        detachedVel.z += 3f;
        detachedVel *= Random.Range(3f, 5f);
        detachedVel *= Mathf.Clamp(playerVel / 5, 1, 10);
        rb.velocity = detachedVel;
        rb.isKinematic = false;
    }

    private void Die() {
        Destroy(gameObject);
    }
}
