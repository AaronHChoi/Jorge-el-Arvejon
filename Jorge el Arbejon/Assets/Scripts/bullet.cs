using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private float lifetime;

    // Start is called before the first frame update
    private void Update()
    {
        

        float movementSpeed = speed * Time.deltaTime * direction;

        transform.Translate(movementSpeed, 0 , 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);

    }

    public void SetDirection(float _direction)
    {

        lifetime = 0;

        direction = _direction;

        gameObject.SetActive(true);

        hit = false;



        float localScaleX = transform.localScale.x;

        if(Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }


}