using System;
using System.Collections.Generic;
using System.Text;

namespace UpdateModelLib
{
    public enum ErrorCode
    {
        OK,
        INVALID_MODEL,
        GLOBAL
    }

    public abstract class UpdateException : Exception
    {
        public UpdateException() { }

        public UpdateException(string message) : base(message) { }

        public UpdateException(ErrorCode errorCode)
        {
            this.ErrorCode = errorCode;
        }

        public UpdateException(ErrorCode errorCode, string errorParameter)
        {
            this.ErrorCode = errorCode;
            this.ErrorParameter = errorParameter;
        }

        public string ErrorArgumentId { get; set; }
        public string ErrorParameter { get; set; }
        public ErrorCode ErrorCode { get; set; }

        public abstract string ErrorMessage();
    }
}
