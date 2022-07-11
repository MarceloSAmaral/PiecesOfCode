using DummyCommercialApp.Models;

namespace DummyCommercialApp.AccountingStrategies
{
    public interface IAccountingStrategyFactory
    {
        IAccountingStrategy GetStrategy(ModesOfTransportation transportationModal, MovementTypes movement);
    }
}
