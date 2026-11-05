using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StarUI : MonoBehaviour
{
    //BEHOLD BILION IF ELSE STATEMENT 
    [SerializeField] Player player;
    [SerializeField] Image[] starImage;
    [SerializeField] Sprite fullStar;
    [SerializeField] Sprite emptyStar;
    [SerializeField] Image flashOverlay;
    [SerializeField] Color flashColor = new Color(1f, 0.9f, 0.2f);
    [SerializeField] float flashDuration = 0.25f;
    [SerializeField] float flashAlpha = 0.35f;
    Coroutine flashRoutine;
    int lastStar = -1;
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        int star = player.starCount;
        if (star == lastStar) return;
        
        Refresh(star);

        if (lastStar >= 0  && star > lastStar)
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(Flash(flashColor));
        }        

        lastStar = star;
    }

    void Refresh(int star)
    {
        for (int i = 0; i < starImage.Length; i++)
            starImage[i].sprite = i < star ? fullStar : emptyStar;
    }

    IEnumerator Flash(Color c)
    {
        float t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(flashAlpha, 0f, t / flashDuration);
            flashOverlay.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        flashOverlay.color = new Color(c.r, c.g, c.b, 0f);
    }
}
