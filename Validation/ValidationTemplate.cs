using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

public class ValidationTemplate :
    IDataErrorInfo,
    INotifyDataErrorInfo
{
    INotifyPropertyChanged target;
    ValidationContext validationContext;
    List<ValidationResult> validationResults;

    public ValidationTemplate(INotifyPropertyChanged target)
    {
        this.target = target;
        validationContext = new ValidationContext(target, null, null);
        validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(target, validationContext, validationResults, true);
        target.PropertyChanged += Validate;
    }

    void Validate(object sender, PropertyChangedEventArgs e)
    {
        validationResults.Clear();
        Validator.TryValidateObject(target, validationContext, validationResults, true);
        var hashSet = new HashSet<string>(validationResults.SelectMany(x => x.MemberNames));
        foreach (var error in hashSet)
        {
            RaiseErrorsChanged(error);
        }
    }

    public void AddError(string errorMessage)
    {
        validationResults.Add(new ValidationResult(errorMessage));
    }

    public void AddError(string errorMessage, string memberName)
    {
        validationResults.Add(new ValidationResult(errorMessage, new string[] { memberName }));
    }

    public void AddError(string errorMessage, IEnumerable<string> memberNames)
    {
        validationResults.Add(new ValidationResult(errorMessage, memberNames));
    }

    public IEnumerable GetErrors(string propertyName)
    {
        return validationResults.Where(x => x.MemberNames.Contains(propertyName))
                                .Select(x => x.ErrorMessage);
    }

    public bool HasErrors
    {
        get { return validationResults.Count > 0; }
    }

    public string Error
    {
        get
        {
            var strings = validationResults.Select(x => x.ErrorMessage)
                                            .ToArray();
            return string.Join(Environment.NewLine, strings);
        }
    }

    public string this[string propertyName]
    {
        get
        {
            var strings = validationResults.Where(x => x.MemberNames.Contains(propertyName))
                                            .Select(x => x.ErrorMessage)
                                            .ToArray();
            return string.Join(Environment.NewLine, strings);
        }
    }

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    void RaiseErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
}
