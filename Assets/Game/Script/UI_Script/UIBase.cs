using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class UIBase : MonoBehaviour
{
    [Header("�t�F�[�h")]
    public GameObject fadeImage; //�F�����ɐݒ肵��Image
    public Image fadeColor;      //fadeImage�̐F���
    public float startCount;     //�Q�[���J�n�܂ł̃J�E���g

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //�V�[���J�n���ɐ���
        GameObject createFade = Instantiate(fadeImage, transform);
        fadeColor = GameObject.Find("FadeImage(Clone)").GetComponent<Image>();
        StartCoroutine(StartGame());

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected IEnumerator StartGame()
    {
        //�t�F�[�h�p�^�C�}�[
        float fadeTimer = 0.0f;
        //�F�����擾
        Color color = fadeColor.color;

        while (fadeTimer < startCount)
        {
            //���X��fadeImage�̓����x���グ��
            color.a = Mathf.Lerp(1.0f, 0.0f, fadeTimer / startCount);
            fadeColor.color = color;
            //���Ԍv��
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        //���S�ɓ����ɂ���
        color.a = 0.0f;
        fadeColor.color = color;
    }
}
