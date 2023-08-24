using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialsManager : MonoBehaviour
{
    [SerializeField]
    GameObject ADcorgi;
    [SerializeField]
    GameObject Shiftcorgi;
    [SerializeField]
    GameObject Jumpcorgi;
    [SerializeField]
    GameObject Click;

    Animator ADcorgianim;
    Animator Shiftcorgianim;
    Animator Jumpcorgianim;

    SpriteRenderer ClickSprite;
    [SerializeField]
    float blinkDuration = 0.5f;
    Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        ADcorgianim = ADcorgi.GetComponent<Animator>();
        Shiftcorgianim = Shiftcorgi.GetComponent<Animator>(); 
        Jumpcorgianim = Jumpcorgi.GetComponent<Animator>();

        ClickSprite = Click.GetComponent<SpriteRenderer>();

        StartCoroutine(BlinkRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            float scaleX = ADcorgi.transform.localScale.x;
            float scaleY = ADcorgi.transform.localScale.y;
            float scaleZ = ADcorgi.transform.localScale.z;

            if (scaleX < 0)
            {
                ADcorgi.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            }
            else
            {
                ADcorgi.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }

            ADcorgianim.SetBool("IsAorDKeyDown", true);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            float scaleX = ADcorgi.transform.localScale.x;
            float scaleY = ADcorgi.transform.localScale.y;
            float scaleZ = ADcorgi.transform.localScale.z;

            if (scaleX > 0)
            {
                ADcorgi.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            }
            else
            {
                ADcorgi.transform.localScale = new Vector3(-scaleX, scaleY, scaleZ);
            }

            ADcorgianim.SetBool("IsAorDKeyDown", true);
        }
        else
        {
            ADcorgianim.SetBool("IsAorDKeyDown", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {   
            Shiftcorgianim.SetBool("IsShiftKeyDown", true);
        }
        else
        {
            Shiftcorgianim.SetBool("IsShiftKeyDown", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumpcorgianim.SetBool("IsSpaceKeyDown", true);
        }
        else
        {
            Jumpcorgianim.SetBool("IsSpaceKeyDown", false);
        }
    }

    private IEnumerator BlinkRoutine()
    {
        Color originalColor = ClickSprite.color; // 초기 스프라이트 색상 저장

        while (true)
        {
            // 투명해지는 부분
            float elapsedTime = 0f;
            while (elapsedTime < blinkDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / (blinkDuration / 2));

                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1f, 0f, t));
                ClickSprite.color = newColor;

                yield return null;
            }

            // 불투명해지는 부분
            elapsedTime = 0f;
            while (elapsedTime < blinkDuration / 2)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / (blinkDuration / 2));

                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(0f, 1f, t));
                ClickSprite.color = newColor;

                yield return null;
            }
        }
    }

}
