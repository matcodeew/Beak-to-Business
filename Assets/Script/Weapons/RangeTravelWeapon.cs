using Unity.Netcode;
using UnityEngine;

public class RangeTravelWeapon : Weapon
{
    private GameObject _currentBullet = null;
    private GameObject _bulletPrefab;
    private Vector2 _endPos;
    private Vector2 _startPos;
    public override void Initialize(WeaponStats data)
    {
        base.Initialize(data);
        _bulletPrefab = data.bulletPrefab;
    }
    public override void Shoot(Transform playerTransform)
    {
        ResetData();
        base.Shoot(playerTransform);

        _startPos = (playerTransform.position + playerTransform.up * playerTransform.localScale.x);
        _endPos = _startPos + (Vector2)playerTransform.up * stats.fireRange;

        playerTransform.GetComponent<Player>().RequestSpawnBullet(_startPos, Vector2.right);

        //if (playerTransform.GetComponent<Player>().IsOwner)
        //{
        //    Vector2 direction = Vector2.right;
        //    playerTransform.GetComponent<Player>().SpawnBulletServerRpc(playerTransform.position, direction, _bulletPrefab);
        //}

        //if (NetworkManager.Singleton.IsClient)
        //{
        //    RequestBulletSpawnServerRpc(_startPos, _endPos, playerTransform.GetComponent<NetworkObject>().OwnerClientId);
        //}
    }

    public override GameObject GetBulletPrefab()
    {
        return _bulletPrefab;
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestBulletSpawnServerRpc(Vector2 startPos, Vector2 endPos, ulong ownerClientId)
    {
        GameObject bullet = Instantiate(_bulletPrefab, startPos, Quaternion.identity);

        NetworkObject bulletNetworkObject = bullet.GetComponent<NetworkObject>();
        if (bulletNetworkObject != null)
        {
            bulletNetworkObject.SpawnWithOwnership(ownerClientId);

            bullet.GetComponent<Bullet>().InitializeBullet(startPos, endPos, stats.bulletSpeed, NetworkManager.Singleton.ConnectedClients[ownerClientId].PlayerObject.GetComponent<Player>());
        }
        else
        {
            Debug.LogError("Le prefab de la balle doit avoir un NetworkObject !");
        }
    }

    private void ResetData()
    {
        _currentBullet = null;
        _endPos = Vector2.zero;
        _startPos = Vector2.zero;
    }
}
