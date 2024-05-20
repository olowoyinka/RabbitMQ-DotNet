using FormulaAirlineAPI.Models;
using FormulaAirlineAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace FormulaAirlineAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
        private readonly IMessageProducer _messageProducer;

        public static readonly List<Booking> _bookings = new();

        public BookingController(ILogger<BookingController> logger,
                                  IMessageProducer messageProducer)
        {
            _logger = logger;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public IActionResult CreatingBooking(Booking newBooking)
        {
            if(!ModelState.IsValid) return BadRequest();

            _bookings.Add(newBooking);

            _messageProducer.SendingMessage<Booking>(newBooking);

            return Ok();
        }
    }
}
