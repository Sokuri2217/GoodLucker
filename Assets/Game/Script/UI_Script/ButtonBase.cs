using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("�{�^���̊g�嗦")]
    public Vector3 originScale; //���l
    public float zoom;          //�g�嗦
    [Header("SE")]
    public AudioClip onCursor; //�J�[�\�����{�^���Əd�Ȃ�����
    public AudioClip isClick; //�{�^�����N���b�N������
    [Header("�X�N���v�g�Q��")]
    public SEManager seManager; //SE

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //�X�N���v�g�����擾
        seManager = GameObject.Find("SEManager").GetComponent<SEManager>();
        //�����l�̕ۑ�
        originScale = transform.localScale;
    }

    // �}�E�X���{�^���ɓ�������
    public void OnPointerEnter(PointerEventData eventData)
    {
        //�{�^�����g�傷��
        transform.localScale = originScale * zoom;
        //SE
        seManager.seSource.PlayOneShot(onCursor);
    }

    // �}�E�X���{�^�����痣�ꂽ��
    public void OnPointerExit(PointerEventData eventData)
    {
        //�{�^���̃T�C�Y�����ɖ߂�
        transform.localScale = originScale;
    }

    //�N���b�N�����Ƃ��̋���
    public void OnClick()
    {
        Button button = GetComponent<Button>();
        button.interactable = false;
    }
    public void ClickSE()
    {
        //SE
        seManager.seSource.PlayOneShot(isClick);
    }
}
