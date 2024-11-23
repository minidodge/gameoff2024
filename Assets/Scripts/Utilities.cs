using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public static class Utilities {
    public static Vector2 getOffCameraTop() {
        // Get the camera's position
        var camPos = Camera.main.transform.position;

        // Go up 20
        camPos = camPos + Vector3.up * 20;

        return camPos;
    }

    public static void addForceTowardPlayer(GameObject objectToMove, float forceFactor) {
        // Get the player
        var player = getPlayerGameObject();

        // Get a vector
        var vectorToPlayer = GetNormalizedVectorToGameObject(objectToMove, player);

        // Now add force
        objectToMove.GetComponent<Rigidbody2D>().AddForce(vectorToPlayer * forceFactor);
    }

    // Note: intended to be called in Update()
    public static void doSpinning(GameObject objectToSpin, Vector3 rotateVector) {
        objectToSpin.transform.Rotate(rotateVector * Time.deltaTime);
    }

    static IEnumerator SpinningRoutine() {
        yield return null;
    }

    public static GameObject getPlayerGameObject() {
        return GameObject.FindWithTag("Player");
    }

    public static Vector2 getPlayerPosition() {
        // Get the player
        return getPlayerGameObject().transform.position;
    }

    public static Vector3 GetNormalizedVectorToGameObject(GameObject sourceObj, GameObject targetObj) {
        // Get a vector to the player
        var vectorToPlayer = targetObj.transform.position - sourceObj.transform.position;
        vectorToPlayer.Normalize();
        return vectorToPlayer;
    }

    /**
     * Tell the given game object to ignore collisions with all colliders from any of its parents
     */
    private static void ignoreParentCollisions2D(GameObject obj, GameObject self) {
        var objCollider = obj.GetComponent<Collider2D>();
        if (objCollider == null) {
            return;
        }
        
        var currentParent = self.transform.parent;
        while (currentParent != null) {
            var parentCollider = currentParent.GetComponent<Collider2D>();
            if (parentCollider != null) {
                Physics2D.IgnoreCollision(parentCollider, objCollider);
            }

            currentParent = currentParent.parent;
        }
    }
    
    public static void ignoreParentCollisions(GameObject obj, GameObject self) {
        var objCollider = obj.GetComponent<Collider>();

        if (objCollider == null) {
            // try 2d 
            ignoreParentCollisions2D(obj, self);
            return;
        }
        
        var currentParent = self.transform.parent;
        while (currentParent != null) {
            var parentCollider = currentParent.GetComponent<Collider>();
            if (parentCollider != null) {
                Physics.IgnoreCollision(parentCollider, objCollider);
            }

            currentParent = currentParent.parent;
        }
    }

    /**
     * Tell the given game object to stop ignoring  collisions with all colliders from any of its parents
     */
    public static void restoreParentCollisions(GameObject obj, GameObject self) {
        var objCollider = obj.GetComponent<Collider>();
        
        if(objCollider == null) {
            // try 2d 
            restoreParentCollisions2D(obj, self);
            return;
        }
        
        var currentParent = self.transform.parent;
        while (currentParent != null) {
            var parentCollider = currentParent.GetComponent<Collider>();
            if (parentCollider != null) {
                Physics.IgnoreCollision(parentCollider, objCollider, false);
            }

            currentParent = currentParent.parent;
        }
    }
    
    /**
    * Tell the given game object to stop ignoring  collisions with all colliders from any of its parents
    */
    private static void restoreParentCollisions2D(GameObject obj, GameObject self) {
        var objCollider = obj.GetComponent<Collider2D>();
        var currentParent = self.transform.parent;
        while (currentParent != null) {
            var parentCollider = currentParent.GetComponent<Collider2D>();
            if (parentCollider != null) {
                Physics2D.IgnoreCollision(parentCollider, objCollider, false);
            }

            currentParent = currentParent.parent;
        }
    }

    public static bool IsInLayerMask(int layer, LayerMask layermask) {
        return layermask == (layermask | (1 << layer));
    }

    /**
     * Determines the xMin, xMax, yMin, and yMax in world space coordinates of the current camera view relative
     * to the z-distance of the given transform. The four x and y coordinates are returned as a Vector4.
     */
    public static Vector4 DetermineClampBoundry(Transform trans) {
        var zDistance = trans.position.z - Camera.main.transform.position.z;
        return new Vector4(
            Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistance)).x, // xMin (x)
            Camera.main.ViewportToWorldPoint(new Vector3(1, 0, zDistance)).x, // xMax (y)
            Camera.main.ViewportToWorldPoint(new Vector3(0, 0, zDistance)).y, // yMin (z)
            Camera.main.ViewportToWorldPoint(new Vector3(0, 1, zDistance)).y // yMax (w)
        );
    }

    public static void ClampToBoundary(Vector4 boundary, Transform trans) {
        var xPos = Mathf.Clamp(trans.position.x, boundary.x, boundary.y);
        var yPos = Mathf.Clamp(trans.position.y, boundary.z, boundary.w);
        trans.position = new Vector2(xPos, yPos);
    }

    public static void WrapToBoundary(Vector4 boundary, Transform trans) {
        float xPos = trans.position.x;
        float yPos = trans.position.y;

        // If less than the x minimum
        if (trans.position.x < boundary.x) {
            xPos = boundary.y; // Set to the max x
        }
        // If greater than the x maximum
        else if (trans.position.x > boundary.y) {
            xPos = boundary.x; // Set to the min x
        }

        // If less than the y minimum
        if (trans.position.y < boundary.z) {
            yPos = boundary.w; // Set to the max y
        }
        // If greater than the y maximum
        else if (trans.position.y > boundary.w) {
            yPos = boundary.z; // Set to the min y
        }

        trans.position = new Vector2(xPos, yPos);
    }

    /**
     * Map an input range to an output range.
     * See: https://forum.unity.com/threads/mapping-or-scaling-values-to-a-new-range.180090/
     */
    public static float Map2(float outputFrom, float outputTo, float inputFrom, float inputTo, float inputValue) {
        if (inputValue <= inputFrom) {
            return outputFrom;
        }

        if (inputValue >= inputTo) {
            return outputTo;
        }

        return (outputTo - outputFrom) * ((inputValue - inputFrom) / (inputTo - inputFrom)) + outputFrom;
    }

    public static double Map(double x, double in_min, double in_max, double out_min, double out_max, bool clamp = false) {
        if (clamp) x = Math.Max(in_min, Math.Min(x, in_max));
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public static bool IsInput() {
        // todo - centralize input manager
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")) {
            return true;
        }

        return false;
    }

    public static IEnumerator WaitForInput() {
        while (!IsInput()) {
            yield return null;
        }
    }
    
    /**
     * Return a vector3 representing the position of the mouse click on a horizontal plane
     * that is parallel with the 'spawner' (or other object) provided via parameter.
     * This can be used to produce a shot straight from the spawner to the mouse click.
     */
    /*public static Vector3 BulletVelocityFromMouse(Transform spawner) {
        GameObject player = getPlayerGameObject();
        Vector3 planePosition = spawner.transform.position;
        
        // Draw a plane along the surface that this spawner would shoot
        Plane plane = new Plane(spawner.transform.up, spawner.transform.position.z);

        // Get a raycast from the camera to the mouse position
        Vector3 lastMousePos = player.GetComponent<PlayerInput>().lastMousePosition;
        Ray ray = Camera.main.ScreenPointToRay(lastMousePos);

        // Check to see if the ray cast from the camera to the mouse intersects with our plane.
        float distance;
        if (plane.Raycast(ray, out distance)) {
            // Get the intersecting point on the plane - it should be along the direction that the
            // weapon could shoot.
            planePosition = ray.GetPoint(distance);
        }
        else {
            Debug.Log("No hit");
        }

        return planePosition;
    }*/
}

[System.Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { }