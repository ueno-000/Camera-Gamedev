using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSprict : MonoBehaviour
{
    /// <summary>ボールの速度</summary>
    [SerializeField] float _speed = 0;
    /// <summary>速度の最小値</summary>
    [Header("速度の最小値"), SerializeField] float _min = 0;
    /// <summary>速度の最大値</summary>
    [Header("速度の最大値"), SerializeField] float _max = 0;

    Rigidbody2D _rb;
    Transform _transform;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector3(_speed, _speed, 0f);

        _transform = transform;
    }

    // 毎フレーム速度をチェックする
    void Update()
    {
        // 現在の速度を取得
        Vector3 velocity = _rb.velocity;
        // 速さを計算
        float clampedSpeed = Mathf.Clamp(velocity.magnitude, _min, _max);
        // 速度を変更
        _rb.velocity = velocity.normalized * clampedSpeed;
    }

    // 衝突したときに呼ばれる
    void OnCollisionEnter2D(Collision2D collision)
    {
        // プレイヤーに当たったときに、跳ね返る方向を変える
        if (collision.gameObject.CompareTag("Player"))
        {
            // プレイヤーの位置を取得
            Vector3 playerPos = collision.transform.position;
            // ボールの位置を取得
            Vector3 ballPos = _transform.position;
            // プレイヤーから見たボールの方向を計算
            Vector3 direction = (ballPos - playerPos).normalized;
            // 現在の速さを取得
            float speed = _rb.velocity.magnitude;
            // 速度を変更
            _rb.velocity = direction * speed;
        }
    }
}
