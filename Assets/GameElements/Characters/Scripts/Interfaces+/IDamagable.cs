
namespace PeggleWars.Characters.Interfaces
{
    public interface IDamagable
    {
        public void TakeDamage(int damage, bool sourceIsAttack = true);
    }
}
