using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject pintuBuka;
    [SerializeField] GameObject pintuTutup;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }
        if (pintuBuka == null)
        {
            pintuBuka = GameObject.Find("OpenDoor");
        }
        if (pintuTutup == null)
        {
            pintuTutup = GameObject.Find("ClosedDoor");
        }
    }

    void Update()
    {
        if (player.starCount == 3)
        {
            pintuBuka.SetActive(true);
            pintuTutup.SetActive(false);
        }
    }
}
