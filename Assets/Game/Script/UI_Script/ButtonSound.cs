using UnityEngine;

public class ButtonSound : ButtonBase
{
    [Header("音量調整パネル")]
    public GameObject sound;

    protected override void Start()
    {
        base.Start();

        //初期設定
        //パネルを非表示
        sound.SetActive(false);
    }

    //パネルを表示
    public void OpenSoundPanel()
    {
        sound.SetActive(true);
    }

    //パネルを非表示
    public void CloseSoundPanel()
    {
        sound.SetActive(false);
    }
}
