﻿using System.Net;

namespace Shared.ErrorModels
{
    public class ValidationErrorToReturn
    {
        public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
        public string Message { get; set; } = "Validation Failed";
        public IEnumerable<ValidationError> ValidationErrors { get; set; } = [];

    }
}
