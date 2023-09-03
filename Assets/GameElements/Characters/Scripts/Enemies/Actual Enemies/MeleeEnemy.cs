namespace Characters.Enemies
{
    public abstract class MeleeEnemy : Enemy
    {
        public override void Attack()
        {
            base.Attack();
            Player.Instance.TakeDamage(Damage);
        }
    }
}
