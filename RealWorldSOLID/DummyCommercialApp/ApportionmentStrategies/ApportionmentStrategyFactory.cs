using DummyCommercialApp.Models;
using System;

namespace DummyCommercialApp.ApportionmentStrategies
{
    internal class ApportionmentStrategyFactory : IApportionmentStrategyFactory
    {
        public IApportionmentStrategy GetStrategy(ModesOfTransportation transportationModal, MovementTypes movement)
        {
            switch (transportationModal)
            {
                case ModesOfTransportation.NotDefined:
                    throw new InvalidOperationException($"The value of {nameof(transportationModal)} is {transportationModal}.");
                case ModesOfTransportation.Road:
                    return new RoadTransportedApportionment();
                case ModesOfTransportation.Rail:
                    return new RailTransportedApportionment();
                case ModesOfTransportation.Water:
                    return new WaterTransportedApportionment();
                case ModesOfTransportation.Pipe:
                    return new PipeTransportedApportionment();
                default:
                    throw new NotImplementedException($"The method {System.Reflection.MethodBase.GetCurrentMethod().Name} does not implement support for {transportationModal}.");
            }
        }
    }
}
