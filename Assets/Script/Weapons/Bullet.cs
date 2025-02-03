using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    private Vector2 _startPos;
    private Vector2 _endPos;
    private float _speed = 5f;
    private float _distTreshHold = 0.05f;
    public Player playerLuncher;

    private void Start()
    {
        Debug.Log("---------- Bullet Spawned ----------");
    }

    public void InitializeBullet(Vector2 startPos, Vector2 endPos, float speed, Player playerLuncher)
    {
        _startPos = startPos;
        _endPos = endPos;
        _speed = speed;
        this.playerLuncher = playerLuncher;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _endPos, _speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, _endPos) < _distTreshHold)
        {
            Server.instance.DestroyObjectOnServerRpc(this.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }
}
