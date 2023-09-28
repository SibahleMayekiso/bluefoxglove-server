namespace BlueFoxGloveAPI.Services
{
    public class PlayerNotFoundExcpetion: Exception
    {
        public PlayerNotFoundExcpetion()
        {

        }

        public PlayerNotFoundExcpetion(string message)
            : base(message)
        {

        }

        public PlayerNotFoundExcpetion(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}