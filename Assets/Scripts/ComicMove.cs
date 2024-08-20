using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicMove : MonoBehaviour
{
    [SerializeField]
    private GameObject[] comics;
    [SerializeField]
    private float[] delays;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("part1", delays[0]);
        Invoke("part2", delays[1]);
        Invoke("part3", delays[2]);
        Invoke("part4", delays[3]);
        Invoke("part5", delays[4]);
    }
    void part1()
    {
        comics[0].GetComponent<Animator>().enabled = true;
    }
    void part2()
    {
        comics[1].GetComponent<Animator>().enabled = true;
    }
    void part3()
    {
        comics[2].GetComponent<Animator>().enabled = true;
    }
    void part4()
    {
        comics[3].GetComponent<Animator>().enabled = true;
    }
    void part5()
    {
        SceneManager.LoadScene("Level1");
    }
}
