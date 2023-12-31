using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class Pit : MonoBehaviour
{

    [Tooltip("The number of stones currently in the pit")]
    public int numberOfStones = 0;

    public GameObject centerOfPit;
    public List<GameObject> stonePrefabs;

    public List<GameObject> renderedStones;

    public float initSpawnInterval = 1f;

    public TextMeshProUGUI numStonesText;

    public AudioSource stonePopSfx;

    public AudioSource stoneClinkSfx;

    // Start is called before the first frame update
    void Start()
    {
        renderedStones = new List<GameObject>();
        numberOfStones = renderedStones.Count;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void Init()
    {
        for (float i = 0; i < initSpawnInterval; i = i += (initSpawnInterval/4))
        {
            StartCoroutine(DelayedSpawn(i));
        }

    }

    IEnumerator DelayedSpawn(float delay)
    {
        yield return new WaitForSeconds(delay);
        AddStone();
    }

    public void DelayedAddStone(float delay)
    {
        StartCoroutine(DelayHelperAddStone(delay));
    }
    public IEnumerator DelayHelperAddStone(float delay)
    {
        yield return new WaitForSeconds(delay);
        AddStone();
    }

    public void AddStone()
    {
        stonePopSfx.PlayOneShot(stonePopSfx.clip);
        StartCoroutine(DelayStoneClinkSfx(1.25f));
        numberOfStones++;
        renderedStones.Add(Instantiate(stonePrefabs[Random.Range(0,stonePrefabs.Count)], transform));
        numStonesText.text = numberOfStones.ToString();
    }

    IEnumerator DelayStoneClinkSfx(float delay)
    {
        yield return new WaitForSeconds(delay);
        stoneClinkSfx.PlayOneShot(stoneClinkSfx.clip);
    }

    
    public void DelayedClearPit(float delay)
    {
        StartCoroutine(DelayHelperClearPit(delay));
    }

    public IEnumerator DelayHelperClearPit(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearPit();
    }
    public void ClearPit()
    {
        foreach(GameObject stone in renderedStones)
        {
            Destroy(stone);
            numberOfStones--;
        }
        renderedStones = new List<GameObject>();
        numStonesText.text = "0";
    }
}
