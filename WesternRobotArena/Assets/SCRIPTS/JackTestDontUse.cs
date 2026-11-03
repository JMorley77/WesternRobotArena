using UnityEngine;
using UnityEngine.InputSystem;

public class JackTestDontUse : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) input.y++;
        if (Keyboard.current.sKey.isPressed) input.y--;
        if (Keyboard.current.aKey.isPressed) input.x--;
        if (Keyboard.current.dKey.isPressed) input.x++;

        Vector3 move = new Vector3(input.x, 0, input.y).normalized;

        transform.Translate(move * speed * Time.deltaTime);
    }
}