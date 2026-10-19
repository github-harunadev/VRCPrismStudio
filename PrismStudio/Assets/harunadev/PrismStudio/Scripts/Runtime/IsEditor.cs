public static class IsEditor
{
    public readonly static bool isEditor = 
#if UNITY_EDITOR
                true;
#else
                false;
#endif
}
