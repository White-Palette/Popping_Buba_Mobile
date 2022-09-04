public class SoundEffect
{
    public static void Play(Effect sound)
    {
        if (sound == Effect.None)
            return;
        SoundManager.Instance.PlaySound(sound);
    }
}