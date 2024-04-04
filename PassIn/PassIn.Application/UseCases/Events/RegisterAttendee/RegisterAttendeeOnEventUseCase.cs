using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee
{
    public class RegisterAttendeeOnEventUseCase
    {
       private readonly PassInDbContext _dbContext;
       
       public RegisterAttendeeOnEventUseCase()
        {
            _dbContext = new PassInDbContext();
        } 

        public ResponseRegisteredJson Execute(RequestRegisterEventJson request, Guid eventId)
        {
            Validate(eventId,request);
            var entity = new Infrastructure.Entities.Attendee
            {
                Email = request.Email,
                Name = request.Name,
                Event_id = eventId,
                Created_At = DateTime.UtcNow,   
            };

            _dbContext.Attendees.Add(entity);
            _dbContext.SaveChanges();

            return new ResponseRegisteredJson
            {
                id = entity.Id,
            };

        }

        private void Validate(Guid eventId, RequestRegisterEventJson request)
        {
            var eventEntity = _dbContext.Events.Find(eventId);
            if (eventEntity is null) {
                throw new NotFoundException("An event with this id does not exist");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ErrorOnValidationException("The name is invalid");
            }
            if (!EmailIsValid(request.Email))
            {
                throw new ErrorOnValidationException("The e-mail is invalid");
            }

            var attendeeAlrealdyRegistered = _dbContext
                .Attendees
                .Any(attendee =>  
                    attendee.Email.Equals(request.Email) 
                    && attendee.Event_id == eventId);

            if (attendeeAlrealdyRegistered) {
                throw new ConflictException("You can't register twice in the same event");
            }

            var attendeesForThisEvent = _dbContext.Attendees.Count(attendee=> attendee.Event_id == eventId);
            

            if(attendeesForThisEvent >= eventEntity.Maximum_Attendees)
            {
                throw new ErrorOnValidationException("There is no room left for this event");
            }

        }

        private bool EmailIsValid(string email)
        {
            try
            {
                new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

       
}
