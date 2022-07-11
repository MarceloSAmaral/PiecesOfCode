using DummyCommercialApp.Models;

namespace DummyCommercialApp.ApportionmentStrategies
{
    public interface IApportionmentStrategyFactory
    {
        IApportionmentStrategy GetStrategy(ModesOfTransportation transportationModal, MovementTypes movement);
    }
}
