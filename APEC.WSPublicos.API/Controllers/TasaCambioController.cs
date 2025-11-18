using APEC.WS.Aplicacion.Interfaces;
using APEC.WS.Aplicacion.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace APEC_WS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasaCambioController : ControllerBase
    {
        private readonly ITasaCambioService _service;
        private readonly ILogger<TasaCambioController> _logger;

        public TasaCambioController(
            ITasaCambioService service,
            ILogger<TasaCambioController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{codigoMoneda}")]
        [ProducesResponseType(typeof(TasaCambioResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public IActionResult Get(
            [Required(ErrorMessage = "El código de moneda es obligatorio")]
            [StringLength(3, MinimumLength = 3, ErrorMessage = "El código debe tener exactamente 3 caracteres")]
            [RegularExpression(@"^[A-Z]+$", ErrorMessage = "Solo se permiten letras mayúsculas")]
            string codigoMoneda)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    _logger.LogWarning("Validación fallida para {CodigoMoneda}: {Errores}",
                        codigoMoneda, string.Join(", ", errors));

                    return BadRequest(new ErrorResponse(
                        "VALIDATION_ERROR",
                        "Error de validación en los parámetros de entrada",
                        errors));
                }

                decimal tasa = _service.ObtenerTasa(codigoMoneda);

                if (tasa == 0m)
                {
                    _logger.LogWarning("Moneda no encontrada: {CodigoMoneda}", codigoMoneda);
                    return NotFound(new ErrorResponse(
                        "CURRENCY_NOT_FOUND",
                        $"No se encontró tasa de cambio para la moneda {codigoMoneda}",
                        $"Códigos soportados: USD, EUR, etc."));
                }

                return Ok(new TasaCambioResponse
                {
                    Moneda = codigoMoneda,
                    Tasa = tasa,
                    FechaActualizacion = DateTime.UtcNow
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Error de argumento para {CodigoMoneda}", codigoMoneda);
                return BadRequest(new ErrorResponse(
                    "INVALID_ARGUMENT",
                    "Parámetro de entrada inválido",
                    ex.Message));
            }
            catch (Exception ex)
            {
                string errorId = Guid.NewGuid().ToString();
                _logger.LogError(ex, "Error {ErrorId} al obtener tasa para {CodigoMoneda}", errorId, codigoMoneda);
                return StatusCode(500, new ErrorResponse(
                    "SERVER_ERROR",
                    "Error interno del servidor",
                    $"Referencia del error: {errorId}"));
            }
        }

        public class TasaCambioResponse
        {
            public string Moneda { get; set; }
            public decimal Tasa { get; set; }
            public DateTime FechaActualizacion { get; set; }
        }

        public class ErrorResponse
        {
            public string ErrorCode { get; set; }
            public string Message { get; set; }
            public object Details { get; set; }

            public ErrorResponse(string errorCode, string message, object details)
            {
                ErrorCode = errorCode;
                Message = message;
                Details = details;
            }
        }
    }
}
