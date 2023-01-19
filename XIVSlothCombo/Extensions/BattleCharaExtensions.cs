using Dalamud.Game.ClientState.Objects.Types;

namespace XIVSlothCombo.Extensions
{
    internal static class BattleCharaExtensions
    {
        public unsafe static uint RawShieldValue(this BattleChara chara)
        {
            FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara* baseVal = (FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara*)chara.Address;
            byte value = baseVal->Character.ShieldValue;
            uint rawValue = chara.MaxHp / 100 * value;

            return rawValue;
        }

        public unsafe static byte ShieldPercentage(this BattleChara chara)
        {
            FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara* baseVal = (FFXIVClientStructs.FFXIV.Client.Game.Character.BattleChara*)chara.Address;
            byte value = baseVal->Character.ShieldValue;

            return value;
        }

        public static bool HasShield(this BattleChara chara) => chara.RawShieldValue() > 0;
    }
}
