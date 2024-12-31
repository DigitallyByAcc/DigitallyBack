using DigitalyAPI.Data;
using DigitalyAPI.Repositories.Implementation;
using DigitalyAPI.Repositories.Interface;

namespace DigitalyAPI.Services.Service
{
    public class ImpayeService
    {
        private readonly IImpayeRepository _impayeRepository;

        public ImpayeService(IImpayeRepository impayeRepository)
        {
            _impayeRepository = impayeRepository;
        }
        public async Task<int> GetNombreImpayes(int clientId)
        {
            return await _impayeRepository.GetNombreImpayesAsync(clientId);
        }
    }
}
