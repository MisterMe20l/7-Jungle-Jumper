using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] Ammo ammo;
    [SerializeField] Weapons weapon;

    [SerializeField] Rigidbody2D m_rb;
    [SerializeField] LayerMask m_groundLayer;

    GameObject target;
    

    Camera mainCam;
    Vector3 mousePos;
    Vector3 direction;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag((m_groundLayer == 7) ? "Player" : "Enemy");

        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        direction = mousePos - transform.position;

        var rotation = transform.position - mousePos;
        var rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        m_rb.velocity = new Vector2(direction.x, direction.y).normalized * weapon.m_projectileSpeed;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    private void Update()
    {
        OnHit();
    }

    private bool HitWall()
    {
        return Physics2D.OverlapBox(transform.position, 
            transform.lossyScale, 0, m_groundLayer) != null;
    }

    private bool HitTarget()
    {
        return Physics2D.OverlapBox(transform.position, 
            transform.lossyScale, 0, ammo.m_targetLayer) != null;
    }

    private void OnHit()
    {
        if (HitWall())
        {
            Destroy(gameObject);
            Destroy(this);
        }
        if (HitTarget())
        {
            target.GetComponent<ICombat>().TakeDamage(weapon.m_damage);
            Destroy(gameObject);
            Destroy(this);
        }
    }
}
