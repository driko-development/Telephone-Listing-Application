using System.Text.RegularExpressions;

namespace Telephone_Listing.Data
{
    public enum Type { PhoneNumber, Name, Invalid }

    public class InputValidator
    {
        private bool MatchRegularExpression(string pattern)
        {
            if (
                    Regex.Match(
                        Input,
                        pattern
                    )
                    .Success
                )
                    return true;
                else
                    return false;
        }

        private bool IsPhoneNumber
        {
            get
            {
                string pattern = @"\(?\d{3}\)?[-\.]? *\d{3}[-\.]? *[-\.]?\d{4}";
                return MatchRegularExpression(pattern);
            }
        }

        private bool IsProperName
        {
            get
            {
                string pattern = @"^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$";
                return MatchRegularExpression(pattern);
            }
        }

        public string Input { get; set; }

        public Type Validate()
        {
            if (string.IsNullOrEmpty(Input))
            {
                return Type.Invalid;
            }
            else if (IsPhoneNumber)
            {
                return Type.PhoneNumber;
            }
            else if (IsProperName)
            {
                return Type.Name;
            }
            else
            {
                return Type.Invalid;
            }
        }
    }
}
