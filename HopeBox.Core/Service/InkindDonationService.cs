using HopeBox.Core.IService;
using HopeBox.Domain.Converter;
using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;
using HopeBox.Infrastructure.Repository;

namespace HopeBox.Core.Service
{
    public class InkindDonationService : BaseService<InkindDonation, InkindDonationDto>, IInkindDonationService
    {
        private readonly IRepository<InkindDonation> _inkindDonationRepository;
        public InkindDonationService(
            IRepository<InkindDonation> inkindDonationRepository,
            IConverter<InkindDonation, InkindDonationDto> converter)
        : base(inkindDonationRepository, converter)
        {
            _inkindDonationRepository = inkindDonationRepository;
        }


    }
}
