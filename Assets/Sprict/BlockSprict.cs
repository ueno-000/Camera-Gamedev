using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSprict : MonoBehaviour
{
    //[SerializeField] GameObject _effect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
       // Instantiate(_effect,this.transform);
        Destroy(gameObject);
    }

}


