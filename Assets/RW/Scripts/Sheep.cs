using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public float heartOffset; // 1
    public GameObject heartPrefab; // 2

    public float runSpeed;
    public float gotHayDestroyDelay;

    private bool hitByHay;
    public float dropDestroyDelay;
    private Collider myCollider;
    private Rigidbody myRigidbody;
    private SheepSpawner sheepSpawner;


    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }

    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }


    

    private void HitByHay()
    {
        

        SoundManager.Instance.PlaySheepHitClip();
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ; // 1
        tweenScale.targetScale = 0; // 2
        tweenScale.timeToReachTarget = gotHayDestroyDelay; // 3
        

        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true;
        runSpeed = 0;

        Destroy(gameObject, gotHayDestroyDelay);
        
        GameStateManager.Instance.SavedSheep();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hay")&& !hitByHay)
        {
            Destroy(other.gameObject);
            HitByHay();
        }

        else if (other.CompareTag("DropSheep"))
        {
            Drop();
        }

    }


    private void Drop()
    {
        

        SoundManager.Instance.PlaySheepDroppedClip();

        GameStateManager.Instance.DroppedSheep();
        sheepSpawner.RemoveSheepFromList(gameObject);

        myRigidbody.isKinematic = false;
        myCollider.isTrigger = false;
        Destroy(gameObject, dropDestroyDelay);
    }

    

}
