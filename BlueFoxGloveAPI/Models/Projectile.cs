namespace BlueFoxGloveAPI.Models
{
    public class Projectile
    {
        public int ProjectileId { get; set; }
        public string PlayerId { get; set; }
        public int Speed { get; set; }
        public Vector Velocity { get; set; }
        public Vector Position { get; set; }
    }
}