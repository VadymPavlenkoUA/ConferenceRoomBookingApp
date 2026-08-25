using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceRoomBooking.Application.Interfaces.Services
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
