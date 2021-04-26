using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public float maxThrowStrength;
    public float chargeTime = 1;

    public GameObject angleBar;
    public GameObject chargeBar;
    public float barDistance;

    public Throwable holding;
    public Collider2D pickupCollision;

    public float charge = 0;

    private float CalculateAngle(Vector2 pointA, Vector2 pointB) {
        float angle = (180 / Mathf.PI) * Mathf.Atan2(pointB.y - pointA.y, pointB.x - pointA.x) + 180;

        if (angle >= 360) {
            angle -= 360;
        }
        if (angle < 0) {
            angle += 360;
        }

        return angle;
    }

    private Vector2 CalculateAngleVector(Vector2 pointA, Vector2 pointB) {
        float angle = CalculateAngle(pointA, pointB);

        Vector2 angleVector = new Vector2(0, 0);

        if (angle < 90) {
            float perc = (angle) / 90;
            angleVector = new Vector2(-(1-perc), -perc);
        }
        else if (angle < 180) {
            float perc = (angle - 90) / 90;
            angleVector = new Vector2(perc, -(1-perc));
        }
        else if (angle < 270) {
            float perc = (angle - 180) / 90;
            angleVector = new Vector2(1-perc, perc);
        }
        else {
            float perc = (angle - 270) / 90;
            angleVector = new Vector2(-perc, 1-perc);
        }

        return angleVector;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (holding != null) {
            // Set Holding Object Pos
            GameObject holdingObject = holding.gameObject;
            holdingObject.transform.position = transform.position;

            // Calculate Angle
            float mouseAngle = CalculateAngle(gameObject.transform.position, mouseWorldPos);
            Vector2 throwAngle = CalculateAngleVector(gameObject.transform.position, mouseWorldPos);

            // Reset Charge
            if (Input.GetMouseButtonDown(0)) {
                charge = 0.1f;
            }
            

            // Charge
            if (Input.GetMouseButton(0)) {
                if (charge < 1) {
                    charge += (Time.deltaTime / chargeTime);
                }
                else {
                    charge = 1;
                }

                // Angle Bar
                angleBar.SetActive(true);
                Vector2 angleBarPos = (Vector2)transform.position - new Vector2(barDistance *  Mathf.Cos(mouseAngle * 0.0174532925f), barDistance *  Mathf.Sin(mouseAngle * 0.0174532925f));
                angleBar.transform.position = angleBarPos;
                angleBar.transform.localRotation = Quaternion.Euler(0, 0, mouseAngle);

                // Charge Bar
                chargeBar.transform.localScale = new Vector3(charge, 1, 1);
                chargeBar.transform.localPosition = new Vector3(0.25f - (charge / 4), 0, 0);
            }
            else{
                angleBar.SetActive(false);
            }

            // Throw
            if (Input.GetMouseButtonUp(0)) {
                // Calculate where to throw
                

                float throwStrength = (maxThrowStrength * charge);

                // Actually Apply Throw
                holding.beingHeld = false;
                holding.thrownStrength = charge;
                holding.canBePickedUp = false;
                holding.rigidBody.velocity = throwAngle * throwStrength;
                if (throwAngle.x < 0) {
                    holding.rigidBody.angularVelocity = throwStrength * 135;
                }
                else {
                    holding.rigidBody.angularVelocity = throwStrength * 135 * -1;
                }
                holding = null;
            }

        }
        else {
            charge = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Throwable")) {
            Throwable hitThrowable = other.GetComponent<Throwable>();;
            if (hitThrowable != null) {
                // Disable collisions between player and throwables
                if (!hitThrowable.collisionSetup) {
                    Collider2D[] playerColliders = gameObject.GetComponents<Collider2D>();
                    for (var i = 0; i < playerColliders.Length; i++) {
                        if (!playerColliders[i].isTrigger) {
                            Physics2D.IgnoreCollision(playerColliders[i], hitThrowable.collision);
                        }
                    }
                }

                // Pickup Throwable
                if (hitThrowable.canBePickedUp && holding == null) {
                    holding = hitThrowable;
                    holding.beingHeld = true;
                    holding.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
            }
        }
    }
}
