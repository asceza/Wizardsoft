using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wizardsoft.DAL
{
    public class Result<T>
    {
        public T Value { get; }
        public string Error { get; }
        public bool IsSuccess { get; }

        private Result(T value, string error, bool isSuccess)
        {
            Value = value;
            Error = error;
            IsSuccess = isSuccess;
        }

        public static Result<T> Success(T value) => new Result<T>(value, null, true);
        public static Result<T> Failure(string error) => new Result<T>(default(T), error, false);
    }
}
