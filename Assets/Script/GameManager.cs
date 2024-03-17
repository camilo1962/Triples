using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject balltricolorPrefab;
    public GameObject panelGameOver;
    public float force = 2000f;

    public GameObject StartCamera;

    public Transform[] launchPosiciones;
    public GameObject[] launchCameras;
    public Transform[] launchesTargets;
    private int posIndex = 0;
    private int camIndex = 0;
    public int shotNumber = 0;
    private Vector3 canastaPos;

    public TMP_Text UIScore;
    public TMP_Text UITime;
    public RectTransform shootBar;
   

    public float totalTime = 60f;
    private int score = 0;
    private int scoreFinal;
    public TMP_Text scoreFinalText;
    private int record;
    public TMP_Text recordText;
    public TMP_Text recordFinalText;

    private float timeRemaining = 60;
    private bool gameOver = false;
    private bool gameStarted = false;
    private float startTime = 0f;

    public Image[] shotIcons;

   

    void Start()
    {
        panelGameOver.SetActive(false);
        //canastaPos = GameObject.FindWithTag("Final").GetComponent<Transform>().position;
        recordText.text = PlayerPrefs.GetInt("Record", 0).ToString();
        recordFinalText.text = PlayerPrefs.GetInt("Record", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (gameStarted)
        {
            
            timeRemaining = Time.time - startTime;
            timeRemaining = totalTime - timeRemaining;
            UITime.text = Mathf.CeilToInt(timeRemaining).ToString();

            if(UITime.text == "0")
            {
                GetComponent<AudioSource>().Play();
                gameStarted = false;
                gameOver = true;
                GameOver();
            }
        }

        //if (Input.GetKeyUp(KeyCode.R) && !gameOver)
        if (SimpleInput.GetButtonDown("Pasa") && !gameOver)
        {
            if(!gameStarted)
            {
                gameStarted = true;
                startTime = Time.time;
            }
            

           if(posIndex < launchPosiciones.Length)
           {
                if (shotNumber > 4)
                {
                    posIndex++;
                    if (posIndex > launchPosiciones.Length)
                    {
                        posIndex = 0;
                    }
                    

                    camIndex++;
                    if (camIndex > launchCameras.Length)
                    {
                        camIndex = 0;
                    }
                    
                    shotNumber = 0;

                    ResetShotIcons();
                }
                NewBall();
                NewCamera();
                shotNumber++;
           }
            else
            {
                GameOver();
            }


        }
        
        
    }

    void NewBall()
    {
        
        GameObject ball;
        if(shotNumber != 4)
        {
            ball = Instantiate(ballPrefab, launchPosiciones[posIndex].position, launchPosiciones[posIndex].rotation) as GameObject;
            ball.GetComponentInChildren<Launcher>().value = 1;
        }
        else
        {
            ball = Instantiate(balltricolorPrefab, launchPosiciones[posIndex].position, launchPosiciones[posIndex].rotation) as GameObject;
            ball.GetComponentInChildren<Launcher>().value = 2;
        }

        ball.GetComponentInChildren<Launcher>().shotNumber = shotNumber;
        ball.GetComponentInChildren<Launcher>().force = force;
        ball.GetComponentInChildren<Launcher>().posIndex = posIndex;
        ball.GetComponentInChildren<Launcher>().canastaPos = launchesTargets[posIndex].transform.position;
        //ball.transform.LookAt(canastaPos);
        //if (!launchPosiciones[posIndex] || shotNumber == 0)
        //{
        //    GameOver();
        //}

    }
    void NewCamera()
    {
        if(camIndex >  0)
        {
            launchCameras[camIndex - 1].SetActive(false);
        }             
        else if(camIndex == 0)
        {
            launchCameras[launchCameras.Length - 1].SetActive(false);
            StartCamera.SetActive(false);
        }

        if(camIndex < launchCameras.Length)
        {
            launchCameras[camIndex].SetActive(true);
          
        }
        else
        {
            StartCamera.SetActive(true);
        }
       


    }

    public void AddScore(int i = 1)
    {
        score += i;
        UIScore.text = score.ToString();

        if(score > PlayerPrefs.GetInt("Record", 0))
        {
            PlayerPrefs.SetInt("Record", score);
            recordText.text = score.ToString();
            recordFinalText.text = score.ToString();
        }

       
        
    }

    public void CheckshotIcons(int index)
    {
        shotIcons[index].gameObject.SetActive(false);
    }

    public void ResetShotIcons()
    {
        foreach(Image img in shotIcons)
        {
            img.gameObject.SetActive(true);
        }
    }  

    public void ResizeShotBar(float value)
    {
        Vector3 newScale = Vector3.one;
        if (value > 1)
            value = 1;
        newScale.x = 1f - value;
        shootBar.localScale = newScale;
    }

    public void GameOver()
    {
        panelGameOver.SetActive(true);
        scoreFinal = score;
        scoreFinalText.text = scoreFinal.ToString();
        
    }
    

   
   
}
