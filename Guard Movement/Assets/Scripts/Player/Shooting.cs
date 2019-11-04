using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Shooting : MonoBehaviour
{

    public GameObject Projectile;

    private float _force = 300f;
    private float _timeHeld;
    private bool _held;
    private int _amount = 5;

    // Update is called once per frame
    void Update()
    {
        if (_amount <= 0)
        {
            return;
        }

        if (!_held && Input.GetKeyDown(KeyCode.G))
        {
            _held = true;
        }
        else if (_held && Input.GetKeyUp(KeyCode.G))
        {
            var spawnPosition = transform.position;

            spawnPosition.x += 1f;
            var instance = Instantiate(Projectile, spawnPosition, Quaternion.identity);
            var rigidBody = instance.GetComponent<Rigidbody>();
            rigidBody.AddRelativeForce(new Vector3(0, 0, _timeHeld * _force));
            _held = false;
            _timeHeld = 0f;
            _amount--;
            Game.UI.SetAmmo(_amount);
        }

        if (_held == false)
        {
            return;
        }

        if (Input.GetKey(KeyCode.G))
        {
            _timeHeld += Time.deltaTime;
        }
    }
}
