namespace BlueFoxGloveAPI.Services
{
    public class ProjectileCreationException: Exception
    {
        public ProjectileCreationException()
        {

        }

        public ProjectileCreationException(string message)
            : base(message)
        {

        }

        public ProjectileCreationException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}