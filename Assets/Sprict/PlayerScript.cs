using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] public float _speed;

    Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // 左右のキー入力により速度を変更する
        _rb.velocity = new Vector3(Input.GetAxis("Horizontal") * _speed, 0f, 0f);
    }
}
