using DummyCommercialApp.Models;
using System;
using System.Collections.Generic;

namespace DummyCommercialApp.AccountingStrategies
{
    internal class AccountingStrategyFactory : IAccountingStrategyFactory
    {
        private Dictionary<(ModesOfTransportation transportationModal, MovementTypes movement), IAccountingStrategy> ConfiguredMappings = new Dictionary<(ModesOfTransportation transportationModal, MovementTypes movement), IAccountingStrategy>();

        public void ConfigureDefaultStrategy(IAccountingStrategy defaultAccountingStrategy)
        {
            foreach (var transportationModal in Enum.GetValues<ModesOfTransportation>())
            {
                foreach (var movement in Enum.GetValues<MovementTypes>())
                {
                    if(ConfiguredMappings.ContainsKey((transportationModal, movement)) == false)
                    {
                        ConfiguredMappings.Add((transportationModal, movement), defaultAccountingStrategy);
                    }
                }
            }
        }

        public void ConfigureStrategy(ModesOfTransportation transportationModal, MovementTypes movement, IAccountingStrategy accountingStrategy)
        {
            if (ConfiguredMappings.ContainsKey((transportationModal, movement)) == false)
            {
                ConfiguredMappings.Add((transportationModal, movement), accountingStrategy);
            }
            else
            {
                ConfiguredMappings[(transportationModal, movement)] = accountingStrategy;
            }
        }

        public IAccountingStrategy GetStrategy(ModesOfTransportation transportationModal, MovementTypes movement)
        {
            if (ConfiguredMappings.ContainsKey((transportationModal, movement)) == false) throw new InvalidOperationException($"The accounting strategy for {transportationModal} and {movement} is not configured.");
            return ConfiguredMappings[(transportationModal, movement)];
        }
    }
}
