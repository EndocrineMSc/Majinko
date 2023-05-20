namespace Characters.Enemies
{
    internal abstract class MeleeEnemy : Enemy
    {
        internal override void Attack()
        {
            base.Attack();
            Player.Instance.TakeDamage(Damage);
        }
    }
}
