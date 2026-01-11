namespace GeneticGame;

public readonly record struct UnitGenetics(
    double BirthModifier,
    double EatModifier,
    double FightModifier,
    double BaseDamage,
    double ArmorPercent,
    double MaxHealth,
    double MaxEnergy
)
{
    public UnitGenetics GetInitializedRandomGenetics()
    {
        double Rnd() => GameSettings.GetPercentOfInitGeneticsRandomization();

        return new UnitGenetics
        {
            BirthModifier = this.BirthModifier * Rnd(),
            EatModifier   = this.EatModifier * Rnd(),
            FightModifier = this.FightModifier * Rnd(),
            BaseDamage    = this.BaseDamage * Rnd(),
            ArmorPercent  = this.ArmorPercent * Rnd(),
            MaxHealth     = this.MaxHealth * Rnd(),
            MaxEnergy     = this.MaxEnergy * Rnd()
        };
    }
    public static UnitGenetics MergeGenetics(UnitGenetics g1, UnitGenetics g2) => new(
        //formula (genetic1+genetic2)/2 * randomizationFactor
        (g1.BirthModifier + g2.BirthModifier)/2 * GameSettings.GetPercentOfInitGeneticsRandomization(),
         (g1.EatModifier + g2.EatModifier)/2     * GameSettings.GetPercentOfInitGeneticsRandomization(),
        (g1.FightModifier + g2.FightModifier)/2 * GameSettings.GetPercentOfInitGeneticsRandomization(),
        (g1.BaseDamage + g2.BaseDamage)/2       * GameSettings.GetPercentOfInitGeneticsRandomization(),
        (g1.ArmorPercent + g2.ArmorPercent)/2   * GameSettings.GetPercentOfInitGeneticsRandomization(),
          (g1.MaxHealth + g2.MaxHealth)/2         * GameSettings.GetPercentOfInitGeneticsRandomization(),
         (g1.MaxEnergy + g2.MaxEnergy)/2         * GameSettings.GetPercentOfInitGeneticsRandomization()
    );
}