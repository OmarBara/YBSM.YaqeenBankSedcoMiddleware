using FluentValidation;
using System.Globalization;
using YBSM.Core.Domain.ModelDTO.RequestDTO;

public class AddLypayTransRequestDtoValidator : AbstractValidator<LypayTransRequestDto>
{
    public AddLypayTransRequestDtoValidator()
    {
       /* RuleFor(x => x.HeaderSwitchModel.TargetSystemUserID)
            .NotEmpty()
            .WithState(_ => new { Code = "E1", Message = "Please check user id" });*/

        RuleFor(x => x.LookUpData.Details.RRN)
            .NotEmpty()
            //.Length(12)
            .Matches(@"^\d{12}$")
            .WithState(_ => new { Code = "E2", Message = "Please check RRN must be 12 digit and not include any characters" });

        RuleFor(x => x.LookUpData.Details.STAN)
            .Matches(@"^\d{6}$")
            .WithState(_ => new { Code = "E3", Message = "Please check STAN must be 6 digit and not include any characters" });

        RuleFor(x => x.LookUpData.Details.TXNAMT)
            .NotEmpty()
            .WithState(_ => new { Code = "E4", Message = "Please check TXNAMT" });

        RuleFor(x => x.LookUpData.Details.TERMID)
            .Matches(@"^\d{6,8}$")
            .WithState(_ => new { Code = "E5", Message = "Please check termid must be not less than 6 and not more then 8 digit and not include any special characters" });

        RuleFor(x => x.LookUpData.Details.SETLDATE)
            .Must(BeAValidDate)
            .WithState(_ => new { Code = "E6", Message = "Please check txn date" });

        RuleFor(x => x.LookUpData.Details.SETLDATE)
            .Matches(@"^\d{2}-\d{2}-\d{4}$")
            .WithState(_ => new { Code = "E7", Message = "Date format must be DD-MM-YYYY" });
    }

    private bool BeAValidDate(string date)
    {
        return DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}
