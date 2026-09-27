using FanHubPlus.Models;

namespace FanHubPlus.ViewModels
{
    public class NearbyEventViewModel
    {
        public Event Event { get; set; } = null!;
        public double? DistanceKm { get; set; }
    }
}
