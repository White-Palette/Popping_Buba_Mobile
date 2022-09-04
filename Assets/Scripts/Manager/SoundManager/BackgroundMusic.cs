public class BackgroundMusic
{
    public static void Play(Music music)
    {
        if (music == Music.None)
            return;
        SoundManager.Instance.PlaySound(music);
    }
}