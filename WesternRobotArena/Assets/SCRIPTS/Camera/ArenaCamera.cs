using System.Collections.Generic;
using UnityEngine;

public class ArenaCamera : MonoBehaviour
{
    [SerializeField]
    private float minZoom = 6f;

    [SerializeField]
    private float screenPadding = 2f;

    [SerializeField]
    private float smoothTime = 0.2f;


    [SerializeField]
    private float rotationSmoothTime = 5f;


    private Camera cam;
    private List<Transform> players = new List<Transform>();

    private float zoomVelocity;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    private void LateUpdate()
    {
        players.RemoveAll(player => player == null); // removes destroyed player references

        if (players.Count == 0)
            return;

        Vector3 targetPosition = GetTargetPosition();

        RotateCamera(targetPosition);
        ZoomCamera();
    }

    public void AddPlayer(Transform player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);

            Debug.Log("Camera added player: " + player.name);
        }
    }

    public void RemovePlayer(Transform player)
    {
        players.Remove(player);
    }

    private Vector3 GetTargetPosition() // gets the middle point between both players
    {
        if (players.Count == 1)
            return players[0].position;

        return (players[0].position + players[1].position) / 2;
    }

    private void RotateCamera(Vector3 target)
    {
        // calculate the direction from the camera to the players midpoint
        Vector3 direction = target - transform.position;

        if (direction.sqrMagnitude < 0.001f)
            return;

        // calculate the rotation required to look at the midpoint
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

        // smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothTime * Time.deltaTime);
    }

    private void ZoomCamera()
    {
        var targetZoom = minZoom;

        if (players.Count >= 2)
        {
            //converts player positions relative to the camera
            var player1Position = transform.InverseTransformPoint(players[0].position);
            var player2Position = transform.InverseTransformPoint(players[1].position);

            var horizontalDistance = Mathf.Max(Mathf.Abs(player1Position.x), Mathf.Abs(player2Position.x));

            var verticalDistance = Mathf.Max(Mathf.Abs(player1Position.y), Mathf.Abs(player2Position.y));

            //accounts for the screen width
            var horizontalZoom = horizontalDistance / cam.aspect;

            targetZoom = Mathf.Max(verticalDistance, horizontalZoom);
            targetZoom += screenPadding;

            if (targetZoom < minZoom)
                targetZoom = minZoom;
        }

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoom, ref zoomVelocity, smoothTime);
    }
}