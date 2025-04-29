using UnityEngine;

public class SlugMove : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float speed = 2.0f;

    private int currentWaypointIndex = 0;

    private void Update()
    {
        if (waypoints.Length == 0) return;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}
