using System.Collections.Generic;
using UnityEngine;

public class ArenaCamera : MonoBehaviour
{
    [SerializeField]
    private float cameraDistance = 20f;

    [SerializeField]
    private float minZoom = 6f;

    [SerializeField]
    private float screenPadding = 2f;

    [SerializeField]
    private float smoothTime = 0.2f;

    private Camera cam;
    private List<Transform> players = new List<Transform>();

    private Vector3 cameraVelocity;
    private float zoomVelocity;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    private void LateUpdate()
    {
        if (players.Count == 0)
            return;

        var targetPosition = GetTargetPosition();

        MoveCamera(targetPosition);
        ZoomCamera();
    }

    public void AddPlayer(Transform player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }

    public void RemovePlayer(Transform player)
    {
        players.Remove(player);
    }

    private Vector3 GetTargetPosition() //gets the middle point between both players
    {
        if (players.Count == 1)
            return players[0].position;

        return (players[0].position + players[1].position) / 2;
    }

    private void MoveCamera(Vector3 target)
    {
        var cameraOffset = -transform.forward * cameraDistance;
        var targetPosition = target + cameraOffset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref cameraVelocity, smoothTime);
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