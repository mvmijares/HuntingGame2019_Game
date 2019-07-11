using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/* Created By: Michael-Vincent Mijares
 * Creation Date: 26/06/2019
 */

public enum FadeType { None, In, Out }
public class FadeScreen : MonoBehaviour
{
    public Image fadeImage;
    private GameManager _gameManager;
    FadeType fade;
    public float speed; //Speed of the fade in.
    private float alpha;
    public void InitializeFadeScreenBar(GameManager manager)
    {
        _gameManager = manager;
        fade = FadeType.None;
    }
    private void Update()
    {
        if(fade != FadeType.None)
        {
            if (fade == FadeType.In)
            {
                alpha += Time.deltaTime * speed;
                if (alpha > 1.0f)
                {
                    alpha = 1.0f;
                    fade = FadeType.None;
                }
            }
            else if(fade == FadeType.Out)
            {
                alpha -= Time.deltaTime * speed;
                if (alpha < 0.0f)
                {
                    alpha = 0.0f;
                    fade = FadeType.None;
                }
            }
            SetColor(alpha);
        }
    }

    public void FadeEvent(FadeType fade)
    {
        this.fade = fade;
        if(fade != FadeType.None)
        {
            if(fade == FadeType.In)
            {
                alpha = 0.0f; // starting alpha is transparent
            }
            else
            {
                alpha = 1.0f; // starting alpha is colored
            }
        }
        Color color = fadeImage.color;
        fadeImage.color = new Color(color.r, color.g, color.b, alpha);
    }

    private void SetColor(float alpha)
    {
        Color color = fadeImage.color;
        fadeImage.color = new Color(color.r, color.g, color.b, alpha);
    }


}
