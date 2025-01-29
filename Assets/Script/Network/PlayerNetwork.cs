using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{

    private float _moveSpeed = 5f;

    private void Update()
    {
        if (!IsOwner) return;

        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDir.z = 1;
        if (Input.GetKey(KeyCode.S)) moveDir.z = -1;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1;
        if (Input.GetKey(KeyCode.D)) moveDir.x = 1;

        transform.position += moveDir * _moveSpeed * Time.deltaTime;
    }

}