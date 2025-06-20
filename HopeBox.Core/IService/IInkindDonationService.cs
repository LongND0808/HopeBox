using HopeBox.Domain.DTOs;
using HopeBox.Domain.Models;

namespace HopeBox.Core.IService
{
    public interface IInkindDonationService : IBaseService<InkindDonation, InkindDonationDto>
    {
        /// <summary>
        /// Retrieve a list of all in-kind donations for a specific user.
        /// </summary>
        /// <returns>A list of in-kind donation donated by user</returns>


        /// <summary>
        /// Create in-kind donation by user
        /// </summary>
        /// <param name="donation">The in-kind donation to create.</param>
        /// <returns>The updated in-kind donation.</returns>


        /// <summary>
        /// Cancel the donation 
        /// </summary>
        /// <param name="id">The ID of the in-kind donation to cancel by owner-user.</param>

    }
}
