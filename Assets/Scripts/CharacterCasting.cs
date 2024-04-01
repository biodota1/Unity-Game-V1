using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCasting : MonoBehaviour
{

    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public float cooldownTime = 5f;
    private bool isCooldown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isCooldown)
            {
                StartCoroutine(CooldownCoroutine());
                var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().velocity = projectileSpawnPoint.forward * projectileSpeed;
            }
           
        }
    }
}
