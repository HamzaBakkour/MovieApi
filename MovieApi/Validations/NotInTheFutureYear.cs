using System.ComponentModel.DataAnnotations;


namespace MovieApi.Validations;

public class NotInTheFutureYear : ValidationAttribute
{
    private readonly int _fromYear;

    public NotInTheFutureYear(int fromYear)
    {
        this._fromYear = fromYear;
    }


    public override bool IsValid(object? value)
    {

        int year;

        if (value is string stringInput)
        {
            stringInput = stringInput.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Last();
            if (!int.TryParse(stringInput, out year))
                return false;
        }
        else if (value is int intInput)
        {
            year = intInput;
        }
        else {
            return false;
        }

        int currentYear = DateTime.Now.Year;
        return year >= _fromYear && year <= currentYear;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The year must be between {_fromYear} and {DateTime.Now.Year}.";
    }
}
