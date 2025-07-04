using UnityEngine;
using System.Collections;

public class ButtonEnd : ButtonBase
{
    [Header("�x��")]
    public float delay;

    public void ButtonClick()
    {
        StartCoroutine(LoadEnd());

    }

    private IEnumerator LoadEnd()
    {
        yield return new WaitForSeconds(delay);

        //�I������
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();//�Q�[���v���C�I��
#endif
        }
    }
}
