namespace Ingweland.Fog.Models.Fog.Entities;

public record BattleKey(string WorldId, byte[] InGameBattleId)
{
    public virtual bool Equals(BattleKey? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (other is null)
        {
            return false;
        }

        return WorldId == other.WorldId &&
               InGameBattleId.SequenceEqual(other.InGameBattleId);
    }

    public object[] ToCompositeKeyArray()
    {
        return [WorldId, InGameBattleId];
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = WorldId.GetHashCode();

            return InGameBattleId.Aggregate(hash, (current, b) => current * 31 + b);
        }
    }
};