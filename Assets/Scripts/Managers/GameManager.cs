using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int playerCount = 0;
    private GameObject leftSide;
    private GameObject rightSide;
    public GameObject ball;
    private GameObject countDown;
    private TextMeshProUGUI countDownText;
    private bool countdownStarted = false;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        countDown = GameObject.Find("StartTimer");
        if (countDown == null)
        {
            countDown = GameObject.FindObjectsOfType<GameObject>(true)
                    .FirstOrDefault(o => o.name == "StartTimer");
        }
        countDownText = countDown.GetComponent<TextMeshProUGUI>();
        leftSide = GameObject.Find("LeftSide");
        rightSide = GameObject.Find("RightSide");
    }

    private void Update()
    {
        if (playerCount == 2 && !countdownStarted)
        {
            countdownStarted = true;
            StartCoroutine(CountDown());
        }
    }

    IEnumerator CountDown()
    {
        int count = 3;

        while (count > 0)
        {
            countDownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countDownText.text = "GO!";

        Destroy(leftSide);
        Destroy(rightSide);
        Instantiate(ball, new Vector3(0, 0, 0), Quaternion.identity);

        StartCoroutine(ClearText());
    }

    IEnumerator ClearText()
    {
        yield return new WaitForSeconds(1f);
        countDownText.text = "";
    }
}
