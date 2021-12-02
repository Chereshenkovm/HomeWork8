namespace Game
{
    public class BulletState
    {
        private float _lifeTimer;

        public BulletState(float lifeTime)
        {
            _lifeTimer = lifeTime;
        }

        public bool IsAlive => _lifeTimer > 0;
        
        public void Tick(float deltaTime)
        {
            _lifeTimer -= deltaTime;
        }
    }
}