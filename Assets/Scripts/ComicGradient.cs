using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicGradient : MonoBehaviour
{
    [SerializeField]
    private GameObject[] comics;
    [SerializeField]
    private float[] delays;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Instance.MusicPlayStr("6");
        Invoke("part1", delays[0]);
        Invoke("part2", delays[1]);
        Invoke("part3", delays[2]);
        Invoke("part4", delays[3]);
        Invoke("part5", delays[4]);
        Invoke("part6", delays[5]);
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
        comics[4].GetComponent<Animator>().enabled = true;
    }
    void part6()
    {
        SoundManager.Instance.MusicPlayStr("1");
        SceneManager.LoadScene("StartScene");
    }
}
