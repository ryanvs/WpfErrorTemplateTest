using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfErrorTemplateTest.ViewModels
{
    public class MainViewModel : ObservableObject, IDataErrorInfo, INotifyDataErrorInfo
    {
        public MainViewModel()
        {
            _title = "Error Template Test";
            _description = "Sample Message is too long...";

            // Trigger validation
            _validationTemplate = new ValidationTemplate(this);
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetField(ref _title, value); }
        }

        private string _description;

        [MinLength(4, ErrorMessage = "Description must be at least 4 characters")]
        [MaxLength(16, ErrorMessage = "Description cannot be longer than 16 characters")]
        public string Description
        {
            get { return _description; }
            set { SetField(ref _description, value); }
        }

        private bool? _isDeployed;

        [Required]
        public bool? IsDeployed
        {
            get { return _isDeployed; }
            set { SetField(ref _isDeployed, value); }
        }

        #region Validation
        private ValidationTemplate _validationTemplate;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { _validationTemplate.ErrorsChanged += value; }
            remove { _validationTemplate.ErrorsChanged -= value; }
        }

        public string Error
        {
            get { return _validationTemplate.Error; }
        }

        public bool HasErrors
        {
            get { return _validationTemplate.HasErrors; }
        }

        public string this[string columnName]
        {
            get { return _validationTemplate[columnName]; }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return _validationTemplate.GetErrors(propertyName);
        }

        #endregion
    }
}
