using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

    public string targetTag = "Player"; // Tag of the target (usually the player)
    private GameObject target;
    
    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;
    
    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;
    
    public bool pathIsEnded = false;
    
    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    void Start () {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        
        // Find the target (player) in the scene
        FindTarget();
        
        if (target == null) {
            Debug.LogError ("Target not found. Make sure there's a GameObject with the tag '" + targetTag + "' in the scene.");
            return;
        }
        
        StartCoroutine (UpdatePath ());
    }
    
    // Method to find the target in the scene
    void FindTarget() {
        target = GameObject.FindGameObjectWithTag(targetTag);
    }
    
    // Public method to set the target manually
    public void SetTarget(GameObject newTarget) {
        target = newTarget;
    }
    
    IEnumerator UpdatePath () {
        if (target != null) {
            seeker.StartPath (transform.position, target.transform.position, OnPathComplete);
        }
        yield return new WaitForSeconds ( 1f/updateRate );
        StartCoroutine (UpdatePath());
    }
    
    public void OnPathComplete (Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }
    
    void FixedUpdate () {
        if (target == null || path == null) return;
        
        if (currentWaypoint >= path.vectorPath.Count) {
            if (pathIsEnded)
                return;
            
            Debug.Log ("End of path reached.");
            pathIsEnded = true;
            return;
        }
        pathIsEnded = false;
    
        Vector3 dir = ( path.vectorPath[currentWaypoint] - transform.position ).normalized;
        dir *= speed * Time.fixedDeltaTime;
        
        rb.AddForce (dir, fMode);
        
        float dist = Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]);
        if (dist < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
    }
}
