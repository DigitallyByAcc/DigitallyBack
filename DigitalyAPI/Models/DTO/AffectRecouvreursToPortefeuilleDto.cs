namespace DigitalyAPI.Models.DTO
{
    public class AffectRecouvreursToPortefeuilleDto
    {
        public int PortefeuilleId { get; set; }
        public List<int> RecouvreursIds { get; set; }
    }
}
