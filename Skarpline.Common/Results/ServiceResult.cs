using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace Skarpline.Common.Results
{
    /// <summary>
    /// Wrapper for responses from Business Layer
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// Determines if request succeeded
        /// </summary>
        public virtual bool Succeeded => Errors == null || !Errors.Any();

        /// <summary>
        /// List of errors if any
        /// </summary>
        public IEnumerable<string> Errors { get; set; }

        public ServiceResult()
        {
        }

        public ServiceResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public ServiceResult(string error)
            : this(new[] { error })
        {
        }

        public IdentityResult ToIdentityResult()
        {
            return Errors != null ? new IdentityResult(Errors) : new IdentityResult();
        }

        public string ErrorMessage
        {
            get
            {
                if (Errors != null && Errors.Any())
                {
                    return string.Join("; ", Errors.ToArray());
                }
                return "Service error";
            }
        }
    }

    /// <summary>
    /// Generic Wrapper for responses from Business Layer
    /// </summary>
    /// <typeparam name="T">Type of expected result</typeparam>
    public class ServiceResult<T> : ServiceResult where T : class
    {
        public ServiceResult() { }

        public ServiceResult(T result)
        {
            Result = result;
            if (typeof(T) != typeof(IdentityResult) || result == null) return;
            var identityResult = result as IdentityResult;
            if (identityResult == null) return;
            Errors = identityResult.Errors;
        }

        public ServiceResult(IEnumerable<string> errors) : base(errors) { }

        public ServiceResult(string error) : base(error) { }

        /// <summary>
        /// Expected Result
        /// </summary>
        public T Result { get; set; }

        public override bool Succeeded => Result != null && base.Succeeded;
    }
}
