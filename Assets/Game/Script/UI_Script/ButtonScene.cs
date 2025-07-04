using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScene : ButtonBase
{
    [Header("�V�[����")]
    public string sceneName;

    [Header("�x��")]
    public float delay; //�V�[���ړ�����܂ł̒x��

    [Header("�t�F�[�h")]
    public Image fadeImage; //���F��Image��ݒ�

    public void ButtonClick()
    {
        //fadeImage���擾
        fadeImage = GameObject.Find("FadeImage(Clone)").GetComponent<Image>();
        //�������s
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        //�t�F�[�h�p�^�C�}�[
        float fadeTimer = 0.0f;
        //�F�����擾
        Color color = fadeImage.color;

        while (fadeTimer<delay)
        {
            //���X��fadeImage�̓����x��������
            color.a = Mathf.Lerp(0.0f, 1.0f, fadeTimer / delay);
            fadeImage.color = color;
            //���Ԍv��
            fadeTimer += Time.deltaTime;
            yield return null;
        }

        //���S�ɍ��ɂ���
        color.a = 1.0f;
        fadeImage.color = color;

        //�V�[���ړ�
        SceneManager.LoadScene(sceneName);
    }
}
