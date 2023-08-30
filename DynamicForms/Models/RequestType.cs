using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicForms.Models
{
    public enum RequestType
    {
        StartForm = 1,
        InputValue,
        NextStep,
        PreviousStep,
        SubmitForm,
    }
}