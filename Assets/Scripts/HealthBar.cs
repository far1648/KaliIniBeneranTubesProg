using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    //BEHOLD BILION IF ELSE STATEMENT 
    [SerializeField] Player player;
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHealth;
    [SerializeField] Sprite emptyHealth;
    [SerializeField] Image flashOverlay;
    [SerializeField] float flashDuration = 0.25f;
    [SerializeField] float flashAlpha = 0.35f;
    int lastHealth = -1;
    Coroutine flashRoutine;
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        int hp = player.HP;
        if (hp == lastHealth) return;

        Refresh(hp);

        if (lastHealth >= 0)
        {
            Color c = hp < lastHealth ? Color.red : Color.green;
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(Flash(c));
        }
        lastHealth = hp;
    }

    void Refresh(int hp)
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].sprite = i < hp ? fullHealth : emptyHealth;
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
