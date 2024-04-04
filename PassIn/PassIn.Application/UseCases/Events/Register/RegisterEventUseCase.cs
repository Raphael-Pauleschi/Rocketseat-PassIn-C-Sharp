using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register
{
    public class RegisterEventUseCase
    {
        public ResponseRegisteredJson Execute(RequestEventJson request)
        {
            Validate(request);
            var dbContext = new PassInDbContext();

            var entityEvent = new Infrastructure.Entities.Event
            {
                Title = request.Title,
                Details = request.Details,
                Maximum_Attendees = request.MaximumAttendees,
                Slug = request.Title.ToLower().Replace(" ","-"),
            };

            dbContext.Events.Add(entityEvent);
            dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                id = entityEvent.Id,
            };
        }

        private void Validate(RequestEventJson request)
        {
            if (request.MaximumAttendees <= 0)
            {
                throw new ErrorOnValidationException("The Maximum attendees is invalid");
            }

            if (string.IsNullOrWhiteSpace(request.Details))
            {
                throw new ErrorOnValidationException("The details can't be null or only white space");

            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                throw new ErrorOnValidationException("The title can't be null or only white space");
            }
        }
    }
}
