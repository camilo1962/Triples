using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class Launcher : MonoBehaviour
{
    public Transform launchPosition;
    public float force = 0f;
    private float startRange =148;
    public float rangeMin = 145; // 0.69
    public float rangeMax =230; // 0.69
    public int posIndex = 0;
    Transform startBall;
    private bool lanzada = false;
    private bool locked = false;

    public float pressTime = 0f;
    private float releaseTime = 0f;
    private GameManager gameManager;

    public Vector3 canastaPos;
    public int value = 0;
    public int shotNumber = 0;

    //public Image shootBar;
    //public Slider slider;
    public GameObject powerBar;

    public AudioClip Aro;
    public AudioClip Bote;
    public AudioClip Tablero;
    public AudioClip Red;

   


    void Start()
    {      

        startBall = GetComponent<Transform>();
        canastaPos = GameObject.FindWithTag("Final").GetComponent<Transform>().position;
        transform.LookAt(canastaPos);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //GetComponentInParent<AutoDestroid>().enabled = false;
        //shootBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (locked) return;
        if (lanzada)
        {           
         gameManager.ResizeShotBar(Time.time - pressTime);           
        }
        if (SimpleInput.GetButtonDown("Pasa"))
        {
            Destroy(gameObject);
        }

        //if (Input.GetKeyDown(KeyCode.Space) && !lanzada)
        if(SimpleInput.GetButtonDown("Bola") && !lanzada)
        {
            pressTime = Time.time;
            lanzada = true;           
            
        }
        //else if(Input.GetKeyUp(KeyCode.Space) && lanzada)
        else if(SimpleInput.GetButtonUp("Bola") && lanzada)
        {

            gameManager.CheckshotIcons(shotNumber);
            locked = true;
            //GetComponentInParent<AutoDestroy>().enabled = true;
            releaseTime = Time.time;

            float diff = releaseTime - pressTime;
            //muy rápido: 0,083
            // normal: 0.363
                   

            GetComponent<Rigidbody>().useGravity = true;
            Vector3 dir = Vector3.zero;

            switch (posIndex)
            {
                case 0: dir = Vector3.up - Vector3.left;startRange = 150f;  break;// 155.1-156.0
                case 1: dir = Vector3.up - Vector3.back; startRange = 150f; break;  // 154.6 -155-4
                case 2: dir = Vector3.up - Vector3.right; startRange = 150f; break; // 152.8 - 153
                case 3: dir = Vector3.up - Vector3.right; startRange = 150f; break; // 155.3 - 155.6
                case 4: dir = Vector3.up - Vector3.right; startRange = 150f; break;  // 152.1 - 152.9
                default: break;
            }
            force = startRange + (diff * 10);

            if (gameManager.shootBar.localScale.x < 0.70 && gameManager.shootBar.localScale.x > 0.41)
            {
                switch (posIndex)
                {
                    case 0: force = Random.Range(155.1f, 156.0f); break;
                    case 1: force = Random.Range(154.6f, 155.4f); break;
                    case 2: force = Random.Range(154.8f, 155.0f); break;
                    case 3: force = Random.Range(155.3f, 155.6f); break;
                    case 4: force = Random.Range(152.1f, 152.9f); break;
                    default: break;
                }
            }

            
            transform.LookAt(canastaPos);
            dir = canastaPos - launchPosition.position;

            Debug.Log(force + " - " + diff);     
                       
            GetComponent<Rigidbody>().AddForce(force * dir.normalized, ForceMode.Impulse);
           
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            GetComponent<AudioSource>().PlayOneShot(Bote);
           
        }
        else if(collision.gameObject.tag == "ring")
        {
            GetComponent<AudioSource>().PlayOneShot(Aro);
        }
        else if (collision.gameObject.tag == "net")
        {
            GetComponent<AudioSource>().PlayOneShot(Red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "score")
        {
            GetComponent<AudioSource>().PlayOneShot(Red);
            gameManager.AddScore(value);
        }
        if (other.gameObject.tag == "limites")
        {
            Destroy(this.gameObject);
        }
    }
}
