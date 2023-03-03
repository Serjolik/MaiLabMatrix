public class Singleton<T> where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get => instance;
    }

    public static bool IsInstantiated
    {
        get => instance != null;
    }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Console.WriteLine("ERROR instance is created");
        }
        else
        {
            instance = (T)this;
        }
    }
}