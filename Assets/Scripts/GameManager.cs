using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    /// <summary>
    /// Level objects
    /// </summary>
    public GameObject[] obstacleSegments;
    public GameObject startSegment;

    /// <summary>
    /// Blocks in scene
    /// </summary>
    private Obstacle newestBlock;
    private Obstacle oldestBlock;
    private List<Transform> activeBlocks = new List<Transform>();

    /// <summary>
    /// References
    /// </summary>
    public PlayerController controller;
    public Text scoreText;
    public Image fade;
    private CameraController cam;

    /// <summary>
    /// Time control
    /// </summary>
    private float timeScale = 1.2f;
    private int minutes = 0;
    private float seconds = 0.00f;
    private string timer;
    private float startTime;

    /// <summary>
    /// Other
    /// </summary>
    public bool active = true;
    private float spawnDepth = 350.0f;
    private float spacing = 30.0f;
    private float zBuffer = 0.0f;

	// Use this for initialization
	void Start ()
    {
        Time.timeScale = timeScale;
        cam = GameObject.Find("Normal Camera").GetComponent<CameraController>();
        //activeSegments.Add(Instantiate(obstacleSegments[0], Vector3.forward * spacing * 2, Quaternion.identity).GetComponent<Rigidbody>());
        startTime = Time.time;
        
        //Spawn empty starting block
        Obstacle block = Instantiate(startSegment, Vector3.back * 25.0f, Quaternion.identity).GetComponent<Obstacle>();
        newestBlock = block;
        activeBlocks.Add(newestBlock.transform);
        
    }
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            if (oldestBlock == null)
            {
                oldestBlock = (Obstacle)activeBlocks[0].GetComponent<Obstacle>();
            }
            
            //Spawn blocks behind the camera
            if (oldestBlock.transform.position.z + oldestBlock.length + 25.0f <= 0.0f)
            {
                activeBlocks.RemoveAt(0);
                Destroy(oldestBlock.gameObject);
            }

            //Spawn blocks
            if (newestBlock.transform.position.z + newestBlock.length <= spawnDepth)
            {
                Obstacle block = Instantiate(obstacleSegments[(int)(Random.Range(0.0f, (float)obstacleSegments.Length))]).GetComponent<Obstacle>();
                block.transform.position = Vector3.forward * (newestBlock.transform.position.z + newestBlock.length);
                block.transform.localScale = new Vector3(Random.value > 0.5f ? 1.0f : -1.0f, 1.0f, block.transform.localScale.z);

                newestBlock = block;
                activeBlocks.Add(newestBlock.transform);

            }


            for (int i = 0; i < activeBlocks.Count; i++)
            {
                activeBlocks[i].position = activeBlocks[i].position + Vector3.back * Time.deltaTime * 15;
            }



            //Additional controls
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reset());
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadScene(0);
            }


            //Time management
            seconds += Time.deltaTime / timeScale;
            if (seconds >= 60.0f)
            {
                minutes++;
                seconds -= 60.0f;
            }
            string timer = "";
            if (minutes < 10)
                timer += "0";
            timer += minutes + ":";
            if (seconds < 10.0f)
                timer += "0";
            timer += seconds.ToString("F2");
            if (active)
                scoreText.text = timer;
        }
        



        /*
        if (active)
        {
            zBuffer = 0.0f;

            for (int i = 0; i < activeSegments.Count; i++)
            {
                if (activeSegments[i].transform.position.z < -spacing * 2)
                {
                    Destroy(activeSegments[i].gameObject);
                    activeSegments.RemoveAt(i);
                    i--;
                }
                else
                {
                    activeSegments[i].velocity = Vector3.back * 15;
                    if (activeSegments[i].transform.position.z > zBuffer)
                        zBuffer = activeSegments[i].transform.position.z;
                }
            }

            if (activeSegments.Count < 5 && zBuffer <= spacing * 2.0f)
            {
                GameObject go = Instantiate(obstacleSegments[(int)(Random.Range(0.0f, (float)obstacleSegments.Length))], Vector3.forward * spacing * 3.0f, Quaternion.identity);
                go.transform.localScale = new Vector3(Random.value > 0.5f ? 1.0f : -1.0f, 1.0f, 1.0f);
                activeSegments.Add(go.GetComponent<Rigidbody>());
                
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reset());
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadScene(0);
            }



            seconds += Time.deltaTime / timeScale;
            if (seconds >= 60.0f)
            {
                minutes++;
                seconds -= 60.0f;
            }
            string timer = "";
            if (minutes < 10)
                timer += "0";
            timer += minutes + ":";
            if (seconds < 10.0f)
                timer += "0";
            timer += seconds.ToString("F2");
            if (active)
                scoreText.text = timer;
        }

      */
    }

    public void FailState(Vector3 velocity)
    {
        active = false;
        //for (int i = 0; i < activeSegments.Count; i++)
        //    activeSegments[i].isKinematic = true;


        StartCoroutine(controller.DisablePlayer(velocity));
        StartCoroutine(cam.LerpPosition(controller.GetComponentInChildren<Rigidbody>().transform));
        StartCoroutine(FailStateReset());
    }
    
    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public IEnumerator FailStateReset()
    {
        yield return new WaitForSeconds(1.0f);
        scoreText.GetComponent<Animator>().Play("ScoreFailState");
        while(!Input.anyKey)
         yield return null;
        fade.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        LoadScene(1);
    }

    public IEnumerator Reset()
    {
        active = false;
        fade.GetComponent<Animator>().Play("FadeOut");
        yield return new WaitForSeconds(0.5f);
        LoadScene(1);
    }
}
