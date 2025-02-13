using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : NetworkBehaviour
{
    private Vector2 _startPos = new Vector2();
    private Vector2 _endPos = new Vector2();
    private float _speed = 5f;
    private float _distTreshHold = 0.05f;
    public ulong throwerID = 0;
    private float _damages = 0;
    public float Damages { get { return _damages; } private set { _damages = value; } }

    [ClientRpc(RequireOwnership = false)]
    public void InitializeBulletClientRpc(float speed, float range, Vector2 direction, ulong throwerID, float damages)
    {
        _startPos = transform.position;
        _endPos = _startPos + direction * range;
        _speed = speed;
        _damages = damages;

        Vector3 endPos = _endPos;
        Vector3 dir = endPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        this.throwerID = throwerID;
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
