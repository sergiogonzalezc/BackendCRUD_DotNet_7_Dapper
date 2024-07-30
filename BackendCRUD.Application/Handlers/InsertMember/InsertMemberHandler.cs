using BackendCRUD.Application.Commands;
using BackendCRUD.Application.Interface;
using BackendCRUD.Application.Model;
using BackendCRUD.Application.Querys;
using BackendCRUD.Application.CQRS;
using FluentValidation;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using FluentValidation.Results;

namespace BackendCRUD.Application.Handlers.InsertMember
{

    public class InsertMemberHandlerValidationValidator: AbstractValidator<InsertMemberCommand>
    {
        public InsertMemberHandlerValidationValidator()
        {
            RuleFor(x => x.input.name).NotEmpty().WithMessage("Valor name requerido!!!"); //lanza error si es nulo o vacio el valor
            RuleFor(x => x.input.name.Trim()).MaximumLength(50).WithMessage("Valor es muy grande");  //lanza error si el valor es mayor
            RuleFor(x => x.input.type).NotEmpty().WithMessage("Valor type requerido!!!");
            RuleFor(x => x.input.salary_per_year).LessThan(10).WithMessage("Valor debe ser menor a 10!!!"); // lanza error si el valor es mayor o igual al valor especificado
            RuleFor(x => x.input.salary_per_year).GreaterThan(0).WithMessage("Valor debe ser mayor que 0!!!"); // lanza error si el valor es menor o igual al valor especificado
        }
    }

    internal class InsertMemberHandler : ICommandHandler<InsertMemberCommand, ResultRequestDTO>
    {
        private readonly IMemberApplication _MembersService;
        

        public InsertMemberHandler(IMemberApplication MembersApplication)
        {
            _MembersService = MembersApplication;
        }

        public async Task<ResultRequestDTO> Handle(InsertMemberCommand request, CancellationToken cancellationToken)
        {
            //Se recorren todos y solo cuando terminen los despliega
            //var validationFailures = await Task.WhenAll(_validadorFluent.Select(validator => validator.ValidateAsync(request, cancellationToken)));
            //var errors = validationFailures.Errors.Select(x => x.ErrorMessage).ToList();
            //if (errors.Any())
            //{
            //    throw new ValidationException(errors.FirstOrDefault());
            //}

            //var errors = validationFailures
            //                .Where(validationResult => !validationResult.IsValid)
            //                .SelectMany(validationResult => validationResult.Errors)
            //                .Select(validationFailure => new ValidationError(
            //                    validationFailure.PropertyName,
            //                    1,
            //                    validationFailure.ErrorMessage))
            //                .ToList();


            //if (errors.Any())
            //{
            //    //throw new Exceptions.ValidationException(errors);
            //    throw new ValidationException(errors.Select(x => x.Message).FirstOrDefault());
            //}

            return await _MembersService.InsertMember(request.input);
        }
    }

    //public class ValidationException : Exception
    //{
    //    public ValidationException()
    //        : base("Se han producido uno o más errores de validación.")
    //    {
    //        Errors = new Dictionary<string, string[]>();
    //    }

    //    public ValidationException(IEnumerable<ValidationFailure> failures)
    //        : this()
    //    {
    //        Errors = failures
    //            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
    //            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    //    }

    //    public IDictionary<string, string[]> Errors { get; }
    //}


    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base() { }
    }


    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
            : base($"No se encontró la entidad \"{name}\" ({key}).")
        {
        }
    }


    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int Code { get; set; }

        public string Message { get; }

        public ValidationError(string field, int code, string message)
        {
            Field = field != string.Empty ? field : null;
            Code = code != 0 ? code : 55;  //set the default code to 55. you can remove it or change it to 400.  
            Message = message;
        }
    }
}

