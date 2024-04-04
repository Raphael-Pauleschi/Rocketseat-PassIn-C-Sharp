using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById
{
    public class GetEventbyIdUseCase
    {
        public ResponseEventJson Execute(Guid id)
        {
            var dbContext = new PassInDbContext();

            //dbContext.Events.FirstOrDefault(ev => ev.Id == id);

            var entityEvent = dbContext.Events
                .Include(ev => ev.Attendees)
                .FirstOrDefault(ev => ev.Id == id);

            if (entityEvent is null)
            {
                throw new NotFoundException("An event with this Id doesn't exist");
            }

            return new ResponseEventJson
            {
                Id = entityEvent.Id,
                Details = entityEvent.Details,
                MaximumAttendees = entityEvent.Maximum_Attendees,
                Title = entityEvent.Title,
                AttendeesAmount = entityEvent.Attendees.Count(),
            };
        }
      
    }
}
