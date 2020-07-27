using Exiled.API.Interfaces;

namespace CraftKnife
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}