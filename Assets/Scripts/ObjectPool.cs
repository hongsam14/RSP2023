using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private DropObject _spawnPrefab;
    [SerializeField] private float size;
    [SerializeField] private int _spawnAmount;
    [SerializeField] private int _maxSpawnAmount;

    private ObjectPool<DropObject> _pool;
    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        //init object pool
        _pool = new ObjectPool<DropObject>(() =>
            {
                return Instantiate<DropObject>(_spawnPrefab);
            },
            (prefab) =>
            {
                prefab.gameObject.SetActive(true);
            },
            (prefab) =>
            {
                prefab.gameObject.SetActive(false);
            },
            (prefab) =>
            {
                Destroy(prefab.gameObject);
            },
            false, _spawnAmount, _maxSpawnAmount
            );

        InvokeRepeating(nameof(Spawn), 1f, 5f);
    }

    private void Spawn()
    {
        for (int i = 0; i < _spawnAmount; i++)
        {
            DropObject p = _pool.Get();
            float x = Random.Range(-1 * (size / 2f), size / 2f);
            p.transform.position = transform.position + new Vector3(x, -2f, 0f);
            p.killAction = Kill;
        }
    }

    private void Kill(DropObject obj)
    {
        _pool.Release(obj);
    }
}
